using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace iRestaurant.Security
{
    /// <summary>Opts an action out of the global CSRF check (server-to-server callbacks, desktop API).</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AllowWithoutAntiForgeryTokenAttribute : Attribute
    {
    }

    /// <summary>
    /// Global filter that rejects state-changing (POST) requests without a valid anti-forgery token.
    /// The token is read from the "RequestVerificationToken" header (set for every jQuery AJAX
    /// call by Views/Shared/_AntiForgery.cshtml) or from the regular hidden form field.
    /// </summary>
    public sealed class ValidateAntiForgeryOnPostAttribute : FilterAttribute, IAuthorizationFilter
    {
        public const string HeaderName = "RequestVerificationToken";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (!string.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                return;
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowWithoutAntiForgeryTokenAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowWithoutAntiForgeryTokenAttribute), true))
                return;

            try
            {
                string headerToken = request.Headers[HeaderName];
                if (!string.IsNullOrEmpty(headerToken))
                {
                    var cookie = request.Cookies[AntiForgeryConfig.CookieName];
                    AntiForgery.Validate(cookie != null ? cookie.Value : null, headerToken);
                }
                else
                {
                    AntiForgery.Validate();
                }
            }
            catch (HttpAntiForgeryException)
            {
                filterContext.Result = new HttpStatusCodeResult(400, "Invalid or missing anti-forgery token.");
            }
        }
    }
}
