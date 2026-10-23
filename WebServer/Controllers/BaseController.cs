using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iRestaurant.Models;
using iRestaurant.Security;

namespace iRestaurant.Controllers
{
    /// <summary>
    /// Common plumbing for all controllers: one database context per request, the identity of
    /// the signed-in user, the shared employee login flow and the view-model builders.
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>JSON code the front-end interprets as "not signed in / not allowed".</summary>
        protected const int NotAllowed = -1;
        protected const int Success = 0;
        protected const int Failure = 1;

        protected readonly iRestaurantDataBaseClass db = new iRestaurantDataBaseClass();

        protected int CurrentCompanyId => UserSession.CompanyId(Session);
        protected int CurrentBranchId => UserSession.BranchId(Session);
        protected int CurrentEmployeeId => UserSession.EmployeeId(Session);
        protected string ClientAddress => Request.UserHostAddress;

        protected JsonResult JsonCode(object value) => Json(value, JsonRequestBehavior.AllowGet);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        #region Employee login (branch admin, cashier and kitchen panels)

        protected const string GenericLoginError = "Your e-mail or password is incorrect. Please try again.";
        protected const string ThrottledLoginError = "Too many failed attempts. Please wait a few minutes and try again.";
        protected const string NoPanelAccessError = "You do not have access to this panel. Please contact your branch manager.";

        /// <summary>
        /// Finds an active employee by company e-mail, employee e-mail and password.
        /// Upgrades legacy password hashes on success.
        /// </summary>
        protected Employees FindEmployeeByCredentials(string companyEmail, string employeeEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(companyEmail) || string.IsNullOrWhiteSpace(employeeEmail) || string.IsNullOrEmpty(password))
                return null;

            List<Employees> candidates = db.Employees
                .Where(x => x.employeeEmail == employeeEmail && x.Branches.Companies.companyEmail == companyEmail
                            && !x.isDeleted && !x.Branches.isDeleted)
                .ToList();

            foreach (Employees employee in candidates)
            {
                bool needsRehash;
                if (!PasswordHasher.Verify(password, employee.employeePassword, out needsRehash))
                    continue;
                if (needsRehash)
                {
                    employee.employeePassword = PasswordHasher.Hash(password);
                    db.SaveChanges();
                }
                return employee;
            }
            return null;
        }

        /// <summary>
        /// Shared sign-in for the employee panels. Supports a normal form post, the single-use
        /// token used by the POS desktop app (<paramref name="loginToken"/>) and the encrypted
        /// remember-me cookie. Credentials are never accepted in the URL any more.
        /// </summary>
        protected ActionResult EmployeeLogin(EmployeeAccountViewModel model, bool? webView, string loginToken, AuthArea area, string defaultAction)
        {
            string controller = area.ToString();
            bool isWebView = webView == true;

            if (!string.IsNullOrEmpty(loginToken))
            {
                int tokenEmployeeId = OneTimeLoginTokens.Redeem(loginToken);
                Employees tokenEmployee = tokenEmployeeId == 0 ? null : db.Employees.FirstOrDefault(x => x.employeeId == tokenEmployeeId && !x.isDeleted && !x.Branches.isDeleted);
                if (tokenEmployee != null && tokenEmployee.isManager)
                {
                    UserSession.SignInEmployee(HttpContext, tokenEmployee, area, true);
                    return Redirect(SafeRedirect.Resolve(model?.returnUrl, controller, defaultAction));
                }
                TempData["Message"] = "The sign-in link has expired. Please sign in again.";
                return View();
            }

            if (Request.HttpMethod != "POST")
            {
                if (UserSession.IsAuthenticated(HttpContext) && UserSession.GetArea(Session) == area)
                    return Redirect(SafeRedirect.Resolve(model?.returnUrl, controller, defaultAction));

                int rememberedId = RememberMeCookie.Read(HttpContext, RememberMeCookie.Kind.Employee,
                    id => db.Employees.Where(x => x.employeeId == id && !x.isDeleted).Select(x => x.employeePassword).FirstOrDefault());
                Employees remembered = rememberedId == 0 ? null : db.Employees.Find(rememberedId);
                if (remembered != null && remembered.isManager && !remembered.Branches.isDeleted)
                {
                    UserSession.SignInEmployee(HttpContext, remembered, area, isWebView);
                    return Redirect(SafeRedirect.Resolve(model?.returnUrl, controller, defaultAction));
                }
                ModelState.Clear();
                return View();
            }

            if (model == null || !ModelState.IsValid)
            {
                TempData["Message"] = "Please fill in all fields.";
                return View();
            }

            string throttleKey = model.companyEmail + "/" + model.employeeEmail;
            if (LoginThrottle.IsBlocked(throttleKey, ClientAddress))
            {
                TempData["Message"] = ThrottledLoginError;
                return View();
            }

            Employees employee = FindEmployeeByCredentials(model.companyEmail, model.employeeEmail, model.employeePassword);
            if (employee == null)
            {
                LoginThrottle.RegisterFailure(throttleKey, ClientAddress);
                TempData["Message"] = GenericLoginError;
                return View();
            }
            LoginThrottle.RegisterSuccess(throttleKey);

            if (!employee.isManager)
            {
                TempData["Message"] = NoPanelAccessError;
                return View();
            }

            UserSession.SignInEmployee(HttpContext, employee, area, isWebView);
            RememberMeCookie.Issue(HttpContext, RememberMeCookie.Kind.Employee, employee.employeeId, employee.employeePassword, TimeSpan.FromDays(7));
            return Redirect(SafeRedirect.Resolve(model.returnUrl, controller, defaultAction));
        }

        /// <summary>Signs out of every panel and forgets the remember-me cookies.</summary>
        protected void SignOutEverywhere()
        {
            RememberMeCookie.Clear(HttpContext, RememberMeCookie.Kind.Employee);
            RememberMeCookie.Clear(HttpContext, RememberMeCookie.Kind.Company);
            UserSession.SignOut(HttpContext);
        }

        /// <summary>
        /// The cashier and kitchen screens ask for the manager's password before signing out
        /// so that staff cannot leave the panel unattended.
        /// </summary>
        protected JsonResult ConfirmPasswordAndSignOut(int employeeId, string employeePassword)
        {
            if (!UserSession.IsAuthenticated(HttpContext))
                return JsonCode(false);

            Employees employee = db.Employees.FirstOrDefault(x => x.employeeId == employeeId && x.branchId == CurrentBranchId && x.isManager && !x.isDeleted);
            bool needsRehash;
            if (employee == null || LoginThrottle.IsBlocked("logout/" + employeeId, ClientAddress)
                || !PasswordHasher.Verify(employeePassword, employee.employeePassword, out needsRehash))
            {
                LoginThrottle.RegisterFailure("logout/" + employeeId, ClientAddress);
                return JsonCode(Failure);
            }

            SignOutEverywhere();
            return JsonCode(Success);
        }

        #endregion

        #region View models

        /// <summary>Everything the branch-level screens (branch admin, cashier, kitchen) display.</summary>
        protected ViewModelDemoVM BuildBranchViewModel(int branchId, bool includeDeletedEmployees = false, bool onlyActivePaymentTypes = true)
        {
            return new ViewModelDemoVM
            {
                Floors = db.Floors.Where(x => x.branchId == branchId && !x.isDeleted).ToList(),
                Tables = db.Tables.Where(x => x.Floors.branchId == branchId && !x.Floors.isDeleted).ToList(),
                TableSessions = db.TableSessions.Where(x => x.branchId == branchId).ToList(),
                Orders = db.Orders.Where(x => x.branchId == branchId).ToList(),
                OrderProducts = db.OrderProducts.Where(x => x.Orders.branchId == branchId).ToList(),
                OrderProductExtras = db.OrderProductExtras.Where(x => x.OrderProducts.Orders.branchId == branchId).ToList(),
                Products = db.Products.Where(x => x.branchId == branchId).ToList(),
                ProductCategories = db.ProductCategories.Where(x => x.Departments.branchId == branchId).ToList(),
                ProductExtras = db.ProductExtras.Where(x => x.Products.branchId == branchId).ToList(),
                ProductOptions = db.ProductOptions.Where(x => x.Products.branchId == branchId).ToList(),
                ProductStockEntries = db.ProductStockEntries.Where(x => x.ProductStocks.branchId == branchId).ToList(),
                ProductStocks = db.ProductStocks.Where(x => x.branchId == branchId).ToList(),
                Employees = db.Employees.Where(x => x.branchId == branchId && (includeDeletedEmployees || !x.isDeleted)).ToList(),
                Departments = db.Departments.Where(x => x.branchId == branchId).ToList(),
                Announcements = db.Announcements.Where(x => x.branchId == branchId).ToList(),
                Payments = db.Payments.Where(x => x.branchId == branchId).ToList(),
                PaymentTypes = db.PaymentTypes.Where(x => x.branchId == branchId && (!onlyActivePaymentTypes || !x.isDeleted)).ToList(),
                TableSessionCampaigns = db.TableSessionCampaigns.Where(x => x.Campaigns.branchId == branchId).ToList(),
                Campaigns = db.Campaigns.Where(x => x.branchId == branchId).ToList(),
                CampaignProducts = db.CampaignProducts.Where(x => x.Campaigns.branchId == branchId).ToList(),
                Menus = db.Menus.Where(x => x.branchId == branchId).ToList(),
                Currencies = db.Currencies.ToList()
            };
        }

        /// <summary>Everything the company-level screens display; only rows owned by the company.</summary>
        protected ViewModelDemoVM BuildCompanyViewModel(int companyId, bool includeBilling = false)
        {
            var vm = new ViewModelDemoVM
            {
                Floors = db.Floors.Where(x => x.Branches.companyId == companyId).ToList(),
                Tables = db.Tables.Where(x => x.Floors.Branches.companyId == companyId).ToList(),
                TableSessions = db.TableSessions.Where(x => x.Branches.companyId == companyId).ToList(),
                Orders = db.Orders.Where(x => x.Branches.companyId == companyId).ToList(),
                OrderProducts = db.OrderProducts.Where(x => x.Orders.Branches.companyId == companyId).ToList(),
                OrderProductExtras = db.OrderProductExtras.Where(x => x.OrderProducts.Orders.Branches.companyId == companyId).ToList(),
                Products = db.Products.Where(x => x.Branches.companyId == companyId).ToList(),
                ProductCategories = db.ProductCategories.Where(x => x.Departments.Branches.companyId == companyId).ToList(),
                ProductExtras = db.ProductExtras.Where(x => x.Products.Branches.companyId == companyId).ToList(),
                ProductOptions = db.ProductOptions.Where(x => x.Products.Branches.companyId == companyId).ToList(),
                Employees = db.Employees.Where(x => x.Branches.companyId == companyId).ToList(),
                Departments = db.Departments.Where(x => x.Branches.companyId == companyId).ToList(),
                Announcements = db.Announcements.Where(x => x.Branches.companyId == companyId).ToList(),
                Payments = db.Payments.Where(x => x.Branches.companyId == companyId).ToList(),
                PaymentTypes = db.PaymentTypes.Where(x => x.Branches.companyId == companyId).ToList(),
                TableSessionCampaigns = db.TableSessionCampaigns.Where(x => x.Campaigns.Branches.companyId == companyId).ToList(),
                Campaigns = db.Campaigns.Where(x => x.Branches.companyId == companyId).ToList(),
                CampaignProducts = db.CampaignProducts.Where(x => x.Campaigns.Branches.companyId == companyId).ToList(),
                Menus = db.Menus.Where(x => x.Branches.companyId == companyId).ToList(),
                Currencies = db.Currencies.ToList(),
                Countries = db.Countries.ToList(),
                Provinces = db.Provinces.ToList(),
                Districts = db.Districts.ToList(),
                Companies = db.Companies.Where(x => x.companyId == companyId).ToList(),
                Branches = db.Branches.Where(x => x.companyId == companyId && !x.isDeleted).ToList()
            };

            if (includeBilling)
            {
                vm.Invoices = db.Invoices.Where(x => x.companyId == companyId).ToList();
                vm.InvoiceServices = db.InvoiceServices.Where(x => x.Invoices.companyId == companyId).ToList();
                vm.InvoicePayments = db.InvoicePayments.Where(x => x.Invoices.companyId == companyId).ToList();
                vm.ServiceCampaignServices = db.ServiceCampaignServices.ToList();
                vm.ServiceCampaigns = db.ServiceCampaigns.ToList();
                vm.Services = db.Services.ToList();
                vm.ServiceLicenses = db.ServiceLicenses.Where(x => x.Invoices.companyId == companyId).ToList();
            }
            return vm;
        }

        #endregion
    }
}
