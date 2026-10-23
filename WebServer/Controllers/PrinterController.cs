using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iRestaurant.Infrastructure;
using iRestaurant.Models;
using iRestaurant.Security;

namespace iRestaurant.Controllers
{
    /// <summary>Prints the kitchen/department tickets of a new order.</summary>
    [RequireLogin(AuthArea.Cashier, AuthArea.BranchAdmin, AuthArea.Kitchen)]
    public class PrinterController : BaseController
    {
        [HttpPost]
        public JsonResult ActivatePrinters(int orderId)
        {
            int branchId = CurrentBranchId;
            Orders order = db.Orders.FirstOrDefault(x => x.orderId == orderId && x.branchId == branchId);
            if (order == null)
                return JsonCode(NotAllowed);

            var tickets = new List<KitchenTicketItem>();

            // Items without extras are grouped per product option and printed with a quantity.
            foreach (var group in order.OrderProducts.Where(x => !x.OrderProductExtras.Any()).GroupBy(x => x.productOptionId))
            {
                OrderProducts sample = group.First();
                tickets.Add(Ticket(order, sample, group.Count()));
            }

            // Items with extras are printed one by one.
            foreach (OrderProducts item in order.OrderProducts.Where(x => x.OrderProductExtras.Any()))
            {
                KitchenTicketItem ticket = Ticket(order, item, 1);
                ticket.extras.AddRange(item.OrderProductExtras.Select(x => x.ProductExtras.productExtraName));
                tickets.Add(ticket);
            }

            new ReceiptPrinter(order.Branches).PrintKitchenTickets(tickets, db.Departments.Where(x => x.branchId == branchId).ToList());
            return JsonCode(true);
        }

        private static KitchenTicketItem Ticket(Orders order, OrderProducts item, int quantity)
        {
            Departments department = item.Products.ProductCategories.Departments;
            return new KitchenTicketItem
            {
                floorName = order.TableSessions.Tables.Floors.floorName,
                tableName = order.TableSessions.Tables.tableName,
                employeeName = order.Employees.employeeName,
                orderDate = order.orderDate,
                orderNote = order.note,
                productName = item.Products.productName,
                productOptionName = item.ProductOptions.productOptionName,
                quantity = quantity,
                departmentName = department.departmentName,
                departmentIp = department.departmentIp
            };
        }
    }
}
