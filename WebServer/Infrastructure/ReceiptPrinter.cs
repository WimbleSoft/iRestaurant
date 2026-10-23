using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using iRestaurant.Models;
using iRestaurant.Security;

namespace iRestaurant.Infrastructure
{
    /// <summary>
    /// Formats ESC/POS documents (bills, receipts, reports, kitchen tickets) and sends them to the
    /// branch's network thermal printers on port 9100.
    ///
    /// The old implementation was a controller whose printer addresses lived in static fields
    /// shared by every tenant, and whose print methods were reachable as public URLs.
    /// This class is created per request for one branch and is not exposed to routing.
    /// </summary>
    public sealed class ReceiptPrinter
    {
        private const char ESC = '\x1b';
        private const char GS = '\x1d';
        private const int LineWidth = 48;
        private const int PrinterPort = 9100;
        private const int TimeoutMilliseconds = 5000;
        private const int CodePage = 857; // Turkish OEM code page supported by most thermal printers

        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        private static readonly string Divider = new string('-', LineWidth);
        private static readonly string DoubleDivider = new string('=', LineWidth);

        private readonly string _header;
        private readonly string _cashierPrinterIp;
        private readonly string _employeePrinterIp;
        private readonly bool _printEmployeeCopy;

        public ReceiptPrinter(Branches branch)
        {
            _header = branch.Companies != null && !string.IsNullOrWhiteSpace(branch.Companies.companyName)
                ? branch.Companies.companyName
                : AppSettings.ReceiptHeader;
            _cashierPrinterIp = branch.cashierPrinterIp;
            _employeePrinterIp = branch.employeePrinterIp;
            _printEmployeeCopy = branch.isEmployeePrinterAutomatic;
        }

        #region Cashier documents

        public bool PrintReport(List<EmployeePerformance> employees, List<PaymentMethodReport> paymentMethods, DateTime startDate, DateTime endDate)
        {
            var sb = Begin();
            Center(sb);
            sb.Append(_header).Append('\n');
            sb.Append("CASHIER\n");
            sb.Append(Fit("---- " + startDate.ToString("dd/MM/yyyy HH:mm", Culture) + "  --  " + endDate.ToString("dd/MM/yyyy HH:mm", Culture) + " ----", LineWidth, center: true)).Append('\n');
            sb.Append(DoubleDivider).Append('\n');
            sb.Append(Fit("REVENUE REPORT FOR THE PERIOD", LineWidth, center: true)).Append('\n');
            sb.Append(DoubleDivider).Append('\n');

            sb.Append(Fit("EMPLOYEE PERFORMANCE", LineWidth, center: true)).Append('\n');
            sb.Append(ReportLine("EMPLOYEE", "SHARE", "REVENUE")).Append('\n');
            Left(sb);
            foreach (EmployeePerformance e in employees)
                sb.Append(ReportLine(e.employeeName, Percent(e.employeePercent), Money(e.employeeAmount) + " TL")).Append('\n');
            Right(sb);
            sb.Append("TOTAL: ").Append(Money(employees.Sum(x => x.employeeAmount))).Append(" TL\n");

            Center(sb);
            sb.Append(DoubleDivider).Append('\n');
            sb.Append(Fit("PAYMENT METHODS", LineWidth, center: true)).Append('\n');
            sb.Append(ReportLine("METHOD", "SHARE", "PAID")).Append('\n');
            Left(sb);
            foreach (PaymentMethodReport m in paymentMethods)
                sb.Append(ReportLine(m.methodName, Percent(m.methodPercent), Money(m.methodAmount) + " TL")).Append('\n');
            Right(sb);
            sb.Append("TOTAL: ").Append(Money(paymentMethods.Sum(x => x.methodAmount))).Append(" TL\n");

            return Send(_cashierPrinterIp, End(sb, cut: true));
        }

        public bool PrintReceipt(List<ReceiptItem> items)
        {
            var sb = Begin();
            Center(sb);
            sb.Append(_header).Append("\n\n");
            Right(sb);
            sb.Append(DateTime.Now.ToString("dd.MM.yyyy", Culture)).Append('\n');
            sb.Append(DateTime.Now.ToString("HH:mm", Culture)).Append('\n');
            Center(sb);
            sb.Append(Divider).Append('\n');
            sb.Append(ItemHeader()).Append('\n');
            Left(sb);
            foreach (ReceiptItem item in items)
                sb.Append(ItemLine(item.productName, item.productQuantity, item.productQuantity * (item.productOptionPriceTotal + item.extraTotal))).Append('\n');
            Center(sb);
            sb.Append(Divider).Append('\n');
            sb.Append("Total: ").Append(Money(items.Sum(x => (x.productOptionPriceTotal + x.extraTotal) * x.productQuantity))).Append(" TL\n");
            return Send(_cashierPrinterIp, End(sb, cut: true));
        }

        public bool PrintBill(List<BillItem> items, List<string> employeeNames, string tableName)
        {
            var sb = Begin();
            Center(sb);
            sb.Append(_header).Append('\n');
            Left(sb);
            sb.Append(TwoColumns("TABLE: " + tableName, DateTime.Now.ToString("dd.MM.yyyy", Culture))).Append('\n');
            sb.Append(TwoColumns("EMPLOYEE: " + string.Join("-", employeeNames), DateTime.Now.ToString("HH:mm", Culture))).Append('\n');
            Center(sb);
            sb.Append(Divider).Append('\n');
            sb.Append(ItemHeader()).Append('\n');
            Left(sb);
            foreach (BillItem item in items)
                sb.Append(ItemLine(item.productName, item.productQuantity, item.productQuantity * (item.productOptionPriceTotal + item.extraTotal))).Append('\n');
            Center(sb);
            sb.Append(Divider).Append('\n');
            sb.Append("Total: ").Append(Money(items.Sum(x => (x.productOptionPriceTotal + x.extraTotal) * x.productQuantity))).Append(" TL\n");
            return Send(_cashierPrinterIp, End(sb, cut: true));
        }

        #endregion

        #region Kitchen tickets

        /// <summary>
        /// Prints one ticket per item on its department's printer (bar, kitchen, ...) and, when the
        /// branch enables it, a summary copy for the waiter.
        /// </summary>
        public void PrintKitchenTickets(List<KitchenTicketItem> items, IEnumerable<Departments> activeDepartments)
        {
            HashSet<string> activeIps = new HashSet<string>(activeDepartments.Where(x => x.isActive).Select(x => x.departmentIp ?? string.Empty));

            foreach (KitchenTicketItem item in items)
            {
                if (!activeIps.Contains(item.departmentIp ?? string.Empty))
                    continue;
                if (Send(item.departmentIp, DepartmentTicket(item)))
                    LaunchPrinterHelper(item.departmentName);
            }

            if (_printEmployeeCopy && items.Count > 0 && activeIps.Contains(_employeePrinterIp ?? string.Empty))
                Send(_employeePrinterIp, EmployeeTicket(items));
        }

        private byte[] DepartmentTicket(KitchenTicketItem item)
        {
            var sb = Begin();
            Center(sb);
            sb.Append(_header).Append('\n');
            sb.Append(item.departmentName).Append('\n');
            Right(sb);
            sb.Append(item.orderDate.ToString("dd.MM.yyyy", Culture)).Append('\n');
            sb.Append(item.orderDate.ToString("HH:mm", Culture)).Append('\n');
            Center(sb);
            sb.Append(Divider).Append('\n');
            Left(sb);
            sb.Append("Floor: ").Append(item.floorName).Append(" | Table: ").Append(item.tableName).Append('\n');
            sb.Append(Divider).Append('\n');
            sb.Append("Employee: ").Append(item.employeeName).Append('\n');
            sb.Append(Divider).Append('\n');
            sb.Append("Product: ").Append(item.productName).Append(' ').Append(item.productOptionName).Append('\n');
            if (item.extras.Count > 0)
            {
                sb.Append("Extras:\n");
                foreach (string extra in item.extras)
                    sb.Append("  ").Append(extra).Append('\n');
            }
            else
            {
                Right(sb);
                sb.Append(item.quantity).Append(" pcs\n");
                Left(sb);
            }
            sb.Append(Divider).Append('\n');
            sb.Append("Order note:\n").Append(item.orderNote).Append('\n');
            return End(sb, cut: false);
        }

        private byte[] EmployeeTicket(List<KitchenTicketItem> items)
        {
            KitchenTicketItem first = items[0];
            var sb = Begin();
            Center(sb);
            sb.Append(_header).Append('\n');
            sb.Append("EMPLOYEE\n");
            Right(sb);
            sb.Append(first.orderDate.ToString("dd.MM.yyyy", Culture)).Append('\n');
            sb.Append(first.orderDate.ToString("HH:mm", Culture)).Append('\n');
            Left(sb);
            sb.Append("Floor: ").Append(first.floorName).Append(" | Table: ").Append(first.tableName).Append('\n');
            sb.Append("Employee: ").Append(first.employeeName).Append('\n');
            sb.Append(new string('_', LineWidth)).Append('\n');

            List<KitchenTicketItem> plain = items.Where(x => x.extras.Count == 0).ToList();
            if (plain.Count > 0)
            {
                sb.Append(Fit("|Product|", 34)).Append(Fit("|Qty|", 14)).Append('\n');
                foreach (KitchenTicketItem item in plain)
                    sb.Append(Fit(item.productName + " " + item.productOptionName, 34)).Append(Fit("x" + item.quantity, 14)).Append('\n');
                sb.Append(DoubleDivider).Append('\n');
            }
            foreach (KitchenTicketItem item in items.Where(x => x.extras.Count > 0))
            {
                sb.Append("Product: ").Append(item.productName).Append(' ').Append(item.productOptionName).Append('\n');
                sb.Append("Extras: ").Append(string.Join(" ", item.extras)).Append('\n');
            }
            sb.Append("Order note:\n ").Append(first.orderNote).Append('\n');
            return End(sb, cut: true);
        }

        /// <summary>
        /// Optionally starts a local helper program after a ticket is printed (configured with
        /// Printer:HelperExecutablePath). The department name is passed as a single, sanitized,
        /// quoted argument so it cannot inject extra command-line options.
        /// </summary>
        private static void LaunchPrinterHelper(string departmentName)
        {
            string path = AppSettings.PrinterHelperPath;
            if (string.IsNullOrEmpty(path))
                return;

            string safeName = new string((departmentName ?? string.Empty).Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_').ToArray());
            try
            {
                using (Process.Start(new ProcessStartInfo(path, "\"" + safeName + "\"") { UseShellExecute = false, CreateNoWindow = true })) { }
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Printer helper could not be started: {0}", e.Message);
            }
        }

        #endregion

        #region Formatting helpers

        private static StringBuilder Begin()
        {
            var sb = new StringBuilder();
            sb.Append(ESC).Append('@');                  // initialize
            sb.Append(ESC).Append('t').Append((char)79); // character table
            return sb;
        }

        private static byte[] End(StringBuilder sb, bool cut)
        {
            Left(sb);
            sb.Append(ESC).Append('E').Append((char)0);  // normal weight
            if (cut)
                sb.Append(GS).Append("V\x41\0");          // feed and cut
            return Encoding.GetEncoding(CodePage).GetBytes(sb.ToString());
        }

        private static void Left(StringBuilder sb) => sb.Append(ESC).Append('a').Append((char)0);
        private static void Center(StringBuilder sb) => sb.Append(ESC).Append('a').Append((char)1);
        private static void Right(StringBuilder sb) => sb.Append(ESC).Append('a').Append((char)2);

        private static string Money(double value) => value.ToString("F2", Culture);
        private static string Percent(double value) => "%" + (double.IsNaN(value) || double.IsInfinity(value) ? 0 : value).ToString("F2", Culture);

        /// <summary>Pads or truncates to exactly <paramref name="width"/> characters.</summary>
        private static string Fit(string text, int width, bool center = false, bool alignRight = false)
        {
            text = (text ?? string.Empty).Replace('\n', ' ');
            if (text.Length >= width)
                return text.Substring(0, width);
            if (alignRight)
                return text.PadLeft(width);
            if (center)
                return text.PadLeft(text.Length + (width - text.Length) / 2).PadRight(width);
            return text.PadRight(width);
        }

        /// <summary>"name x qty      amount TL" = 30 + 5 + 10 + 3 = 48 characters.</summary>
        private static string ItemLine(string name, int quantity, double amount)
            => Fit(name, 30) + Fit(" x" + quantity, 5) + Fit(Money(amount), 10, alignRight: true) + " TL";

        private static string ItemHeader() => Fit("|Product|", 30) + Fit("|Qty|", 5) + Fit("|Subtotal|", 13, alignRight: true);

        /// <summary>"name   share   amount" = 22 + 10 + 16 = 48 characters.</summary>
        private static string ReportLine(string name, string share, string amount)
            => Fit(" " + name, 22) + Fit(share, 10) + Fit(amount, 16, alignRight: true);

        private static string TwoColumns(string left, string right)
            => Fit(left, LineWidth - right.Length - 1) + " " + right;

        #endregion

        /// <summary>Sends raw bytes to a LAN printer. Refuses addresses outside private networks.</summary>
        private static bool Send(string host, byte[] data)
        {
            if (!PrinterAddress.IsAllowed(host))
            {
                Trace.TraceWarning("Refusing to print to non-private address '{0}'.", host);
                return false;
            }

            try
            {
                using (var client = new TcpClient())
                {
                    if (!client.ConnectAsync(host.Trim(), PrinterPort).Wait(TimeoutMilliseconds))
                        return false;
                    client.SendTimeout = TimeoutMilliseconds;
                    using (NetworkStream stream = client.GetStream())
                        stream.Write(data, 0, data.Length);
                }
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Printing to {0} failed: {1}", host, e.Message);
                return false;
            }
        }
    }
}
