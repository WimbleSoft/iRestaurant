using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iRestaurant.Infrastructure;
using iRestaurant.Models;
using iRestaurant.Security;

namespace iRestaurant.Controllers
{
    /// <summary>
    /// Cashier (register) panel: open table sessions, partial and full payments, bills, receipts
    /// and the daily revenue report.
    ///
    /// The views still post <c>branchId</c>; it is ignored and every record is checked against the
    /// branch of the signed-in session.
    /// </summary>
    [RequireLogin(AuthArea.Cashier)]
    public class CashierController : BaseController
    {
        #region Request models

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

        public class Revenue
        {
            public string paymentTypeName { get; set; }
            public double amount { get; set; }
            public string currencyName { get; set; }
        }

        #endregion

        #region Pages

        [AllowAnonymous]
        public ActionResult Login(EmployeeAccountViewModel model, bool? webView, string loginToken)
        {
            return EmployeeLogin(model, webView, loginToken, AuthArea.Cashier, "Cashier");
        }

        /// <summary>Asks for the manager's password before leaving the register screen.</summary>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckLogout(int employeeId, string employeePassword)
        {
            return ConfirmPasswordAndSignOut(employeeId, employeePassword);
        }

        /// <param name="id">Optional floor filter.</param>
        public ActionResult Cashier(int? id)
        {
            int branchId = CurrentBranchId;
            ViewModelDemoVM vm = BuildBranchViewModel(branchId, includeDeletedEmployees: true, onlyActivePaymentTypes: false);
            vm.TableSessions = db.TableSessions
                .Where(x => x.branchId == branchId && x.isPaid == false && (id == null || id == 0 || x.Tables.floorId == id))
                .ToList();
            ViewBag.floorId = id ?? 0;
            return View(vm);
        }

        public ActionResult Tables()
        {
            int branchId = CurrentBranchId;
            ViewModelDemoVM vm = BuildBranchViewModel(branchId, includeDeletedEmployees: true, onlyActivePaymentTypes: false);
            vm.TableSessions = vm.TableSessions.Where(x => x.isPaid == false).ToList();
            return View(vm);
        }

        [HttpPost]
        public ActionResult TableSessions(int tableSessionId)
        {
            int branchId = CurrentBranchId;
            ViewModelDemoVM vm = BuildBranchViewModel(branchId, includeDeletedEmployees: true, onlyActivePaymentTypes: false);
            vm.TableSessions = vm.TableSessions.Where(x => x.isPaid == false && x.tableSessionId == tableSessionId).ToList();
            return View(vm);
        }

        #endregion

        #region Totals and reports

        /// <summary>Revenue per payment type in the given period; deleted payment types are summed per currency.</summary>
        [HttpPost]
        public JsonResult CashierReport(DateTime startDate, DateTime endDate)
        {
            int branchId = CurrentBranchId;
            List<Payments> payments = db.Payments.Where(x => x.branchId == branchId && x.paymentDate >= startDate && x.paymentDate <= endDate).ToList();
            var revenue = new List<Revenue>();

            foreach (PaymentTypes type in db.PaymentTypes.Where(x => x.branchId == branchId && !x.isDeleted).ToList())
            {
                revenue.Add(new Revenue
                {
                    paymentTypeName = type.paymentTypeName,
                    amount = payments.Where(x => x.paymentTypeId == type.paymentTypeId).Sum(x => x.paymentAmount),
                    currencyName = type.Currencies.currencyName
                });
            }

            foreach (var deleted in payments.Where(x => x.PaymentTypes.isDeleted).GroupBy(x => x.PaymentTypes.currencyId))
            {
                Currencies currency = db.Currencies.Find(deleted.Key);
                revenue.Add(new Revenue
                {
                    paymentTypeName = "Deleted type",
                    amount = deleted.Sum(x => x.paymentAmount),
                    currencyName = currency != null ? currency.currencyName : "?"
                });
            }
            return JsonCode(revenue);
        }

        /// <summary>Totals for the current business day: still open at tables vs. already paid at the register.</summary>
        [HttpPost]
        public JsonResult Footer()
        {
            int branchId = CurrentBranchId;
            DateTime dayStart, dayEnd;
            BusinessDay(out dayStart, out dayEnd);

            List<OrderProducts> unpaid = db.OrderProducts
                .Where(x => x.Orders.branchId == branchId && x.Orders.orderDate >= dayStart && x.Orders.orderDate <= dayEnd
                            && !x.isPaid && x.Orders.TableSessions.isPaid == false)
                .ToList();

            double atTables = SalesTotal(unpaid, onlyUnpaidExtras: true);
            double atCashier = db.Payments.Where(x => x.branchId == branchId && x.paymentDate >= dayStart && x.paymentDate <= dayEnd)
                                          .Sum(x => (double?)x.paymentAmount) ?? 0;

            return JsonCode(new { atTablesTotalObj = atTables, atCashierTotalObj = atCashier });
        }

        [HttpPost]
        public JsonResult GetDailyRevenue()
        {
            int branchId = CurrentBranchId;
            DateTime dayStart, dayEnd;
            BusinessDay(out dayStart, out dayEnd);

            var revenue = db.PaymentTypes.Where(x => x.branchId == branchId).ToList().Select(type => new Revenue
            {
                paymentTypeName = type.paymentTypeName,
                amount = type.Payments.Where(x => x.paymentDate >= dayStart && x.paymentDate <= dayEnd).Sum(x => x.paymentAmount),
                currencyName = type.Currencies.currencyName
            }).ToList();
            return JsonCode(revenue);
        }

        /// <summary>Prints the employee-performance and payment-method report for a period.</summary>
        [HttpPost]
        public JsonResult GetReport(DateTime startDate, DateTime endDate)
        {
            int branchId = CurrentBranchId;
            endDate = endDate.AddSeconds(59).AddMilliseconds(999);

            List<OrderProducts> sold = db.OrderProducts
                .Where(x => x.Orders.branchId == branchId && x.paidDate >= startDate && x.paidDate <= endDate)
                .ToList();
            double grandTotal = SalesTotal(sold, onlyUnpaidExtras: false);

            var employees = new List<EmployeePerformance>();
            foreach (Employees employee in db.Employees.Where(x => x.branchId == branchId && !x.isDeleted).ToList())
            {
                double amount = SalesTotal(sold.Where(x => x.Orders.employeeId == employee.employeeId).ToList(), onlyUnpaidExtras: false);
                employees.Add(new EmployeePerformance
                {
                    employeeId = employee.employeeId,
                    employeeName = employee.employeeName,
                    employeeAmount = amount,
                    employeePercent = Share(amount, grandTotal)
                });
            }

            double deletedAmount = SalesTotal(sold.Where(x => x.Orders.Employees.isDeleted).ToList(), onlyUnpaidExtras: false);
            employees.Add(new EmployeePerformance { employeeId = 0, employeeName = "DELETED", employeeAmount = deletedAmount, employeePercent = Share(deletedAmount, grandTotal) });

            List<Payments> payments = db.Payments.Where(x => x.branchId == branchId && x.paymentDate >= startDate && x.paymentDate <= endDate).ToList();
            double paymentTotal = payments.Sum(x => x.paymentAmount);
            List<PaymentMethodReport> methods = payments
                .GroupBy(x => x.PaymentTypes.paymentTypeName)
                .Select(g => new PaymentMethodReport { methodName = g.Key, methodAmount = g.Sum(x => x.paymentAmount), methodPercent = Share(g.Sum(x => x.paymentAmount), paymentTotal) })
                .ToList();

            Printer().PrintReport(employees, methods, startDate, endDate);
            return JsonCode(true);
        }

        [HttpPost]
        public JsonResult CountTableSessions(int? id)
        {
            int branchId = CurrentBranchId;
            return JsonCode(db.TableSessions.Count(x => x.branchId == branchId && x.isPaid == false && (id == null || x.Tables.floorId == id)));
        }

        #endregion

        #region Payments

        [HttpPost]
        public JsonResult IsFullyPaid(int tableSessionId)
        {
            return JsonCode(OwnTableSession(tableSessionId) != null && CloseIfFullyPaid(tableSessionId));
        }

        /// <summary>Marks the selected items that have extras as paid.</summary>
        [HttpPost]
        public JsonResult WithExtrasCheckboxValuesArrayHandler(List<WithExtrasCheckbox> withExtrasCheckboxValuesArray)
        {
            foreach (WithExtrasCheckbox selected in withExtrasCheckboxValuesArray ?? new List<WithExtrasCheckbox>())
            {
                OrderProducts item = OwnOrderProduct(selected.orderProductId);
                if (item != null)
                    MarkPaid(item);
            }
            db.SaveChanges();
            return JsonCode(true);
        }

        /// <summary>Marks the selected campaigns (and all of their items) as paid.</summary>
        [HttpPost]
        public JsonResult CampaignCheckboxValuesArrayHandler(List<CampaignCheckbox> campaignCheckboxValuesArray)
        {
            foreach (CampaignCheckbox selected in campaignCheckboxValuesArray ?? new List<CampaignCheckbox>())
            {
                TableSessionCampaigns campaign = OwnTableSessionCampaign(selected.tableSessionCampaignId);
                if (campaign == null)
                    continue;
                foreach (OrderProducts item in db.OrderProducts.Where(x => x.tableSessionCampaignId == campaign.tableSessionCampaignId).ToList())
                    MarkPaid(item);
                campaign.isPaid = true;
            }
            db.SaveChanges();
            return JsonCode(true);
        }

        /// <summary>Marks the given quantity of unpaid items without extras as paid.</summary>
        [HttpPost]
        public JsonResult WithoutExtrasHandler(WithoutExtras[] withoutExtras)
        {
            foreach (WithoutExtras selected in withoutExtras ?? new WithoutExtras[0])
            {
                foreach (OrderProducts item in UnpaidItemsWithoutExtras(selected).ToList())
                    MarkPaid(item);
            }
            db.SaveChanges();
            return JsonCode(true);
        }

        [HttpPost]
        public JsonResult TakePayment(Payments payment)
        {
            if (payment == null || double.IsNaN(payment.paymentAmount) || double.IsInfinity(payment.paymentAmount)
                || OwnTableSession(payment.tableSessionId) == null || OwnPaymentType(payment.paymentTypeId) == null)
                return JsonCode(NotAllowed);

            db.Payments.Add(new Payments
            {
                paymentAmount = payment.paymentAmount,
                paymentTypeId = payment.paymentTypeId,
                tableSessionId = payment.tableSessionId,
                paymentDate = DateTime.Now,
                branchId = CurrentBranchId
            });
            db.SaveChanges();
            return JsonCode(true);
        }

        /// <summary>Returns the table session id of the deleted payment so the view can refresh it.</summary>
        [HttpPost]
        public JsonResult DeletePayment(int paymentId)
        {
            int branchId = CurrentBranchId;
            Payments payment = db.Payments.FirstOrDefault(x => x.paymentId == paymentId && x.branchId == branchId);
            if (payment == null)
                return JsonCode(NotAllowed);

            int tableSessionId = payment.tableSessionId;
            db.Payments.Remove(payment);
            db.SaveChanges();
            return JsonCode(tableSessionId);
        }

        #endregion

        #region Cancelling items

        [HttpPost]
        public JsonResult CancelWithExtrasProduct(WithExtrasCheckbox withExtrasCheckboxValuesArray)
        {
            OrderProducts item = withExtrasCheckboxValuesArray == null ? null : OwnOrderProduct(withExtrasCheckboxValuesArray.orderProductId);
            if (item != null)
            {
                RemoveItem(item);
                db.SaveChanges();
            }
            return JsonCode(true);
        }

        [HttpPost]
        public JsonResult CancelCampaign(CampaignCheckbox campaignCheckboxValuesArray)
        {
            TableSessionCampaigns campaign = campaignCheckboxValuesArray == null ? null : OwnTableSessionCampaign(campaignCheckboxValuesArray.tableSessionCampaignId);
            if (campaign != null)
            {
                foreach (OrderProducts item in db.OrderProducts.Where(x => x.tableSessionCampaignId == campaign.tableSessionCampaignId).ToList())
                    RemoveItem(item);
                db.TableSessionCampaigns.Remove(campaign);
                db.SaveChanges();
            }
            return JsonCode(true);
        }

        [HttpPost]
        public JsonResult CancelWithoutExtrasProduct(WithoutExtras withoutExtras)
        {
            if (withoutExtras != null)
            {
                foreach (OrderProducts item in UnpaidItemsWithoutExtras(withoutExtras).ToList())
                    RemoveItem(item);
                db.SaveChanges();
            }
            return JsonCode(true);
        }

        #endregion

        #region Tables, waiter calls and bills

        [HttpPost]
        public JsonResult ChangeTable(int tableSessionId, int newTableId)
        {
            TableSessions session = OwnTableSession(tableSessionId);
            Tables newTable = OwnTable(newTableId);
            if (session == null || newTable == null)
                return JsonCode(NotAllowed);

            if (session.Tables != null)
                session.Tables.status = false;
            session.tableId = newTable.tableId;
            newTable.status = true;
            db.SaveChanges();
            return JsonCode(true);
        }

        [HttpPost]
        public JsonResult CloseTableSession(int tableSessionId)
        {
            TableSessions session = OwnTableSession(tableSessionId);
            if (session == null || db.OrderProducts.Any(x => x.Orders.tableSessionId == tableSessionId && !x.isPaid))
                return JsonCode(false);

            Close(session);
            db.SaveChanges();
            return JsonCode(true);
        }

        [HttpPost]
        public JsonResult IsAnyWaiterCall()
        {
            int branchId = CurrentBranchId;
            return JsonCode(db.TableSessions.Where(x => x.branchId == branchId && x.waiterRequested).Select(x => x.tableSessionId).ToList());
        }

        [HttpPost]
        public JsonResult SendWaiter(int tableSessionId)
        {
            TableSessions session = OwnTableSession(tableSessionId);
            if (session == null)
                return JsonCode(NotAllowed);
            session.waiterRequested = false;
            db.SaveChanges();
            return JsonCode(true);
        }

        [HttpPost]
        public JsonResult IsAnyBillRequest()
        {
            int branchId = CurrentBranchId;
            return JsonCode(db.TableSessions.Where(x => x.branchId == branchId && x.billRequested).Select(x => x.tableSessionId).ToList());
        }

        /// <summary>Prints the full bill of a table.</summary>
        [HttpPost]
        public JsonResult PrintBillReceipt(int tableSessionId)
        {
            TableSessions session = OwnTableSession(tableSessionId);
            if (session == null)
                return JsonCode(NotAllowed);
            return JsonCode(PrintBill(session, onlyUnpaid: false));
        }

        /// <summary>Answers a "bill please" request: clears the flag and prints the unpaid items.</summary>
        [HttpPost]
        public JsonResult SendBill(int tableSessionId)
        {
            TableSessions session = OwnTableSession(tableSessionId);
            if (session == null)
                return JsonCode(NotAllowed);
            session.billRequested = false;
            db.SaveChanges();
            return JsonCode(PrintBill(session, onlyUnpaid: true));
        }

        /// <summary>Prints a receipt for the items selected for a partial payment.</summary>
        [HttpPost]
        public JsonResult PrintReceipt(WithoutExtras[] withoutExtras, List<CampaignCheckbox> campaignCheckboxValuesArray, List<WithExtrasCheckbox> withExtrasCheckboxValuesArray)
        {
            var receipt = new List<ReceiptItem>();

            foreach (WithoutExtras selected in withoutExtras ?? new WithoutExtras[0])
            {
                if (OwnTableSession(selected.tableSessionId) == null)
                    continue;
                int branchId = CurrentBranchId;
                ProductOptions option = db.ProductOptions.FirstOrDefault(x => x.productOptionId == selected.productOptionId && x.Products.branchId == branchId);
                if (option == null || selected.orderQuantity <= 0)
                    continue;
                receipt.Add(new ReceiptItem
                {
                    productQuantity = selected.orderQuantity,
                    extraTotal = 0,
                    productName = option.Products.productName + " - " + option.productOptionName,
                    productOptionPriceTotal = option.productOptionPrice + option.Products.productPrice
                });
            }

            foreach (WithExtrasCheckbox selected in withExtrasCheckboxValuesArray ?? new List<WithExtrasCheckbox>())
            {
                OrderProducts item = OwnOrderProduct(selected.orderProductId);
                if (item == null)
                    continue;
                double extras = item.OrderProductExtras.Sum(x => x.ProductExtras.productExtraPrice);
                receipt.Add(new ReceiptItem
                {
                    productQuantity = 1,
                    extraTotal = extras,
                    productName = MarkExtras(item.Products.productName + " - " + item.ProductOptions.productOptionName, extras > 0 || item.ProductOptions.productOptionPrice > 0),
                    productOptionPriceTotal = item.ProductOptions.productOptionPrice + item.Products.productPrice
                });
            }

            foreach (CampaignCheckbox selected in campaignCheckboxValuesArray ?? new List<CampaignCheckbox>())
            {
                TableSessionCampaigns campaign = OwnTableSessionCampaign(selected.tableSessionCampaignId);
                if (campaign == null)
                    continue;
                List<OrderProducts> items = db.OrderProducts.Where(x => x.tableSessionCampaignId == campaign.tableSessionCampaignId).ToList();
                double extras = items.SelectMany(x => x.OrderProductExtras).Sum(x => x.ProductExtras.productExtraPrice);
                double options = items.Sum(x => x.ProductOptions.productOptionPrice);
                receipt.Add(new ReceiptItem
                {
                    productQuantity = 1,
                    extraTotal = extras,
                    productName = MarkExtras(campaign.Campaigns.campaignName, extras > 0 || options > 0),
                    productOptionPriceTotal = options + campaign.Campaigns.campaignPrice
                });
            }

            return JsonCode(Printer().PrintReceipt(receipt));
        }

        #endregion

        #region Helpers

        private ReceiptPrinter Printer() => new ReceiptPrinter(db.Branches.Find(CurrentBranchId));

        /// <summary>The register's business day runs from 03:00 to 03:00 the next morning.</summary>
        private static void BusinessDay(out DateTime start, out DateTime end)
        {
            DateTime now = DateTime.Now;
            if (now.TimeOfDay <= TimeSpan.FromHours(3))
            {
                start = now.AddHours(-24);
                end = now;
            }
            else
            {
                start = now.Date.AddHours(3);
                end = now.Date.AddDays(1).AddHours(3);
            }
        }

        private static double Share(double part, double total) => total > 0 ? part * 100 / total : 0;

        /// <summary>
        /// Sales value of a set of ordered items: product + option + extras for regular items, and
        /// campaign price + options + extras (once per campaign) for campaign items.
        /// </summary>
        private static double SalesTotal(List<OrderProducts> items, bool onlyUnpaidExtras)
        {
            Func<OrderProducts, double> extras = x => x.OrderProductExtras.Where(e => !onlyUnpaidExtras || !e.isPaid).Sum(e => e.ProductExtras.productExtraPrice);

            double total = items.Where(x => x.tableSessionCampaignId == null)
                                .Sum(x => x.Products.productPrice + x.ProductOptions.productOptionPrice + extras(x));

            foreach (var campaign in items.Where(x => x.tableSessionCampaignId != null).GroupBy(x => x.tableSessionCampaignId))
                total += campaign.Sum(x => x.ProductOptions.productOptionPrice + extras(x)) + campaign.First().TableSessionCampaigns.Campaigns.campaignPrice;
            return total;
        }

        private IQueryable<OrderProducts> UnpaidItemsWithoutExtras(WithoutExtras selected)
        {
            int branchId = CurrentBranchId;
            return db.OrderProducts
                .Where(x => x.Orders.branchId == branchId && x.productOptionId == selected.productOptionId && !x.isPaid
                            && x.Orders.tableSessionId == selected.tableSessionId && !x.OrderProductExtras.Any())
                .OrderBy(x => x.orderProductId)
                .Take(Math.Max(0, selected.orderQuantity));
        }

        private static void MarkPaid(OrderProducts item)
        {
            foreach (OrderProductExtras extra in item.OrderProductExtras)
                extra.isPaid = true;
            item.isPaid = true;
            if (item.paidDate == null)
                item.paidDate = DateTime.Now;
        }

        private void RemoveItem(OrderProducts item)
        {
            db.OrderProductExtras.RemoveRange(item.OrderProductExtras.ToList());
            db.OrderProducts.Remove(item);
        }

        private static string MarkExtras(string name, bool hasExtras)
        {
            if (!hasExtras)
                return name;
            name = name.Length > 24 ? name.Substring(0, 24) : name.PadRight(24);
            return name + " (Ex.)";
        }

        private static void Close(TableSessions session)
        {
            session.isPaid = true;
            session.tableSessionClosingDate = DateTime.Now;
            if (session.Tables != null)
                session.Tables.status = false;
        }

        /// <summary>When everything on the table is paid: print the bill, close the session and free the table.</summary>
        private bool CloseIfFullyPaid(int tableSessionId)
        {
            bool fullyPaid = !db.OrderProducts.Any(x => x.Orders.tableSessionId == tableSessionId && !x.isPaid)
                             && !db.OrderProductExtras.Any(x => x.OrderProducts.Orders.tableSessionId == tableSessionId && !x.isPaid);
            if (!fullyPaid)
                return false;

            TableSessions session = OwnTableSession(tableSessionId);
            PrintBill(session, onlyUnpaid: false);
            Close(session);
            db.SaveChanges();
            return true;
        }

        private bool PrintBill(TableSessions session, bool onlyUnpaid)
        {
            List<OrderProducts> items = db.OrderProducts
                .Where(x => x.Orders.tableSessionId == session.tableSessionId && (!onlyUnpaid || !x.isPaid))
                .ToList();
            var bill = new List<BillItem>();

            foreach (var group in items.Where(x => !x.OrderProductExtras.Any() && x.tableSessionCampaignId == null).GroupBy(x => x.productOptionId))
            {
                OrderProducts first = group.First();
                bill.Add(new BillItem
                {
                    employeeName = first.Orders.Employees.employeeName,
                    productQuantity = group.Count(),
                    productName = first.Products.productName + " - " + first.ProductOptions.productOptionName,
                    extraTotal = 0,
                    productOptionPriceTotal = first.Products.productPrice + first.ProductOptions.productOptionPrice
                });
            }

            foreach (OrderProducts item in items.Where(x => x.OrderProductExtras.Any() && x.tableSessionCampaignId == null))
            {
                double extras = item.OrderProductExtras.Where(x => !onlyUnpaid || !x.isPaid).Sum(x => x.ProductExtras.productExtraPrice);
                bill.Add(new BillItem
                {
                    employeeName = item.Orders.Employees.employeeName,
                    productQuantity = 1,
                    extraTotal = extras,
                    productName = MarkExtras(item.Products.productName + " - " + item.ProductOptions.productOptionName, extras > 0 || item.ProductOptions.productOptionPrice > 0),
                    productOptionPriceTotal = item.Products.productPrice + item.ProductOptions.productOptionPrice
                });
            }

            foreach (var campaign in items.Where(x => x.tableSessionCampaignId != null).GroupBy(x => x.tableSessionCampaignId))
            {
                OrderProducts first = campaign.First();
                double extras = campaign.SelectMany(x => x.OrderProductExtras).Where(x => !onlyUnpaid || !x.isPaid).Sum(x => x.ProductExtras.productExtraPrice);
                double options = campaign.Sum(x => x.ProductOptions.productOptionPrice);
                bill.Add(new BillItem
                {
                    employeeName = first.Orders.Employees.employeeName,
                    productQuantity = 1,
                    extraTotal = extras,
                    productName = MarkExtras(first.TableSessionCampaigns.Campaigns.campaignName, extras > 0 || options > 0),
                    productOptionPriceTotal = options + first.TableSessionCampaigns.Campaigns.campaignPrice
                });
            }

            List<string> employees = bill.Select(x => x.employeeName).Distinct().ToList();
            string tableName = session.Tables != null ? session.Tables.tableName : string.Empty;
            return Printer().PrintBill(bill, employees, tableName);
        }

        private TableSessions OwnTableSession(int id) { int b = CurrentBranchId; return db.TableSessions.FirstOrDefault(x => x.tableSessionId == id && x.branchId == b); }
        private TableSessionCampaigns OwnTableSessionCampaign(int id) { int b = CurrentBranchId; return db.TableSessionCampaigns.FirstOrDefault(x => x.tableSessionCampaignId == id && x.TableSessions.branchId == b); }
        private OrderProducts OwnOrderProduct(int id) { int b = CurrentBranchId; return db.OrderProducts.FirstOrDefault(x => x.orderProductId == id && x.Orders.branchId == b); }
        private Tables OwnTable(int id) { int b = CurrentBranchId; return db.Tables.FirstOrDefault(x => x.tableId == id && x.Floors.branchId == b); }
        private PaymentTypes OwnPaymentType(int id) { int b = CurrentBranchId; return db.PaymentTypes.FirstOrDefault(x => x.paymentTypeId == id && x.branchId == b && !x.isDeleted); }

        #endregion
    }
}
