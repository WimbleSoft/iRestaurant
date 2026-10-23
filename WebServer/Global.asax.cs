using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace iRestaurant
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcHandler.DisableMvcResponseHeader = true;
            AntiForgeryConfig.RequireSsl = false; // set to true once the site is served over HTTPS only
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true; // sent for every response below instead
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpResponse response = Context?.Response;
            if (response == null)
                return;
            try
            {
                response.Headers.Remove("Server");
                response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                response.Headers["X-Content-Type-Options"] = "nosniff";
                response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            }
            catch (PlatformNotSupportedException)
            {
                // Header manipulation needs the IIS integrated pipeline.
            }
        }
    }
}
