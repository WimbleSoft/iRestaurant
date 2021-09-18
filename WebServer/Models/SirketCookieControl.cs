using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using static iRestaurant.Controllers.HomeController;

namespace iRestaurant.Models
{
    public class SirketCookieControl
    {
        public void CookieKaydet(Sirketler sirketBilgi)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["sirketCookie"] != null)
            {
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["sirketCookie"];
            }
            else
            {
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("sirketCookie");
            }
            Cookie.Expires = DateTime.Now.AddDays(3);
            Cookie["sirketAd"] = sirketBilgi.sirketAd;
            Cookie["sirketEmail"] = sirketBilgi.sirketEmail;
            Cookie["sirketParola"] = sirketBilgi.sirketParola;

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        public HttpCookie CookieGetir()
        {
            if (HttpContext.Current.Request.Cookies["sirketCookie"] != null)
                return HttpContext.Current.Request.Cookies["sirketCookie"];
            else
                return null;
        }

        public void CookieSil() => HttpContext.Current.Response.Cookies["sirketCookie"].Expires = DateTime.Now.AddDays(-1);
    }
}