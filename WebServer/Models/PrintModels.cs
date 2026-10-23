using System;
using System.Collections.Generic;

namespace iRestaurant.Models
{
    /// <summary>A line on the bill printed for a table.</summary>
    public class BillItem
    {
        public string productName { get; set; }
        public int productQuantity { get; set; }
        public double productOptionPriceTotal { get; set; }
        public double extraTotal { get; set; }
        public string employeeName { get; set; }
    }

    /// <summary>A line on a partial-payment receipt.</summary>
    public class ReceiptItem
    {
        public string productName { get; set; }
        public int productQuantity { get; set; }
        public double productOptionPriceTotal { get; set; }
        public double extraTotal { get; set; }
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

    /// <summary>One ordered product as printed on a kitchen/department ticket.</summary>
    public class KitchenTicketItem
    {
        public string floorName { get; set; }
        public string tableName { get; set; }
        public string employeeName { get; set; }
        public string productName { get; set; }
        public string productOptionName { get; set; }
        public int quantity { get; set; }
        public List<string> extras { get; set; } = new List<string>();
        public DateTime orderDate { get; set; }
        public string departmentName { get; set; }
        public string departmentIp { get; set; }
        public string orderNote { get; set; }
    }
}
