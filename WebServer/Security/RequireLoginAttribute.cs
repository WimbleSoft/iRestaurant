using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iRestaurant.Security
{
    /// <summary>
    /// Authorization filter for the panels. Runs before the action executes (the previous
    /// filters ran after it, so every JSON endpoint was effectively public).
    ///
    /// Page requests without a valid sign-in are redirected to the panel's login page.
    /// JSON endpoints answer <c>-1</c>, which the front-end already treats as "not signed in".
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class RequireLoginAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly AuthArea[] _areas;

        public RequireLoginAttribute(params AuthArea[] areas)
        {
            if (areas == null || areas.Length == 0)
                throw new ArgumentException("At least one area is required.", nameof(areas));
            _areas = areas;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                return;

            HttpContextBase http = filterContext.HttpContext;
            AuthArea primaryArea = _areas[0];

            if (IsSignedIntoAllowedArea(http) || TrySignInFromRememberMe(http, primaryArea))
                return;

            filterContext.Result = IsJsonAction(filterContext)
                ? (ActionResult)new JsonResult { Data = -1, JsonRequestBehavior = JsonRequestBehavior.AllowGet }
                : new RedirectResult(LoginUrl(primaryArea, http.Request));
        }

        private bool IsSignedIntoAllowedArea(HttpContextBase http)
        {
            if (!UserSession.IsAuthenticated(http))
                return false;

            AuthArea current = UserSession.GetArea(http.Session);
            if (!_areas.Contains(current))
                return false;

            using (var db = new iRestaurantDataBaseClass())
            {
                if (UserSession.IsCompanyArea(current))
                {
                    int companyId = UserSession.CompanyId(http.Session);
                    return db.Companies.Any(x => x.companyId == companyId && x.isActivated && !x.isDeleted);
                }

                // Re-check on every request so that revoking permissions or deleting an
                // employee takes effect immediately.
                int employeeId = UserSession.EmployeeId(http.Session);
                int branchId = UserSession.BranchId(http.Session);
                return db.Employees.Any(x => x.employeeId == employeeId && x.branchId == branchId
                                             && x.isManager && !x.isDeleted && !x.Branches.isDeleted);
            }
        }

        private static bool TrySignInFromRememberMe(HttpContextBase http, AuthArea area)
        {
            using (var db = new iRestaurantDataBaseClass())
            {
                if (UserSession.IsCompanyArea(area))
                {
                    int companyId = RememberMeCookie.Read(http, RememberMeCookie.Kind.Company,
                        id => db.Companies.Where(x => x.companyId == id && x.isActivated && !x.isDeleted).Select(x => x.companyPassword).FirstOrDefault());
                    var company = companyId == 0 ? null : db.Companies.Find(companyId);
                    if (company == null)
                        return false;
                    UserSession.SignInCompany(http, company, area);
                    return true;
                }

                int employeeId = RememberMeCookie.Read(http, RememberMeCookie.Kind.Employee,
                    id => db.Employees.Where(x => x.employeeId == id && x.isManager && !x.isDeleted && !x.Branches.isDeleted).Select(x => x.employeePassword).FirstOrDefault());
                var employee = employeeId == 0 ? null : db.Employees.Find(employeeId);
                if (employee == null)
                    return false;
                bool webView = http.Session["webView"] is bool && (bool)http.Session["webView"];
                UserSession.SignInEmployee(http, employee, area, webView);
                return true;
            }
        }

        private static bool IsJsonAction(AuthorizationContext filterContext)
        {
            var reflected = filterContext.ActionDescriptor as ReflectedActionDescriptor;
            if (reflected != null && typeof(JsonResult).IsAssignableFrom(reflected.MethodInfo.ReturnType))
                return true;
            return filterContext.HttpContext.Request.IsAjaxRequest();
        }

        public static string LoginUrl(AuthArea area, HttpRequestBase request)
        {
            string controller = area == AuthArea.None ? "Home" : area.ToString();
            string url = "/" + controller + "/Login";

            // The login views read the action name back from the query string, so only the
            // last path segment of the requested page is passed (validated again after login).
            string[] segments = request.Url.AbsolutePath.Trim('/').Split('/');
            string action = request.HttpMethod == "GET" && segments.Length >= 2 ? segments[1] : null;
            return string.IsNullOrEmpty(action) ? url : url + "?returnUrl=" + HttpUtility.UrlEncode(action);
        }
    }
}
