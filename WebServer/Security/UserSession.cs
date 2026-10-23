using System;
using System.Web;

namespace iRestaurant.Security
{
    /// <summary>The panels a user can be signed into.</summary>
    public enum AuthArea
    {
        None,
        /// <summary>Public site, signed in as a company (shopping cart / checkout).</summary>
        Home,
        CompanyAdmin,
        BranchAdmin,
        Cashier,
        Kitchen
    }

    /// <summary>
    /// Typed wrapper around the ASP.NET session for the signed-in user.
    /// The session keys are kept as before because the Razor views read some of them directly.
    /// </summary>
    public static class UserSession
    {
        private const string AuthCookieName = "iRestaurant.Auth";
        private const string AuthTokenKey = "authToken";
        private const string AreaKey = "LoginArea";

        public static AuthArea GetArea(HttpSessionStateBase session)
        {
            AuthArea area;
            return session?[AreaKey] != null && Enum.TryParse(session[AreaKey].ToString(), out area) ? area : AuthArea.None;
        }

        public static int CompanyId(HttpSessionStateBase session) => ToInt(session["companyId"]);
        public static int BranchId(HttpSessionStateBase session) => ToInt(session["branchId"]);
        public static int EmployeeId(HttpSessionStateBase session) => ToInt(session["loggedInEmployeeId"]);

        public static bool IsCompanyArea(AuthArea area) => area == AuthArea.Home || area == AuthArea.CompanyAdmin;
        public static bool IsEmployeeArea(AuthArea area) => area == AuthArea.BranchAdmin || area == AuthArea.Cashier || area == AuthArea.Kitchen;

        /// <summary>
        /// True when the session belongs to a signed-in user and the session is bound to this browser.
        /// The extra cookie defeats session fixation: knowing a session id is not enough.
        /// </summary>
        public static bool IsAuthenticated(HttpContextBase context)
        {
            var session = context.Session;
            if (session == null || !(session["login"] is bool) || !(bool)session["login"])
                return false;

            string expected = session[AuthTokenKey] as string;
            HttpCookie cookie = context.Request.Cookies[AuthCookieName];
            return expected != null && cookie != null && PasswordHasher.FixedTimeEquals(expected, cookie.Value);
        }

        public static void SignInCompany(HttpContextBase context, Companies company, AuthArea area)
        {
            var session = StartFreshSession(context, area);
            session["isActivated"] = company.isActivated;
            session["companyId"] = company.companyId;
            session["companyName"] = company.companyName;
            session["companyEmail"] = company.companyEmail;
            session["companyPhone"] = company.companyPhone;
        }

        public static void SignInEmployee(HttpContextBase context, Employees employee, AuthArea area, bool webView)
        {
            var session = StartFreshSession(context, area);
            session["employeeName"] = employee.employeeName;
            session["employeeEmail"] = employee.employeeEmail;
            session["isManager"] = employee.isManager;
            session["loggedInEmployeeId"] = employee.employeeId;
            session["branchId"] = employee.branchId;
            session["branchName"] = employee.Branches.branchName;
            session["companyId"] = employee.Branches.companyId;
            session["companyName"] = employee.Branches.Companies.companyName;
            session["companyEmail"] = employee.Branches.Companies.companyEmail;
            session["webView"] = webView;
        }

        public static void SignOut(HttpContextBase context)
        {
            context.Session?.Clear();
            ExpireCookie(context, AuthCookieName);
        }

        private static HttpSessionStateBase StartFreshSession(HttpContextBase context, AuthArea area)
        {
            var session = context.Session;
            object cart = session["cart"];
            session.Clear();
            if (cart != null)
                session["cart"] = cart; // keep the shopping cart across the checkout login

            string token = SecureRandom.HexToken(32);
            session[AuthTokenKey] = token;
            session["login"] = true;
            session[AreaKey] = area.ToString();

            context.Response.Cookies.Add(new HttpCookie(AuthCookieName, token)
            {
                HttpOnly = true,
                Secure = context.Request.IsSecureConnection,
                Path = "/"
            });
            return session;
        }

        internal static void ExpireCookie(HttpContextBase context, string name)
        {
            context.Response.Cookies.Add(new HttpCookie(name, string.Empty)
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = context.Request.IsSecureConnection,
                Path = "/"
            });
        }

        private static int ToInt(object value)
        {
            int result;
            return value != null && int.TryParse(value.ToString(), out result) ? result : 0;
        }
    }
}
