using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static iRestaurantPosPrinterSoftware.PrinterControl;

namespace iRestaurantPosPrinterSoftware
{
    class BrowserMethods
    {
        iRestaurantEntities iRestaurant = new iRestaurantEntities();
        private static ChromiumWebBrowser _instanceBrowser = null;
        
        private static Dashboard _instanceMainForm = null;

        public bool PrintReport(List<EmployeePerformance> employeePerformanceList, List<PaymentMethodReport> paymentMethodReportList, DateTime startDate, DateTime endDate)
        {
            bool status = true;
            StringBuilder sb = new StringBuilder();
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text

            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append("CASHIER\n");

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("-----" + startDate.ToString("dd/MM/yyyy HH:mm") + "  --  " + endDate.ToString("dd/MM/yyyy HH:mm") + "-----\n");
            sb.Append("================================================\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("----------Revenue Report by Date Range----------\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("================================================\n");

            sb.Append("              EMPLOYEE PERFORMANCE              \n");
            sb.Append("|EMPLOYEE|           |SHARE|    |EMP. REVENUE|  \n");
            char[] employeeRowsArray = new char[48];
            char[] paymentRowsArray = new char[48];
            sb.Append(ESC + "a" + (char)0); // Align left

            foreach (var employeePerformance in employeePerformanceList)
            {
                employeePerformance.employeeName = " " + employeePerformance.employeeName;
                string employeeAmount = employeePerformance.employeeAmount.ToString("F2");
                double employeePercent = Math.Round(employeePerformance.employeePercent, 2);
                for (int i = 0; i < employeePerformance.employeeName.ToCharArray().Length; i++)
                {
                    employeeRowsArray[i] = employeePerformance.employeeName.ToCharArray()[i];
                }
                for (int i = employeePerformance.employeeName.ToCharArray().Length; i < 20; i++)
                {
                    employeeRowsArray[i] = ' ';
                }
                employeeRowsArray[20] = ' ';
                employeeRowsArray[21] = '%';
                for (int i = 0; i < employeePercent.ToString("F2").ToCharArray().Length; i++)
                {
                    employeeRowsArray[i + 22] = employeePercent.ToString("F2").ToCharArray()[i];
                }
                for (int i = 26; i <= 44; i++)
                {
                    employeeRowsArray[i] = ' ';
                }
                for (int i = employeeAmount.ToString().ToCharArray().Length; i > 0; i--)
                {
                    employeeRowsArray[35 + i + (9 - employeeAmount.ToString().ToCharArray().Length)] = employeeAmount.ToString().ToCharArray()[i - 1];
                }
                employeeRowsArray[45] = ' ';
                employeeRowsArray[46] = 'T';
                employeeRowsArray[47] = 'L';
                string str = new string(employeeRowsArray);
                sb.Append(str + "\n");
            }
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("TOTAL: " + employeePerformanceList.Sum(x => x.employeeAmount).ToString("F2") + " TL");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)2); // Align right

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("================================================\n");
            sb.Append(ESC + "a" + (char)1); // Align center

            sb.Append("                PAYMENT METHODS                 \n");
            sb.Append("|PAY. TYPE |         |SHARE|      |CUST. PAID|  \n");

            foreach (var paymentMethod in paymentMethodReportList)
            {
                paymentMethod.methodName = " " + paymentMethod.methodName;
                string paymentMethodAmount = paymentMethod.methodAmount.ToString("F2");
                char[] odemeYonMiktar5Karakter = paymentMethodAmount.ToString().ToCharArray();

                double paymentMethodPercent = Math.Round(paymentMethod.methodPercent, 2);
                for (int i = 0; i < paymentMethod.methodName.ToCharArray().Length; i++)
                {
                    paymentRowsArray[i] = paymentMethod.methodName.ToCharArray()[i];
                }
                for (int i = paymentMethod.methodName.ToCharArray().Length; i < 20; i++)
                {
                    paymentRowsArray[i] = ' ';
                }
                paymentRowsArray[20] = ' ';
                paymentRowsArray[21] = '%';
                for (int i = 0; i < paymentMethodPercent.ToString().ToCharArray().Length; i++)
                {
                    paymentRowsArray[i + 22] = paymentMethodPercent.ToString().ToCharArray()[i];
                }

                for (int i = 26; i <= 44; i++)
                {
                    paymentRowsArray[i] = ' ';
                }
                for (int i = paymentMethodAmount.ToString().ToCharArray().Length; i > 0; i--)
                {
                    paymentRowsArray[35 + i + (9 - paymentMethodAmount.ToString().ToCharArray().Length)] = paymentMethodAmount.ToString().ToCharArray()[i - 1];
                }

                paymentRowsArray[45] = ' ';
                paymentRowsArray[46] = 'T';
                paymentRowsArray[47] = 'L';
                string str = new string(paymentRowsArray);
                sb.Append(str + "\n");

            }
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("TOTAL: " + paymentMethodReportList.Sum(x => x.methodAmount).ToString("F2") + " TL");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)2); // Align right
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            // Feed and cut paper
            sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                //TcpPrint(cashierPrinterIp, b);
                System.Windows.Forms.MessageBox.Show("Report printed on the cashier printer.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                status = false;
            }
            return status;
        }
        public bool PrintReceipt(List<ReceiptItem> receiptItemList)
        {
            bool status = true;
            StringBuilder sb = new StringBuilder();
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)2);
            sb.Append(DateTime.Now.ToShortDateString() + "\n");

            sb.Append(ESC + "a" + (char)2);
            sb.Append(DateTime.Now.ToShortTimeString() + "\n");

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append("|Product |                   |Qty | |Subtotal  |\n");
            char[] rowsArray = new char[48];
            char[] productNameArray = new char[30];
            char[] extraArray = new char[48];
            char[] productQuantityArray = new char[3];
            char[] productPriceArray = new char[10];

            sb.Append(ESC + "a" + (char)0); // Align left

            #region rowPrinting
            foreach (var receiptItem in receiptItemList)
            {

                for (int i = 0; i < receiptItem.productName.ToCharArray().Length; i++)
                {
                    rowsArray[i] = receiptItem.productName.ToCharArray()[i];
                }
                for (int i = receiptItem.productName.ToCharArray().Length; i < 30; i++)
                {
                    rowsArray[i] = ' ';
                }
                rowsArray[30] = ' ';
                rowsArray[31] = 'x';
                for (int i = 0; i < receiptItem.productQuantity.ToString().ToCharArray().Length; i++)
                {
                    rowsArray[i + 32] = receiptItem.productQuantity.ToString().ToCharArray()[i];
                }
                for (int i = 32 + receiptItem.productQuantity.ToString().ToCharArray().Length; i < 35; i++)
                {
                    rowsArray[i] = ' ';
                }

                for (int i = 35; i <= 44; i++)
                {
                    rowsArray[i] = ' ';
                }
                for (int i = 0; i < (receiptItem.productQuantity * (receiptItem.productOptionPriceTotal + receiptItem.extraTotal)).ToString("F2").ToCharArray().Length; i++)
                {
                    rowsArray[36 + i + (9 - (receiptItem.productQuantity * (receiptItem.productOptionPriceTotal + receiptItem.extraTotal)).ToString("F2").ToCharArray().Length)] = (receiptItem.productQuantity * (receiptItem.productOptionPriceTotal + receiptItem.extraTotal)).ToString("F2").ToCharArray()[i];
                }

                rowsArray[45] = ' ';
                rowsArray[46] = 'T';
                rowsArray[47] = 'L';
                string str = new string(rowsArray);
                sb.Append(str + "\n");

            }
            #endregion rowPrinting

            string str2 = new string(extraArray);
            sb.Append(str2 + "\n");

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center

            sb.Append(ESC + "a" + (char)1); // Align right
            sb.Append("Total: " + (receiptItemList.Sum(x => (x.productOptionPriceTotal + x.extraTotal) * x.productQuantity).ToString("F2")) + " TL");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)1); // Align right


            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            // Feed and cut paper
            sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                //TcpPrint(cashierPrinterIp, b);
                System.Windows.Forms.MessageBox.Show("Receipt printed on the cashier printer.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                status = false;
            }
            return status;
        }
        public bool PrintBill(List<BillItem> receiptItemList, List<string> employees, string tableName)
        {
            string abc = DateTime.Now.ToString("dd.MM.yyyy");
            bool status = true;
            StringBuilder sb = new StringBuilder();
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append(ESC + "a" + (char)0);

            #region Table and date row
            char[] tableAndDateRow = new char[48];
            for (int i = 0; i < 48; i++)
            {
                tableAndDateRow[i] = ' ';
            }
            for (int i = 0; i < 5; i++)
            {
                tableAndDateRow[i] = "TBL: "[i];
            }
            for (int i = 0; i < tableName.Length; i++)
            {
                tableAndDateRow[i + 5] = tableName[i];
            }
            tableAndDateRow[37] = ' ';
            for (int i = 0; i < 10; i++)
            {
                tableAndDateRow[i + 38] = abc[i];
            }

            string tableAndDateRowSTR = new string(tableAndDateRow);
            sb.Append(tableAndDateRowSTR + "\n");
            #endregion Table and date row

            #region Employees and time row
            char[] employeeAndTimeRow = new char[48];

            for (int i = 0; i < 48; i++)
            {
                employeeAndTimeRow[i] = ' ';
            }
            for (int i = 0; i < "EMPLOYEE: ".Length; i++)
            {
                employeeAndTimeRow[i] = "EMPLOYEE: "[i];
            }
            string employeeString = "";
            foreach (var p in employees)
            {
                employeeString += p;
                if (p != employees.Last())
                {
                    employeeString += "-";
                }
            }
            for (int i = 0; i < employeeString.Length; i++)
            {
                employeeAndTimeRow[i + 9] = employeeString[i];
            }

            for (int i = 0; i < 5; i++)
            {
                employeeAndTimeRow[i + 43] = DateTime.Now.ToShortTimeString()[i];
            }
            string employeeAndTimeRowSTR = new string(employeeAndTimeRow);
            sb.Append(employeeAndTimeRowSTR + "\n");
            #endregion Employees and time row

            sb.Append(ESC + "a" + (char)0);

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append("|Product |                   |Qty | |Subtotal  |\n");
            char[] rowsArray = new char[48];
            char[] productNameArray = new char[30];
            char[] extraArray = new char[48];
            char[] productQuantityArray = new char[3];
            char[] productPriceArray = new char[10];

            sb.Append(ESC + "a" + (char)0); // Align left
            #region printRows
            foreach (var receiptItem in receiptItemList)
            {

                for (int i = 0; i < receiptItem.productName.ToCharArray().Length; i++)
                {
                    rowsArray[i] = receiptItem.productName.ToCharArray()[i];
                }
                for (int i = receiptItem.productName.ToCharArray().Length; i < 30; i++)
                {
                    rowsArray[i] = ' ';
                }
                rowsArray[30] = ' ';
                rowsArray[31] = 'x';
                for (int i = 0; i < receiptItem.productQuantity.ToString().ToCharArray().Length; i++)
                {
                    rowsArray[i + 32] = receiptItem.productQuantity.ToString().ToCharArray()[i];
                }
                for (int i = 32 + receiptItem.productQuantity.ToString().ToCharArray().Length; i < 35; i++)
                {
                    rowsArray[i] = ' ';
                }

                for (int i = 35; i <= 44; i++)
                {
                    rowsArray[i] = ' ';
                }
                for (int i = 0; i < (receiptItem.productQuantity * (receiptItem.productOptionPriceTotal + receiptItem.extraTotal)).ToString("F2").ToCharArray().Length; i++)
                {
                    rowsArray[36 + i + (9 - (receiptItem.productQuantity * (receiptItem.productOptionPriceTotal + receiptItem.extraTotal)).ToString("F2").ToCharArray().Length)] = (receiptItem.productQuantity * (receiptItem.productOptionPriceTotal + receiptItem.extraTotal)).ToString("F2").ToCharArray()[i];
                }

                rowsArray[45] = ' ';
                rowsArray[46] = 'T';
                rowsArray[47] = 'L';
                string str = new string(rowsArray);
                sb.Append(str + "\n");

            }
            #endregion printRows

            string str2 = new string(extraArray);
            sb.Append(str2 + "\n");

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center

            sb.Append(ESC + "a" + (char)1); // Align right
            sb.Append("Total: " + (receiptItemList.Sum(x => (x.productOptionPriceTotal + x.extraTotal) * x.productQuantity).ToString("F2")) + " TL");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)1); // Align right


            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            // Feed and cut paper
            sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                //TcpPrint(cashierPrinterIp, b);
                System.Windows.Forms.MessageBox.Show("Bill printed on the cashier printer.");
                /*try
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrinterSettings.PrinterName = "CASHIER";
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            
                    pd.Print();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }*/

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                status = false;
            }
            return status;
        }
        public BrowserMethods(ChromiumWebBrowser browser, Dashboard dashboardForm)
        {
            _instanceBrowser = browser;
            _instanceMainForm = dashboardForm;
        }

        public void printBillReceipt(string tableSessionId2, string branchId2)
        {
            int tableSessionId = Convert.ToInt32(tableSessionId2);
            int branchId = Convert.ToInt32(branchId2);

            List<BillItem> billItemList = new List<BillItem>();
            // Products without extras, not in a campaign
            foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.OrderProductExtras.Count() == 0 && x.tableSessionCampaignId == null).GroupBy(x => x.productOptionId))
            {
                BillItem billItem_ = new BillItem
                {
                    employeeName = iRestaurant.OrderProducts.Where(x => x.productOptionId == orderProduct.Key && x.Orders.tableSessionId == tableSessionId).FirstOrDefault().Orders.Employees.employeeName,
                    productQuantity = orderProduct.Count(),
                    productName = iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().Products.productName + " - " + iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().productOptionName,
                    extraTotal = 0,
                    productOptionPriceTotal = iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().Products.productPrice + iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().productOptionPrice
                };
                billItemList.Add(billItem_);
            }
            // Products with extras, not in a campaign
            foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.OrderProductExtras.Count() > 0 && x.tableSessionCampaignId == null))
            {
                BillItem billItem_ = new BillItem
                {
                    employeeName = orderProduct.Orders.Employees.employeeName,
                    productQuantity = 1,
                    productName = orderProduct.Products.productName + " - " + orderProduct.ProductOptions.productOptionName
                };
                foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.orderProductId == orderProduct.orderProductId))
                {
                    billItem_.extraTotal += orderProductExtra.ProductExtras.productExtraPrice;
                }
                double optionTotal = orderProduct.ProductOptions.productOptionPrice;
                int productNameLength = billItem_.productName.Length;
                if (productNameLength > 24)
                {
                    billItem_.productName.Remove(24);
                }
                productNameLength = billItem_.productName.Length;
                if (billItem_.extraTotal > 0 || optionTotal > 0)
                {
                    for (int i = 1; i <= (24 - productNameLength); i++)
                    {
                        billItem_.productName += " ";
                    }
                    billItem_.productName += " (Ex.)";
                }
                billItem_.productOptionPriceTotal = orderProduct.Products.productPrice + optionTotal;
                billItemList.Add(billItem_);
            }
            // Campaign products (grouped per campaign)
            foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.tableSessionCampaignId != null).GroupBy(x => x.tableSessionCampaignId))
            {
                BillItem billItem_ = new BillItem
                {
                    employeeName = iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.tableSessionCampaignId == orderProduct.Key).FirstOrDefault().Orders.Employees.employeeName,
                    productQuantity = 1,
                    productName = iRestaurant.TableSessionCampaigns.Where(x => x.tableSessionCampaignId == orderProduct.Key).FirstOrDefault().Campaigns.campaignName,
                    extraTotal = 0
                };
                foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.OrderProducts.Orders.tableSessionId == tableSessionId && x.OrderProducts.tableSessionCampaignId == orderProduct.Key))
                {
                    billItem_.extraTotal += orderProductExtra.ProductExtras.productExtraPrice;
                }
                double optionTotal = iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == orderProduct.Key).Sum(x => x.ProductOptions.productOptionPrice);
                int productNameLength = billItem_.productName.Length;
                if (productNameLength > 24)
                {
                    billItem_.productName = billItem_.productName.Remove(24);
                }
                productNameLength = billItem_.productName.Length;
                if (billItem_.extraTotal > 0 || optionTotal > 0)
                {
                    for (int i = 1; i <= (24 - productNameLength); i++)
                    {
                        billItem_.productName += " ";
                    }
                    billItem_.productName += " (Ex.)";
                }
                billItem_.productOptionPriceTotal = iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == orderProduct.Key).Sum(x => x.ProductOptions.productOptionPrice) + iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == orderProduct.Key).FirstOrDefault().TableSessionCampaigns.Campaigns.campaignPrice;
                billItemList.Add(billItem_);
            }
            List<String> employeeList = new List<String>();
            foreach (var employee in billItemList.GroupBy(x => x.employeeName).ToList())
            {
                employeeList.Add(employee.Key);
            }
            string tableName = iRestaurant.TableSessions.Where(x => x.tableSessionId == tableSessionId).FirstOrDefault().Tables.tableName;

            bool printingStatus = PrintBill(billItemList, employeeList, tableName);
        }

        public void sendBill(string tableSessionId2, string branchId2)
        {
            //tableSessionId = Convert.ToInt32(tableSessionId);
            try
            {
                int tableSessionId = Convert.ToInt32(tableSessionId2);
                int branchId = Convert.ToInt32(branchId2);
                List<BillItem> billItemList = new List<BillItem>();
                // Products without extras, not in a campaign
                foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.isPaid == false && x.OrderProductExtras.Count() == 0 && x.tableSessionCampaignId == null).GroupBy(x => x.productOptionId))
                {
                    BillItem billItem_ = new BillItem
                    {
                        employeeName = iRestaurant.OrderProducts.Where(x => x.productOptionId == orderProduct.Key && x.Orders.tableSessionId == tableSessionId).FirstOrDefault().Orders.Employees.employeeName,
                        productQuantity = orderProduct.Count(),
                        productName = iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().Products.productName + " - " + iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().productOptionName,
                        extraTotal = 0,
                        productOptionPriceTotal = iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().Products.productPrice + iRestaurant.ProductOptions.Where(x => x.productOptionId == orderProduct.Key).FirstOrDefault().productOptionPrice
                    };
                    billItemList.Add(billItem_);
                }
                // Products with extras, not in a campaign
                foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.isPaid == false && x.OrderProductExtras.Count() > 0 && x.tableSessionCampaignId == null))
                {
                    BillItem billItem_ = new BillItem
                    {
                        employeeName = orderProduct.Orders.Employees.employeeName,
                        productQuantity = 1,
                        productName = orderProduct.Products.productName + " - " + orderProduct.ProductOptions.productOptionName
                    };
                    foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.orderProductId == orderProduct.orderProductId))
                    {
                        billItem_.extraTotal += orderProductExtra.ProductExtras.productExtraPrice;
                    }
                    double optionTotal = orderProduct.ProductOptions.productOptionPrice;
                    int productNameLength = billItem_.productName.Length;
                    if (productNameLength > 24)
                    {
                        billItem_.productName.Remove(24);
                    }
                    productNameLength = billItem_.productName.Length;
                    if (billItem_.extraTotal > 0 || optionTotal > 0)
                    {
                        for (int i = 1; i <= (24 - productNameLength); i++)
                        {
                            billItem_.productName += " ";
                        }
                        billItem_.productName += " (Ex.)";
                    }
                    billItem_.productOptionPriceTotal = orderProduct.Products.productPrice + optionTotal;
                    billItemList.Add(billItem_);
                }
                // Campaign products (grouped per campaign)
                foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.isPaid == false && x.tableSessionCampaignId != null).GroupBy(x => x.tableSessionCampaignId))
                {
                    BillItem billItem_ = new BillItem
                    {
                        employeeName = iRestaurant.OrderProducts.Where(x => x.Orders.tableSessionId == tableSessionId && x.tableSessionCampaignId == orderProduct.Key).FirstOrDefault().Orders.Employees.employeeName,
                        productQuantity = 1,
                        productName = iRestaurant.TableSessionCampaigns.Where(x => x.tableSessionCampaignId == orderProduct.Key).FirstOrDefault().Campaigns.campaignName,
                        extraTotal = 0
                    };
                    foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.OrderProducts.Orders.tableSessionId == tableSessionId && x.isPaid == false && x.OrderProducts.tableSessionCampaignId == orderProduct.Key))
                    {
                        billItem_.extraTotal += orderProductExtra.ProductExtras.productExtraPrice;
                    }
                    double optionTotal = iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == orderProduct.Key).Sum(x => x.ProductOptions.productOptionPrice);
                    int productNameLength = billItem_.productName.Length;
                    if (productNameLength > 24)
                    {
                        billItem_.productName = billItem_.productName.Remove(24);
                    }
                    productNameLength = billItem_.productName.Length;
                    if (billItem_.extraTotal > 0 || optionTotal > 0)
                    {
                        for (int i = 1; i <= (24 - productNameLength); i++)
                        {
                            billItem_.productName += " ";
                        }
                        billItem_.productName += " (Ex.)";
                    }
                    billItem_.productOptionPriceTotal = iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == orderProduct.Key).Sum(x => x.ProductOptions.productOptionPrice) + iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == orderProduct.Key).FirstOrDefault().TableSessionCampaigns.Campaigns.campaignPrice;
                    billItemList.Add(billItem_);
                }
                List<String> employeeList = new List<String>();
                foreach (var employee in billItemList.GroupBy(x => x.employeeName).ToList())
                {
                    employeeList.Add(employee.Key);
                }
                string tableName = iRestaurant.TableSessions.Where(x => x.tableSessionId == tableSessionId).FirstOrDefault().Tables.tableName;


                bool printingStatus = PrintBill(billItemList, employeeList, tableName);
                if (printingStatus)
                {
                    iRestaurant.TableSessions.Where(x => x.tableSessionId == tableSessionId).ToList().FirstOrDefault().billRequested = false;
                    iRestaurant.SaveChanges();
                }
            }
            catch
            {

            }
        }

        public void getReport(string startDate2, string endDate2, string branchId2)
        {
            int branchId = Convert.ToInt32(branchId2);
            DateTime startDate = Convert.ToDateTime(startDate2);
            DateTime endDate = Convert.ToDateTime(endDate2);
            List<EmployeePerformance> employeePerformanceList = new List<EmployeePerformance>();
            List<PaymentMethodReport> paymentMethodReportList = new List<PaymentMethodReport>();
            endDate = endDate.AddSeconds(59).AddMilliseconds(999);
            #region Employee performance
            foreach (var employee in iRestaurant.Employees.ToList())
            {
                EmployeePerformance employeePerformance = new EmployeePerformance();
                double employeeTotalAmount = 0;
                double totalAmount = 0;

                // Totals for this employee

                // Non-campaign order products: product prices plus their options and extras
                foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.Orders.employeeId == employee.employeeId && x.tableSessionCampaignId == null && x.paidDate >= startDate && x.paidDate <= endDate).ToList())
                {
                    employeeTotalAmount += orderProduct.ProductOptions.productOptionPrice; // Option prices of this employee's non-campaign orders
                    employeeTotalAmount += orderProduct.Products.productPrice; // Product prices of this employee's non-campaign orders
                    foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.orderProductId == orderProduct.orderProductId).ToList())
                    {
                        employeeTotalAmount += orderProductExtra.ProductExtras.productExtraPrice;  // Extras of this employee's non-campaign orders
                    }

                }

                // Campaign order products: options and extras
                foreach (var productGroup in iRestaurant.OrderProducts.Where(x => x.Orders.employeeId == employee.employeeId && x.tableSessionCampaignId != null && x.paidDate >= startDate && x.paidDate <= endDate).GroupBy(x => x.tableSessionCampaignId).ToList())
                {
                    // Groups this employee's campaign products by campaign and sums the option prices of each group
                    employeeTotalAmount += iRestaurant.OrderProducts.Where(x => x.Orders.employeeId == employee.employeeId && x.tableSessionCampaignId == productGroup.Key && x.paidDate >= startDate && x.paidDate <= endDate).Sum(x => x.ProductOptions.productOptionPrice);

                    foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.OrderProducts.Orders.employeeId == employee.employeeId && x.OrderProducts.tableSessionCampaignId == productGroup.Key).ToList())
                    {
                        employeeTotalAmount += orderProductExtra.ProductExtras.productExtraPrice;  // Extras of this employee's campaign orders
                    }

                }
                foreach (var productGroup in iRestaurant.OrderProducts.Where(x => x.Orders.employeeId == employee.employeeId && x.tableSessionCampaignId != null && x.paidDate >= startDate && x.paidDate <= endDate).GroupBy(x => x.tableSessionCampaignId).ToList())
                {
                    employeeTotalAmount += iRestaurant.TableSessionCampaigns.Where(x => x.tableSessionCampaignId == productGroup.Key).FirstOrDefault().Campaigns.campaignPrice; // Adds the campaign price once per campaign group
                }

                // Totals across all employees

                // Non-campaign order products: product prices plus their options and extras
                foreach (var orderProduct in iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == null && x.paidDate >= startDate && x.paidDate <= endDate).ToList())
                {
                    totalAmount += orderProduct.ProductOptions.productOptionPrice; // Option prices for all employees
                    totalAmount += orderProduct.Products.productPrice; // Product prices for all employees
                    foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.orderProductId == orderProduct.orderProductId).ToList())
                    {
                        totalAmount += orderProductExtra.ProductExtras.productExtraPrice;  // Extras for all employees
                    }

                }

                // Campaign order products: options and extras
                foreach (var productGroup in iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId != null && x.paidDate >= startDate && x.paidDate <= endDate).GroupBy(x => x.tableSessionCampaignId).ToList())
                {
                    // Groups all employees' campaign products by campaign and sums the option prices of each group
                    totalAmount += iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId == productGroup.Key && x.paidDate >= startDate && x.paidDate <= endDate).Sum(x => x.ProductOptions.productOptionPrice);
                    foreach (var orderProductExtra in iRestaurant.OrderProductExtras.Where(x => x.OrderProducts.tableSessionCampaignId == productGroup.Key).ToList())
                    {
                        totalAmount += orderProductExtra.ProductExtras.productExtraPrice;  // Extras for all employees
                    }
                }
                // Campaign prices of campaign order products
                foreach (var productGroup in iRestaurant.OrderProducts.Where(x => x.tableSessionCampaignId != null && x.paidDate >= startDate && x.paidDate <= endDate).GroupBy(x => x.tableSessionCampaignId).ToList())
                {
                    totalAmount += iRestaurant.TableSessionCampaigns.Where(x => x.tableSessionCampaignId == productGroup.Key).FirstOrDefault().Campaigns.campaignPrice; // Adds the campaign price once per campaign group
                }
                employeePerformance.employeeId = employee.employeeId;
                employeePerformance.employeeName = employee.employeeName;
                employeePerformance.employeeAmount = employeeTotalAmount;
                employeePerformance.employeePercent = employeeTotalAmount * 100 / totalAmount;
                employeePerformanceList.Add(employeePerformance);
            }
            #endregion
            #region Payment method report
            double totalPayment = 0;
            foreach (var payment in iRestaurant.Payments.Where(x => x.paymentDate >= startDate && x.paymentDate <= endDate))
            {
                totalPayment += payment.paymentAmount;
            }
            foreach (var payment in iRestaurant.Payments.Where(x => x.paymentDate >= startDate && x.paymentDate <= endDate).GroupBy(x => x.PaymentTypes.paymentTypeName))
            {
                PaymentMethodReport paymentMethod = new PaymentMethodReport();
                paymentMethod.methodName = payment.Key;
                paymentMethod.methodAmount = payment.Sum(x => x.paymentAmount);
                paymentMethod.methodPercent = Convert.ToDouble(paymentMethod.methodAmount * 100 / totalPayment);


                paymentMethodReportList.Add(paymentMethod);
            }
            #endregion
            bool printStatus = PrintReport(employeePerformanceList, paymentMethodReportList, startDate, endDate);
            
        }
    }
}
