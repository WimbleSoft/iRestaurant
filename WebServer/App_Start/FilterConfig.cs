using System.Web.Mvc;
using iRestaurant.Security;

namespace iRestaurant
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // Every POST must carry an anti-forgery token (CSRF protection); see Views/Shared/_AntiForgery.cshtml.
            filters.Add(new ValidateAntiForgeryOnPostAttribute());
        }
    }
}
