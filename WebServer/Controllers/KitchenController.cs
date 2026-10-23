using System;
using System.Linq;
using System.Web.Mvc;
using iRestaurant.Models;
using iRestaurant.Security;

namespace iRestaurant.Controllers
{
    /// <summary>Kitchen display: today's incoming and completed orders of the branch.</summary>
    [RequireLogin(AuthArea.Kitchen)]
    public class KitchenController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login(EmployeeAccountViewModel model, bool? webView, string loginToken)
        {
            return EmployeeLogin(model, webView, loginToken, AuthArea.Kitchen, "NewIncomingOrders");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckLogout(int employeeId, string employeePassword)
        {
            return ConfirmPasswordAndSignOut(employeeId, employeePassword);
        }

        public ActionResult NewIncomingOrders()
        {
            ViewBag.OpenPage = "NewIncomingOrders";
            ViewBag.Title = "New Orders";
            return View(TodaysOrders(completed: false));
        }

        public ActionResult CompletedOrders()
        {
            ViewBag.OpenPage = "CompletedOrders";
            ViewBag.Title = "Completed Orders";
            return View(TodaysOrders(completed: true));
        }

        /// <summary>Marks every item of the order as prepared and the order as completed.</summary>
        [HttpPost]
        public JsonResult ConfirmAll(int orderId)
        {
            return SetOrderCompleted(orderId, true);
        }

        /// <summary>Moves a completed order back to the incoming list.</summary>
        [HttpPost]
        public JsonResult ConfirmingAll(int orderId)
        {
            return SetOrderCompleted(orderId, false);
        }

        /// <summary>Toggles the "prepared" flag of a single item; returns the new state.</summary>
        [HttpPost]
        public JsonResult ChangeOrderProductsStatus(int orderProductId)
        {
            int branchId = CurrentBranchId;
            OrderProducts item = db.OrderProducts.FirstOrDefault(x => x.orderProductId == orderProductId && x.Orders.branchId == branchId);
            if (item == null)
                return JsonCode(null);

            item.isConfirmed = !item.isConfirmed;
            db.SaveChanges();
            return JsonCode(item.isConfirmed);
        }

        [HttpPost]
        public JsonResult CountOrders()
        {
            int branchId = CurrentBranchId;
            return JsonCode(db.Orders.Count(x => x.branchId == branchId && !x.orderStatus));
        }

        private JsonResult SetOrderCompleted(int orderId, bool completed)
        {
            int branchId = CurrentBranchId;
            Orders order = db.Orders.FirstOrDefault(x => x.orderId == orderId && x.branchId == branchId);
            if (order == null)
                return JsonCode(NotAllowed);

            foreach (OrderProducts item in order.OrderProducts)
                item.isConfirmed = completed;
            order.orderStatus = completed;
            db.SaveChanges();
            return JsonCode("true");
        }

        private System.Collections.Generic.List<Orders> TodaysOrders(bool completed)
        {
            int branchId = CurrentBranchId;
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            return db.Orders.Where(x => x.branchId == branchId && x.orderStatus == completed && x.orderDate >= today && x.orderDate < tomorrow).ToList();
        }
    }
}
