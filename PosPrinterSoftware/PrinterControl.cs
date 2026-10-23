using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace iRestaurantPosPrinterSoftware
{
    class PrinterControl 
    {
        iRestaurantEntities iRestaurant = new iRestaurantEntities();
        public PrinterControl(int branchId)
        {
            MessageBox.Show("The printing service has started.");
            while (true)
            {
                Thread.Sleep(5000);
                iRestaurantEntities iRestaurantPrinter = new iRestaurantEntities();
                employeePrinterIp = "";
                cashierPrinterIp = "";
                foreach (var order in iRestaurantPrinter.PrintingList.Where(x => x.branchId == branchId))
                {
                    try
                    {
                        ActivatePrinters(order.orderId);
                        iRestaurantPrinter.PrintingList.RemoveRange(iRestaurantPrinter.PrintingList.Where(x => x.orderId == order.orderId));
                        
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                iRestaurantPrinter.SaveChanges();
                iRestaurantPrinter.Dispose();
                Thread.Sleep(5000);
            }
        }

        #region Models
        public class BillItem
        {
            public string productName { get; set; }
            public int productQuantity { get; set; }
            public double productOptionPriceTotal { get; set; }
            public double extraTotal { get; set; }
            public string employeeName { get; set; }
            /*public billItem()
            {
                employees = new List<string>();
            }*/
        }
        public class ReceiptItem
        {
            public string productName { get; set; }
            public int productQuantity { get; set; }
            public double productOptionPriceTotal { get; set; }
            public double extraTotal { get; set; }
        }
        public class WithExtrasCheckbox
        {
            public int orderProductId { get; set; }

        }
        public class CampaignCheckbox
        {
            public int tableSessionCampaignId { get; set; }

        }
        public class WithoutExtras
        {
            public int productOptionId { get; set; }
            public int orderQuantity { get; set; }
            public int tableSessionId { get; set; }
        }
        public class EmployeePerformance
        {
            public string employeeName { get; set; }
            public int employeeId { get; set; }
            public double employeeAmount { get; set; }
            public double employeePercent { get; set; }
        }
        public class PaymentMethodReport
        {
            public string methodName { get; set; }
            public double methodPercent { get; set; }
            public double methodAmount { get; set; }
        }
        public class WithoutExtrasToPrint
        {
            public string floorName { get; set; }
            public string employeeName { get; set; }
            public string tableName { get; set; }
            public string productName { get; set; }
            public string productOptionName { get; set; }
            public int quantity { get; set; }
            public DateTime orderDate { get; set; }
            public string departmentIp { get; set; }
            public string orderNote { get; set; }
        }
        public class WithExtrasToPrint
        {
            public string floorName { get; set; }
            public string employeeName { get; set; }
            public string tableName { get; set; }
            public string productName { get; set; }
            public string productOptionName { get; set; }
            public List<String> extras { get; set; }
            public DateTime orderDate { get; set; }
            public string departmentIp { get; set; }
            public string orderNote { get; set; }
            public WithExtrasToPrint()
            {
                extras = new List<string>();
            }
        }
        public class EmployeeToPrint
        {
            public List<WithoutExtrasToPrint> withoutExtrasToPrint;
            public List<WithExtrasToPrint> withExtrasToPrint;
            public EmployeeOrderDetails employeeOrderDetails;
            public EmployeeToPrint()
            {
                withoutExtrasToPrint = new List<WithoutExtrasToPrint>();
                withExtrasToPrint = new List<WithExtrasToPrint>();
                employeeOrderDetails = new EmployeeOrderDetails();
            }
            public class EmployeeOrderDetails
            {
                public DateTime orderDate { get; set; }
                public string floorName { get; set; }
                public string tableName { get; set; }
                public string employeeName { get; set; }
                public string orderNote { get; set; }
            }
        }

        public static string employeePrinterIp = "192.168.1.3";
        public static string cashierPrinterIp = "192.168.1.4";

        #endregion


        

        public void ActivatePrinters(int orderId)
        {
            #region 
            EmployeeToPrint employeeToPrintModel = new EmployeeToPrint();
            iRestaurantEntities cashierModel = new iRestaurantEntities();
            foreach (var order in cashierModel.Orders.Where(x => x.orderId == orderId).ToList())
            {

                #region Products without extras
                foreach (var orderProduct in cashierModel.OrderProducts.Where(x => /*x.isPaid==false && */x.orderId == order.orderId && x.OrderProductExtras.Where(y => y.orderProductId == x.orderProductId).Count() == 0 /*&& x.isConfirmed == false*/).GroupBy(x => x.productOptionId).ToList())
                {
                    WithoutExtrasToPrint withoutExtrasToPrintModel = new WithoutExtrasToPrint
                    {
                        floorName = order.TableSessions.Tables.Floors.floorName,
                        tableName = order.TableSessions.Tables.tableName,
                        orderDate = order.orderDate,
                        employeeName = order.Employees.employeeName,
                        quantity = orderProduct.Count(),
                        productOptionName = cashierModel.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().productOptionName,
                        productName = cashierModel.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().Products.productName,
                        departmentIp = cashierModel.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().Products.ProductCategories.Departments.departmentIp
                    };
                    /*foreach (var a in cashierModel.OrderProducts.Where(x => x.productOptionId == orderProduct.Key && x.orderId == order.orderId && x.OrderProductExtras.Where(y => y.orderProductId == x.orderProductId).Count() == 0).ToList())
                    {
                        a.isConfirmed = true;
                    }*/
                    string departmentName = cashierModel.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().Products.ProductCategories.Departments.departmentName;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                    bool withoutExtrasReportPrintFailed = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                    try
                    {
                        PrintWithoutExtrasReport(withoutExtrasToPrintModel, departmentName);
                    }
                    catch (Exception error)
                    {
                        if (error.Message != null) withoutExtrasReportPrintFailed = true;
                    }
                    //if (!withoutExtrasReportPrintFailed) cashierModel.SaveChanges();
                    employeeToPrintModel.withoutExtrasToPrint.Add(withoutExtrasToPrintModel);
                }
                #endregion
                #region Products with extras
                foreach (var orderProduct3 in cashierModel.OrderProducts.Where(x => /*x.isPaid == false &&*/ x.orderId == order.orderId && x.OrderProductExtras.Where(y => y.orderProductId == x.orderProductId).Count() > 0 /*&& x.isConfirmed == false*/).ToList())
                {
                    WithExtrasToPrint withExtrasToPrintModel = new WithExtrasToPrint
                    {
                        floorName = orderProduct3.Orders.TableSessions.Tables.Floors.floorName,
                        tableName = orderProduct3.Orders.TableSessions.Tables.tableName,
                        orderDate = orderProduct3.Orders.orderDate,
                        employeeName = orderProduct3.Orders.Employees.employeeName
                    };
                    withExtrasToPrintModel.extras.Clear();
                    foreach (var a in cashierModel.OrderProductExtras.Where(x => x.orderProductId == orderProduct3.orderProductId))
                    {
                        withExtrasToPrintModel.extras.Add(a.ProductExtras.productExtraName);
                    }
                    withExtrasToPrintModel.productOptionName = orderProduct3.ProductOptions.productOptionName;
                    withExtrasToPrintModel.productName = orderProduct3.Products.productName;
                    withExtrasToPrintModel.departmentIp = orderProduct3.Products.ProductCategories.Departments.departmentIp;
                    //orderProduct3.isConfirmed = true;
                    string departmentName = orderProduct3.Products.ProductCategories.Departments.departmentName;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                    bool withExtrasReportPrintFailed = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                    try
                    {
                        PrintWithExtrasReport(withExtrasToPrintModel, departmentName);
                    }
                    catch (Exception error)
                    {
                        if (error.Message != null) withExtrasReportPrintFailed = true;
                    }
                    //if(!withExtrasReportPrintFailed) cashierModel.SaveChanges();
                    employeeToPrintModel.withExtrasToPrint.Add(withExtrasToPrintModel);
                }
                #endregion
                //cashierModel.SaveChanges();
                employeeToPrintModel.employeeOrderDetails.floorName = order.TableSessions.Tables.Floors.floorName;
                employeeToPrintModel.employeeOrderDetails.tableName = order.TableSessions.Tables.tableName;
                employeeToPrintModel.employeeOrderDetails.employeeName = order.Employees.employeeName;
                employeeToPrintModel.employeeOrderDetails.orderNote = order.note;
                employeeToPrintModel.employeeOrderDetails.orderDate = order.orderDate;
                #pragma warning disable CS0219 // Variable is assigned but its value is never used
                bool employeeReportPrintFailed = false;
                #pragma warning restore CS0219 // Variable is assigned but its value is never used
                try
                {
                    PrintEmployeeReport(employeeToPrintModel.withExtrasToPrint, employeeToPrintModel.withoutExtrasToPrint, employeeToPrintModel.employeeOrderDetails);
                }
                catch (Exception error)
                {
                    if (error.Message != null) employeeReportPrintFailed = true;
                }
                //if(!employeeReportPrintFailed) cashierModel.SaveChanges();

                employeeToPrintModel.withoutExtrasToPrint.Clear();
                employeeToPrintModel.withExtrasToPrint.Clear();
                cashierModel.Dispose();
                Thread.Sleep(1000);
            }
            #endregion
            //return Json(true, JsonRequestBehavior.AllowGet);
        }

        public static void PrintWithoutExtrasReport(WithoutExtrasToPrint withoutExtrasToPrintModel, string departmentName)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantEntities yazdiriciModel = new iRestaurantEntities();

            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text
            #region Date and time
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append(departmentName + "\n");
            sb.Append(ESC + "a" + (char)2);
            sb.Append(withoutExtrasToPrintModel.orderDate.ToString("dd.MM.yyyy") + "\n");

            sb.Append(ESC + "a" + (char)2);
            sb.Append(withoutExtrasToPrintModel.orderDate.ToString("hh:mm") + "\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            #endregion Date and time
            #region Floor, table and employee
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Floor:" + withoutExtrasToPrintModel.floorName + " | ");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("Table:" + withoutExtrasToPrintModel.tableName);
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("\n");
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Employee:" + withoutExtrasToPrintModel.employeeName + "\n");
            #endregion Floor, table and employee
            #region Product info and order note
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Product:" + withoutExtrasToPrintModel.productName + " " + withoutExtrasToPrintModel.productOptionName + "\n");
            sb.Append(ESC + "a" + (char)0); // Align left

            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append(withoutExtrasToPrintModel.quantity + "Quantity" + "\n");
            sb.Append(ESC + "a" + (char)2); // Align right

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center

            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Order Note:\n");
            sb.Append(ESC + "a" + (char)0); // Align left

            sb.Append(withoutExtrasToPrintModel.orderNote);
            #endregion Product info and order note
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(57));
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(40) + Convert.ToChar(65) + Convert.ToChar(4) + Convert.ToChar(0) + Convert.ToChar(48) + Convert.ToChar(49) + Convert.ToChar(3) + Convert.ToChar(10));
            //sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] toWriteByte = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                foreach (var department in yazdiriciModel.Departments.Where(x => x.departmentIp == withoutExtrasToPrintModel.departmentIp))
                {
                    if (department.isActive == true)
                    {
                        try
                        {
                            TcpPrint(withoutExtrasToPrintModel.departmentIp, toWriteByte);
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = department.departmentName;
                            pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void PrintWithExtrasReport(WithExtrasToPrint withExtrasToPrintModel, string departmentName)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantEntities yazdiriciModel = new iRestaurantEntities();
            #region Date and time
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            sb.Append("\n");
            // Add header text

            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append(departmentName + "\n");
            sb.Append(ESC + "a" + (char)2);
            sb.Append(withExtrasToPrintModel.orderDate.ToString("dd.MM.yyyy") + "\n");


            sb.Append(ESC + "a" + (char)2);
            sb.Append(withExtrasToPrintModel.orderDate.ToString("hh:mm") + "\n");
            //sb.Append(time + ":" + dakika + "\n");

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            #endregion Date and time
            #region Floor, table and employee
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Floor:" + withExtrasToPrintModel.floorName + " | ");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("Table:" + withExtrasToPrintModel.tableName);
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("\n");
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Employee:" + withExtrasToPrintModel.employeeName + "\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            #endregion Floor, table and employee
            #region Product name, extras and order note
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Product:" + withExtrasToPrintModel.productName + " " + withExtrasToPrintModel.productOptionName + "\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Extras:\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            foreach (var aaa in withExtrasToPrintModel.extras.ToList())
            {
                sb.Append(aaa + "\n");
            }
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Order Note:\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(withExtrasToPrintModel.orderNote);
            #endregion Product name, extras and order note
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);

            // Feed and cut paper
            //sb.Append(ESC + Convert.ToChar("(")+Convert.ToChar("A") + Convert.ToChar() + Convert.ToChar() + Convert.ToChar() + Convert.ToChar())
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(57));
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(40) + Convert.ToChar(65) + Convert.ToChar(4)+ Convert.ToChar(0) + Convert.ToChar(48) + Convert.ToChar(49) + Convert.ToChar(3) + Convert.ToChar(10));
            //sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] toWriteByte = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                foreach (var department in yazdiriciModel.Departments.Where(x => x.departmentIp == withExtrasToPrintModel.departmentIp).ToList())
                {
                    if (department.isActive == true)
                    {
                        try
                        {
                            TcpPrint(withExtrasToPrintModel.departmentIp, toWriteByte);
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = department.departmentName;
                            pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void PrintEmployeeReport(List<WithExtrasToPrint> withExtrasToPrintModelList, List<WithoutExtrasToPrint> withoutExtrasToPrintModelList, EmployeeToPrint.EmployeeOrderDetails employeeOrderDetails)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantEntities yazdiriciModel = new iRestaurantEntities();
            // Initialize printer
            #region Date and time
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append("EMPLOYEE\n");
            sb.Append(ESC + "a" + (char)2);
            sb.Append(employeeOrderDetails.orderDate.ToString("dd.MM.yyyy") + "\n");

            sb.Append(ESC + "a" + (char)2);

            sb.Append(employeeOrderDetails.orderDate.ToString("hh.mm") + "\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            //sb.Append("________________________________________________\n");
            #endregion Date and time
            #region Floor, table and employee
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Floor:" + employeeOrderDetails.floorName + " | ");
            sb.Append(ESC + "a" + (char)0); // Align left

            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("Table:" + employeeOrderDetails.tableName);
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("\n");

            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Employee:" + employeeOrderDetails.employeeName + "\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            #endregion Floor, table and employee
            #region Product list
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("________________________________________________\n");
            if (withoutExtrasToPrintModelList.Count() > 0)
            {

                sb.Append("|Product |                      |Qty |          \n");
                char[] rowsArray = new char[48];
                char[] productNameArray = new char[30];
                char[] extraArray = new char[48];
                char[] productQuantityArray = new char[3];
                char[] productPriceArray = new char[10];

                sb.Append(ESC + "a" + (char)0); // Align left

                foreach (var withoutExtrasToPrintModel in withoutExtrasToPrintModelList)
                {

                    for (int i = 0; i < (withoutExtrasToPrintModel.productName + ' ' + withoutExtrasToPrintModel.productOptionName).ToCharArray().Length; i++)
                    {
                        rowsArray[i] = (withoutExtrasToPrintModel.productName + ' ' + withoutExtrasToPrintModel.productOptionName).ToCharArray()[i];
                    }
                    for (int i = (withoutExtrasToPrintModel.productName + ' ' + withoutExtrasToPrintModel.productOptionName).ToCharArray().Length; i < 33; i++)
                    {
                        rowsArray[i] = ' ';
                    }
                    rowsArray[33] = ' ';
                    rowsArray[34] = 'x';
                    for (int i = 0; i < withoutExtrasToPrintModel.quantity.ToString().ToCharArray().Length; i++)
                    {
                        rowsArray[i + 35] = withoutExtrasToPrintModel.quantity.ToString().ToCharArray()[i];
                    }
                    for (int i = 35 + withoutExtrasToPrintModel.quantity.ToString().ToCharArray().Length; i < 38; i++)
                    {
                        rowsArray[i] = ' ';
                    }

                    for (int i = 38; i <= 47; i++)
                    {
                        rowsArray[i] = ' ';
                    }

                    string str = new string(rowsArray);
                    sb.Append(str + "\n");

                }
                sb.Append(ESC + "a" + (char)1); // Align center
                sb.Append("================================================\n");
                sb.Append(ESC + "a" + (char)1); // Align center
            }
            if (withExtrasToPrintModelList.Count() > 0)
            {
                foreach (var withExtrasToPrintModel in withExtrasToPrintModelList)
                {
                    if (withExtrasToPrintModelList.Count() > 1)
                    {
                        sb.Append(ESC + "a" + (char)1); // Align center
                        sb.Append("-   -   -   -   -   -   -   -   -   -   -   -   \n");
                        sb.Append(ESC + "a" + (char)1); // Align center
                    }
                    sb.Append(ESC + "a" + (char)0); // Align left
                    sb.Append("Product: " + withExtrasToPrintModel.productName + " " + withExtrasToPrintModel.productOptionName + "\n");

                    sb.Append(ESC + "a" + (char)0); // Align left
                    sb.Append("Extras: ");
                    sb.Append(ESC + "a" + (char)0); // Align left
                    foreach (var aaa in withExtrasToPrintModel.extras.ToList())
                    {
                        sb.Append(aaa + " ");
                    }
                    sb.Append("\n");
                }
                sb.Append("================================================\n");
            }
            #endregion Product list
            #region Order note
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Order Note:\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(" " + employeeOrderDetails.orderNote + "\n");
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            #endregion Order note

            // Feed and cut paper
            sb.Append(GS + "V\x41\0");

            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] toWriteByte = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);

            try
            {
                foreach (var department in yazdiriciModel.Departments.Where(x => x.departmentIp == employeePrinterIp).ToList())
                {
                    if (department.isActive == true)
                    {
                        //TcpPrint(employeePrinterIp, toWriteByte);
                        /*
                        try
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = department.departmentName;
                            pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        */
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        public static void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            
        }

        public const char ESC = '\x1b';
        public const char FS = '\x1c';
        public const char GS = '\x1d';
        public const char NUL = '\x00';
        public static void TcpPrint(string host, byte[] data)
        {
            Socket s = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            s.Connect(host, 9100);
            s.Send(data);
            s.Disconnect(false);
        }

    }
}
