using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using iRestaurant.Models;

namespace iRestaurant.Attributes
{
    public class SubeYonetimRoleControl : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var iRestaurant = new iRestaurantDataBaseClass();
            
            if (HttpContext.Current.Session["login"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "SubeYonetim")
            {//SESSİON YOKSA COOKİE KONTROL ET
                    string returnUrl = filterContext.HttpContext.Request.FilePath.Substring(13).ToString();
                    filterContext.HttpContext.Response.Redirect("~/SubeYonetim/YoneticiGiris?returnUrl=" + returnUrl);
            }
            else
            {
                string personelAd = HttpContext.Current.Session["personelAd"].ToString();
                int personelId = Convert.ToInt32(HttpContext.Current.Session["girenid"]);
                var uye_rol = iRestaurant.Personeller.Where(x => x.yetki == true).FirstOrDefault(x => x.personelId == personelId);
                if (uye_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/SubeYonetim/Anasayfa");
                    //(string)filterContext.RouteData.Values.Values.LastOrDefault();
                }
            }
            base.OnActionExecuting(filterContext);

            
        }

    }
    public class HomeSirketRoleControl : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var iRestaurant = new iRestaurantDataBaseClass();


            if (HttpContext.Current.Session["login"] == null || !(HttpContext.Current.Session["GirilenYer"].ToString() == "Home" || HttpContext.Current.Session["GirilenYer"].ToString() == "SirketYonetim"))
            {//SESSİON YOKSA COOKİE KONTROL ET

                filterContext.HttpContext.Response.Redirect("~/Home/Login?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(6).ToString());
            }
            else
            {
                string sirketEmail = HttpContext.Current.Session["sirketEmail"].ToString();
                int sirketId = Convert.ToInt32(HttpContext.Current.Session["sirketId"]);

                var patron_rol = iRestaurant.Sirketler.Where(x => x.aktiflestirildiMi == true && x.sirketId == sirketId).FirstOrDefault();
                if (patron_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Home/Index");
                    //(string)filterContext.RouteData.Values.Values.LastOrDefault();
                }
            }
            base.OnActionExecuting(filterContext);


        }

    }
    public class SirketYonetimRoleControl : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var iRestaurant = new iRestaurantDataBaseClass();


            if (HttpContext.Current.Session["login"] == null || !(HttpContext.Current.Session["GirilenYer"].ToString() == "SirketYonetim" || HttpContext.Current.Session["GirilenYer"].ToString() == "Home"))
            {//SESSİON YOKSA COOKİE KONTROL ET
                
                filterContext.HttpContext.Response.Redirect("~/SirketYonetim/YoneticiGiris?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(15).ToString());
            }
            else
            {
                string sirketEmail = HttpContext.Current.Session["sirketEmail"].ToString();
                int sirketId = Convert.ToInt32(HttpContext.Current.Session["sirketId"]);

                var patron_rol = iRestaurant.Sirketler.Where(x => x.aktiflestirildiMi == true && x.sirketId == sirketId).FirstOrDefault();
                if (patron_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/SirketYonetim/Anasayfa");
                    //(string)filterContext.RouteData.Values.Values.LastOrDefault();
                }
            }
            base.OnActionExecuting(filterContext);


        }

    }
    public class KasaRoleControl : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var iRestaurant = new iRestaurantDataBaseClass();
            
            if (HttpContext.Current.Session["login"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Kasa")
            {//SESSİON YOKSA COOKİE KONTROL ET
            
                filterContext.HttpContext.Response.Redirect("~/Kasa/YoneticiGiris?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(6).ToString());
            }
            else
            {
                string personelAd = HttpContext.Current.Session["personelAd"].ToString();
                int personelId = Convert.ToInt32(HttpContext.Current.Session["girenid"]);
                var uye_rol = iRestaurant.Personeller.Where(x => x.yetki == true).FirstOrDefault(x => x.personelId == personelId);
                if (uye_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Kasa/Kasa");
                }
            }
            base.OnActionExecuting(filterContext);


        }

    }
    public class MutfakRoleControl : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var iRestaurant = new iRestaurantDataBaseClass();

            if (HttpContext.Current.Session["login"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Mutfak")
            {//SESSİON YOKSA COOKİE KONTROL ET

                filterContext.HttpContext.Response.Redirect("~/Mutfak/YoneticiGiris?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(8).ToString());
            }
            else
            {
                string personelAd = HttpContext.Current.Session["personelAd"].ToString();
                int personelId = Convert.ToInt32(HttpContext.Current.Session["girenid"]);
                var uye_rol = iRestaurant.Personeller.Where(x => x.yetki == true).FirstOrDefault(x => x.personelId == personelId);
                if (uye_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Mutfak/YeniGelenSiparisler");
                }
            }
            base.OnActionExecuting(filterContext);


        }

    }
    public class SirketYonetimAsyncFunctionRoleControl : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Check it's JsonResult that we're dealing with
            JsonResult jsonRes = filterContext.Result as JsonResult;
            if (jsonRes == null)
                return;
            if (HttpContext.Current.Session["login"] == null || !(HttpContext.Current.Session["GirilenYer"].ToString() == "Home" || HttpContext.Current.Session["GirilenYer"].ToString() == "SirketYonetim"))
            {
                SirketCookieControl sirketCookie = new SirketCookieControl();
                if (sirketCookie.CookieGetir() != null)
                {
                    HttpCookie cookie = sirketCookie.CookieGetir();
                    string sirketEmail = cookie["sirketEmail"].ToString();
                    string sirketParola = cookie["sirketParola"].ToString();
                    var sirketYonetimController = new Controllers.SirketYonetimController();
                    SirketHesapViewModel sirketHesapViewModel = new SirketHesapViewModel
                    {
                        sirketEmail = sirketEmail,
                        sirketParola = sirketParola
                    };
                    bool basariliMi = sirketYonetimController.SirketAsyncAttributeCookieSessionLogin(sirketHesapViewModel);
                    if (!basariliMi)
                    {
                        jsonRes.Data = -1;
                    }
                }
                else
                {
                    jsonRes.Data = -1;
                }

            }

        }
    }
    public class SubeYonetimAsyncFunctionRoleControl : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Check it's JsonResult that we're dealing with
            JsonResult jsonRes = filterContext.Result as JsonResult;
            if (jsonRes == null)
                return;
            if (HttpContext.Current.Session["login"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "SubeYonetim")
            {
                PersonelCookieControl personelCookie = new PersonelCookieControl();
                if (personelCookie.CookieGetir() != null)
                {
                    HttpCookie cookie = personelCookie.CookieGetir();
                    string personelEmail = cookie["personelEmail"].ToString();
                    string personelParola = cookie["personelParola"].ToString();
                    string sirketEmail = cookie["sirketEmail"].ToString();
                    var a = new Controllers.SubeYonetimController();
                    PersonelHesapViewModel adminHesapViewModel = new PersonelHesapViewModel
                    {
                        personelEmail = personelEmail,
                        personelParola = personelParola,
                        sirketEmail= sirketEmail
                    };
                    bool basariliMi = a.SubeAsyncAttributeCookieSessionLogin(adminHesapViewModel);
                    if (!basariliMi)
                    {
                        jsonRes.Data = -1;
                    }
                }
                else
                {
                    jsonRes.Data = -1;
                }
                
            }

        }
    }
    public class KasaAsyncFunctionRoleControl : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Check it's JsonResult that we're dealing with
            JsonResult jsonRes = filterContext.Result as JsonResult;
            if (jsonRes == null)
                return;
            if (HttpContext.Current.Session["login"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Kasa")
            {
                PersonelCookieControl personelCookie = new PersonelCookieControl();
                if (personelCookie.CookieGetir() != null)
                {
                    HttpCookie cookie = personelCookie.CookieGetir();
                    string personelEmail = cookie["personelEmail"].ToString();
                    string personelParola = cookie["personelParola"].ToString();
                    string sirketEmail = cookie["sirketEmail"].ToString();

                    var a = new Controllers.KasaController();
                    PersonelHesapViewModel adminHesapViewModel = new PersonelHesapViewModel
                    {
                        personelEmail = personelEmail,
                        personelParola = personelParola,
                        sirketEmail=sirketEmail
                    };
                    bool basariliMi = a.KasaAsyncAttributeCookieSessionLogin(adminHesapViewModel,false);
                    if (!basariliMi)
                    {
                        jsonRes.Data = -1;
                    }
                }
                else
                {
                    jsonRes.Data = -1;
                }

            }

        }
    }
    public class MutfakAsyncFunctionRoleControl : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Check it's JsonResult that we're dealing with
            JsonResult jsonRes = filterContext.Result as JsonResult;
            if (jsonRes == null)
                return;
            if (HttpContext.Current.Session["login"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Mutfak")
            {
                PersonelCookieControl personelCookie = new PersonelCookieControl();
                if (personelCookie.CookieGetir() != null)
                {
                    HttpCookie cookie = personelCookie.CookieGetir();
                    string personelEmail = cookie["personelEmail"].ToString();
                    string personelParola = cookie["personelParola"].ToString();
                    string sirketEmail = cookie["sirketEmail"].ToString();

                    var a = new Controllers.KasaController();
                    PersonelHesapViewModel adminHesapViewModel = new PersonelHesapViewModel
                    {
                        personelEmail = personelEmail,
                        personelParola = personelParola,
                        sirketEmail = sirketEmail
                    };
                    bool basariliMi = a.KasaAsyncAttributeCookieSessionLogin(adminHesapViewModel, false);
                    if (!basariliMi)
                    {
                        jsonRes.Data = -1;
                    }
                }
                else
                {
                    jsonRes.Data = -1;
                }

            }

        }
    }
}