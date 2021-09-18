using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iRestaurant.Models
{
    public class PersonelCookieControl
    {
        public void CookieKaydet(Personeller personelBilgi, string sirketEmail)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["personelCookie"] != null)
            {
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["personelCookie"];
            }
            else
            {
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("personelCookie");
            }
            Cookie.Expires = DateTime.Now.AddDays(7);
            Cookie["personelAd"] = personelBilgi.personelAd;
            Cookie["personelEmail"] = personelBilgi.personelEmail;
            Cookie["personelParola"] = personelBilgi.personelParola;
            Cookie["sirketEmail"] = sirketEmail;

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        public HttpCookie CookieGetir()
        {
            if (HttpContext.Current.Request.Cookies["personelCookie"] != null)
                return HttpContext.Current.Request.Cookies["personelCookie"];
            else
                return null;
        }

        public void CookieSil() => HttpContext.Current.Response.Cookies["personelCookie"].Expires = DateTime.Now.AddDays(-1);
    }
}