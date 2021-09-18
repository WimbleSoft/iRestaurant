using Rework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iRestaurant.Models;
using System.Web.Script.Serialization;
using System.Globalization;

namespace iRestaurant.Controllers
{
    public class SubeYonetimController : Controller
    {
        iRestaurantDataBaseClass yonetimModel = new iRestaurantDataBaseClass();
        // GET: Yonetim
        #region // Sayfa Çağırımları

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Anasayfa()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };


            var weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int pazartesiCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && x.Siparisler.siparisTarihi.DayOfWeek == DayOfWeek.Monday).Count();
            int saliCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && x.Siparisler.siparisTarihi.DayOfWeek == DayOfWeek.Tuesday).Count();
            int carsambaCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && x.Siparisler.siparisTarihi.DayOfWeek == DayOfWeek.Wednesday).Count();
            int persembeCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && x.Siparisler.siparisTarihi.DayOfWeek == DayOfWeek.Thursday).Count();
            int cumaCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && x.Siparisler.siparisTarihi.DayOfWeek == DayOfWeek.Friday).Count();
            int cumartesiCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && x.Siparisler.siparisTarihi.DayOfWeek == DayOfWeek.Saturday).Count();
            int pazarCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) && x.Siparisler.siparisTarihi.DayOfWeek == DayOfWeek.Sunday).Count();
            int toplamCount = yonetimModel.SiparisUrunleri.ToList().Where(x => weekOfYear == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Siparisler.siparisTarihi, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).Count();

            ViewBag.pazartesiCount = pazartesiCount;
            ViewBag.saliCount = saliCount;
            ViewBag.carsambaCount = carsambaCount;
            ViewBag.persembeCount = persembeCount;
            ViewBag.cumaCount = cumaCount;
            ViewBag.cumartesiCount = cumartesiCount;
            ViewBag.pazarCount = pazarCount;
            ViewBag.toplamCount = toplamCount;



            ViewBag.siparisData = 
            (
                from sue in vm.SiparisUrunleriEkstralari
                join su in vm.SiparisUrunleri on sue.siparisUrunlerId equals su.siparisUrunlerId
                join s in vm.Siparisler on su.siparislerId equals s.siparislerId
                group sue.SiparisUrunleri.Siparisler by su.Urunler.urunAd into g
                orderby g.Count() descending
                select new { urunlerAd = g.Key, adet=g.Count() } 
            );
            return View(vm);
        }
        
        public ActionResult Cikis(PersonelHesapViewModel LoginModel)
        {
            
            PersonelCookieControl personelCookie = new PersonelCookieControl();
            personelCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("YoneticiGiris", "SubeYonetim");
        }
        
        public ActionResult YoneticiGiris(PersonelHesapViewModel LoginModel, bool? webView)
        {
            PersonelCookieControl personelCookie = new PersonelCookieControl();
            if (personelCookie.CookieGetir() != null)
            {
                HttpCookie cookie = personelCookie.CookieGetir();
                string personelEmail = cookie["personelEmail"].ToString();
                string personelParola = cookie["personelParola"].ToString();
                string sirketEmail = cookie["sirketEmail"].ToString();
                var personel = yonetimModel.Personeller.Where(x => x.Subeler.Sirketler.sirketEmail == sirketEmail && x.personelEmail == personelEmail && x.personelParola == personelParola).FirstOrDefault();
                if (personel != null)
                {
                    if (personel.yetki == true)
                    {
                        personelCookie.CookieSil();
                        Personeller personelBilgisi = new Personeller
                        {
                            personelAd = personel.personelAd,
                            personelEmail = personel.personelEmail,
                            personelParola = personel.personelParola
                        };
                        personelCookie.CookieKaydet(personelBilgisi, personel.Subeler.Sirketler.sirketEmail);
                        Session["personelAd"] = personel.personelAd;
                        Session["yetki"] = personel.yetki;
                        Session["girenid"] = personel.personelId;
                        Session["subeId"] = personel.subeId;
                        Session["sirketId"] = personel.Subeler.sirketId;
                        Session["sirketAd"] = personel.Subeler.Sirketler.sirketAd;
                        Session["sirketEmail"] = personel.Subeler.Sirketler.sirketEmail;
                        Session["subeAd"] = personel.Subeler.subeAd;
                        Session["personelEmail"] = personel.personelEmail;
                        if (webView.HasValue)
                        {
                            if (webView.Value == true)
                            {
                                Session["webView"] = true;
                            }
                            else
                            {
                                Session["webView"] = false;
                            }
                        }
                        else
                        {
                            Session["webView"] = false;
                        }

                        Session["login"] = true;
                        Session["GirilenYer"] = "SubeYonetim";
                        if (LoginModel.returnUrl == null)
                        {
                            return RedirectToAction("Anasayfa", "SubeYonetim");
                        }
                        else
                        {
                            return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "SubeYonetim");
                        }
                    }
                    else
                    {
                        TempData["Mesaj"] = "Bu panele erişim izniniz bulunmamaktadır. Lütfen şirket üst yetkilisi ile iletişime geçiniz.";
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Email / şifre bilgileriniz hatalıdır. Lütfen tekrar deneyiniz.";
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string personelParola = "";
                    if (webView.HasValue)
                    {
                        if (webView.Value == true)
                        {
                            personelParola = LoginModel.personelParola;
                        }
                        else
                        {
                            personelParola = LoginModel.personelParola.ToSHA(Crypto.SHA_Type.SHA256);
                        }
                    }
                    else
                    {
                        personelParola = LoginModel.personelParola.ToSHA(Crypto.SHA_Type.SHA256);
                    }
                    var personel = yonetimModel.Personeller.Where(x => x.Subeler.Sirketler.sirketEmail == LoginModel.sirketEmail && x.personelEmail == LoginModel.personelEmail && x.personelParola == personelParola).FirstOrDefault();
                    if (personel != null)
                    {
                        if (personel.yetki == true)
                        {
                            personelCookie.CookieSil();
                            Personeller personelBilgisi = new Personeller
                            {
                                personelAd = personel.personelAd,
                                personelEmail = personel.personelEmail,
                                personelParola = personel.personelParola
                            };
                            personelCookie.CookieKaydet(personelBilgisi, personel.Subeler.Sirketler.sirketEmail);
                            Session["personelAd"] = personel.personelAd;
                            Session["yetki"] = personel.yetki;
                            Session["girenid"] = personel.personelId;
                            Session["subeId"] = personel.subeId;
                            Session["sirketId"] = personel.Subeler.sirketId;
                            Session["sirketAd"] = personel.Subeler.Sirketler.sirketAd;
                            Session["sirketEmail"] = personel.Subeler.Sirketler.sirketEmail;
                            Session["subeAd"] = personel.Subeler.subeAd;
                            Session["personelEmail"] = personel.personelEmail;
                            if (webView.HasValue)
                            {
                                if (webView.Value == true)
                                {
                                    Session["webView"] = true;
                                }
                            }

                            Session["login"] = true;
                            Session["GirilenYer"] = "SubeYonetim";
                            if (LoginModel.returnUrl == null)
                            {
                                return RedirectToAction("Anasayfa", "SubeYonetim");
                            }
                            else
                            {
                                return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "SubeYonetim");
                            }
                        }
                        else
                        {
                            TempData["Mesaj"] = "Bu panele erişim izniniz bulunmamaktadır. Lütfen şirket üst yetkilisi ile iletişime geçiniz.";
                        }
                    }
                    else
                    {
                        TempData["Mesaj"] = "Email / şifre bilgileriniz hatalıdır. Lütfen tekrar deneyiniz.";
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen tüm bilgileri eksiksiz doldurunuz.";
                }
            }
            
            return View();
        }

        public bool SubeAsyncAttributeCookieSessionLogin(PersonelHesapViewModel LoginModel)
        {
            if (ModelState.IsValid)
            {
                var personel = yonetimModel.Personeller.Where(x => x.Subeler.Sirketler.sirketEmail == LoginModel.sirketEmail && x.personelEmail == LoginModel.personelEmail && x.personelParola == LoginModel.personelParola && x.yetki == true).FirstOrDefault();
                if (personel != null)
                {
                    Session["personelAd"] = personel.personelAd;
                    Session["yetki"] = personel.yetki;
                    Session["girenid"] = personel.personelId;
                    Session["subeId"] = personel.subeId;
                    Session["sirketAd"] = personel.Subeler.Sirketler.sirketAd;
                    Session["subeAd"] = personel.Subeler.subeAd;
                    Session["personelEmail"] = personel.personelEmail;
                    Session["sirketEmail"] = personel.Subeler.Sirketler.sirketEmail;
                    Session["login"] = true;
                    Session["GirilenYer"] = "SubeYonetim";

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult OdemeTurleri()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };

            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Kampanyalar()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };

            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Katlar()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Menuler()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Duyurular()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Departmanlar()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult UrunKategorileri()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Masalar()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult Personeller()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [Attributes.SubeYonetimRoleControl]
        public ActionResult UrunlerleOzellikleri()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.subeId == subeId && x.silindi==false).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.subeId == subeId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunStokGirdiler = yonetimModel.UrunStokGirdiler.Where(x=>x.UrunStoklar.subeId ==subeId).ToList(),
                UrunStoklar = yonetimModel.UrunStoklar.Where(x=>x.subeId==subeId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        #endregion

        #region // Asenkron Sorgular
        
        #region // YoneticiAyarları
        
        [HttpPost]
        public JsonResult AyarlarGuncelle(Personeller personelBilgisi, int subeId)
        {
            if (Session["login"] == null)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    MD5 pwds = MD5.Create();
                    foreach (var yonetici in yonetimModel.Personeller.Where(x => x.personelId == Convert.ToInt32(Session["girenId"])))
                    {
                        yonetici.personelAd = personelBilgisi.personelAd;
                        yonetici.personelTelefon = personelBilgisi.personelTelefon;
                        if (Convert.ToBoolean(Session["yetki"]) == true)
                        {
                            yonetici.personelEmail = personelBilgisi.personelEmail;
                        }
                        if(personelBilgisi.personelParola != null)
                        {
                            if(personelBilgisi.personelParola.Count()>=8 && personelBilgisi.personelParola.Count() <= 20)
                            {
                                byte[] byteDegeri = System.Text.Encoding.UTF8.GetBytes(personelBilgisi.personelParola);
                                byte[] sifreliByte = pwds.ComputeHash(byteDegeri);
                                yonetici.personelParola = Convert.ToBase64String(sifreliByte);
                            }
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }

                        yonetimModel.SaveChanges();
                    }
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult JsonGirisKontrol(PersonelHesapViewModel LoginModel)
        {
            try
            {
                List<Personeller> personelList = yonetimModel.Personeller.Where(x => x.Subeler.Sirketler.sirketEmail == LoginModel.sirketEmail).ToList();
                if (personelList.Count > 0)
                {
                    List<Personeller> personellist = personelList.Where(x=> x.personelEmail == LoginModel.personelEmail && x.personelParola == LoginModel.personelParola).ToList();
                    if (personellist.Count > 0)
                    {
                        var personel = personellist.FirstOrDefault();
                        if (personel.yetki == true)
                        {
                            return Json("true_" + personel.subeId, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Bu panele erişim izniniz bulunmamaktadır. Lütfen şirketinizi yetkilisi ile iletişime geçiniz.", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Email / şifre bilgileriniz hatalıdır. Lütfen tekrar deneyiniz.", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Şirket email adresini yanlış girdiniz veya bu şirket personeli değilsiniz.", JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Json("Bir sunucu hatası oluştu. Lütfen daha sonra tekrar deneyin.", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region // ÜrünlerEkstralarÖzelliklerDepartmanlar

        #region // Ürünler

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunEkle(Urunler urunBilgisi, HttpPostedFileBase urunResimFile)
        {
            
            if (Convert.ToInt32(Session["subeId"]) == urunBilgisi.subeId)
            {
                Urunler _urun = new Urunler
                {
                    urunAd = urunBilgisi.urunAd,
                    urunAciklama = urunBilgisi.urunAciklama,
                    urunFiyat = urunBilgisi.urunFiyat,
                    urunStokAlarmAdet = urunBilgisi.urunStokAlarmAdet,
                    urunResim = urunBilgisi.urunResim,
                    urunKategoriId = urunBilgisi.urunKategoriId,
                    urunYapimSuresi = urunBilgisi.urunYapimSuresi,
                    yayinda = urunBilgisi.yayinda,
                    subeId = Convert.ToInt32(Session["subeId"])
                };
                if (urunResimFile != null && urunResimFile.ContentLength > 0)
                {
                    try
                    {
                        string yol = Path.Combine(Server.MapPath("~/assets/urunresimleri"), Path.GetFileName(urunResimFile.FileName));
                        urunResimFile.SaveAs(yol);
                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    _urun.urunResim = urunResimFile.FileName;
                }
                else
                {
                    _urun.urunResim = "resimyok.jpg";
                }
                Urunler yeniUrun = yonetimModel.Urunler.Add(_urun);
                yonetimModel.SaveChanges();
                yonetimModel.UrunOzellikler.Add(new UrunOzellikler { urunOzellikAd = "Normal", urunOzellikFiyat = 0, urunId = yeniUrun.urunId });
                yonetimModel.UrunStoklar.Add(new UrunStoklar { urunId = yeniUrun.urunId, adet = 0, subeId = yeniUrun.subeId });
                yonetimModel.SaveChanges();
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunGuncelle(Urunler urunBilgisi, HttpPostedFileBase urunResimFile)
        {
            if (Convert.ToInt32(Session["subeId"]) == urunBilgisi.subeId)
            {
                if(yonetimModel.Urunler.Where(x => x.urunId == urunBilgisi.urunId).Count() == 1)
                {
                    Urunler a = yonetimModel.Urunler.Where(x => x.urunId == urunBilgisi.urunId).First();
                    a.urunAd = urunBilgisi.urunAd;
                    a.urunKategoriId = urunBilgisi.urunKategoriId;
                    a.urunFiyat = urunBilgisi.urunFiyat;
                    a.urunAciklama = urunBilgisi.urunAciklama;
                    a.yayinda = urunBilgisi.yayinda;
                    a.urunYapimSuresi = urunBilgisi.urunYapimSuresi;
                    a.urunStokAlarmAdet = urunBilgisi.urunStokAlarmAdet;
                    if (urunResimFile != null && urunResimFile.ContentLength > 0)
                    {
                        try
                        {
                            string yol = Path.Combine(Server.MapPath("~/assets/urunresimleri"), Path.GetFileName(urunResimFile.FileName));
                            urunResimFile.SaveAs(yol);
                            ViewBag.Message = "File uploaded successfully";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                            return Json(1, JsonRequestBehavior.AllowGet);
                        }
                        a.urunResim = urunResimFile.FileName;
                    }
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
               

                
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunSil(int urunId,int subeId)
        {
            if(Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //Ürün Sipariş Ürünleri Ekstralarını Temizle
                    yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.urunId == urunId));
                    //Ürün Sipariş Ürünlerini Temizle
                    yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.urunId == urunId));
                    //Ürün Ekstralarını Temizle
                    yonetimModel.UrunEkstralar.RemoveRange(yonetimModel.UrunEkstralar.Where(d => d.urunId == urunId));
                    //Ürün Özelliklerini Temizle
                    yonetimModel.UrunOzellikler.RemoveRange(yonetimModel.UrunOzellikler.Where(d => d.urunId == urunId));
                    //Ürün Stok Girdilerini Temizle
                    yonetimModel.UrunStokGirdiler.RemoveRange(yonetimModel.UrunStokGirdiler.Where(d => d.UrunStoklar.urunId == urunId));
                    //Ürün Stoklarını Temizle
                    yonetimModel.UrunStoklar.RemoveRange(yonetimModel.UrunStoklar.Where(d => d.urunId == urunId));
                    //En son ürünleri Temizle
                    yonetimModel.Urunler.RemoveRange(yonetimModel.Urunler.Where(d => d.urunId == urunId));

                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region // ÜrünStokları
        public class UrunStokGirdiJsonDonus
        {
            public double urunStokGirdiAdet { get; set; }
            public string urunStokGirdiTarih { get; set; }
            public int urunStokGirdiId { get; set; }
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunStokGirdiEkle(UrunStokGirdiler urunStokGirdi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    UrunStokGirdiler _urunStokGirdi = new UrunStokGirdiler
                    {
                        urunStokId = urunStokGirdi.urunStokId,
                        urunStokGirdiTarih = DateTime.Now,
                        urunStokGirdiAdet = urunStokGirdi.urunStokGirdiAdet
                    };
                    

                    _urunStokGirdi.urunStokGirdiId = yonetimModel.UrunStokGirdiler.Add(_urunStokGirdi).urunStokGirdiId;
                    UrunStoklar urunStok = yonetimModel.UrunStoklar.Where(x => x.urunStokId == urunStokGirdi.urunStokId).FirstOrDefault();
                    urunStok.adet += urunStokGirdi.urunStokGirdiAdet;
                    
                    yonetimModel.SaveChanges();
                    
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    UrunStokGirdiJsonDonus jsonDonus = new UrunStokGirdiJsonDonus
                    {
                        urunStokGirdiAdet =_urunStokGirdi.urunStokGirdiAdet,
                        urunStokGirdiId = _urunStokGirdi.urunStokGirdiId,
                        urunStokGirdiTarih = _urunStokGirdi.urunStokGirdiTarih.ToString()
                    };

                    string nesne = js.Serialize(jsonDonus)/*.Replace(@"\","").Replace(@"'/","").Replace(@"/", "").Replace("Date(","").Replace(")","").Replace('"','_').Replace("_","")*/;
                    
                    return Json(nesne, JsonRequestBehavior.AllowGet);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunStokGirdiGuncelle(UrunStokGirdiler urunStokGirdi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    
                    if(yonetimModel.UrunStokGirdiler.Where(x => x.urunStokGirdiId == urunStokGirdi.urunStokGirdiId).Count()==1)
                    {
                        UrunStokGirdiler _urunStokGirdi = yonetimModel.UrunStokGirdiler.Where(x => x.urunStokGirdiId == urunStokGirdi.urunStokGirdiId).FirstOrDefault();
                        _urunStokGirdi.urunStokGirdiAdet = urunStokGirdi.urunStokGirdiAdet;

                        yonetimModel.SaveChanges();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunStokGirdiSil(int urunStokGirdiId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    
                    if (yonetimModel.UrunStokGirdiler.Where(x => x.urunStokGirdiId == urunStokGirdiId).Count()>0)
                    {
                        UrunStokGirdiler _urunStokGirdi = yonetimModel.UrunStokGirdiler.Where(x => x.urunStokGirdiId == urunStokGirdiId).First();
                        _urunStokGirdi.UrunStoklar.adet -= _urunStokGirdi.urunStokGirdiAdet;
                        yonetimModel.UrunStokGirdiler.Remove(_urunStokGirdi);
                        yonetimModel.SaveChanges();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }

                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region // ÜrünÖzellikleri

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OzellikEkle(UrunOzellikler urunOzellikBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    UrunOzellikler _urunOzellik = new UrunOzellikler
                    {
                        urunId = urunOzellikBilgisi.urunId,
                        urunOzellikAd = urunOzellikBilgisi.urunOzellikAd,
                        urunOzellikFiyat = urunOzellikBilgisi.urunOzellikFiyat
                    };
                    yonetimModel.UrunOzellikler.Add(_urunOzellik);
                    yonetimModel.SaveChanges();

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OzellikSil(int urunOzellikId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    if (yonetimModel.UrunOzellikler.Where(y => y.urunId == yonetimModel.UrunOzellikler.Where(x => x.urunOzellikId == urunOzellikId).FirstOrDefault().urunId).Count() == 1)
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Sipariş Ürünleri Ekstralarını Sil.
                        yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.urunOzellikId == urunOzellikId));

                        //Sipariş Ürünlerini Sil.
                        yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.urunOzellikId == urunOzellikId));

                        //Ürün Özelliğini Sil.
                        yonetimModel.UrunOzellikler.RemoveRange(yonetimModel.UrunOzellikler.Where(d => d.urunOzellikId == urunOzellikId));

                        //Veritabanını Kaydet
                        yonetimModel.SaveChanges();
                    }
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        #endregion

        #region // ÜrünEkstraları

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult EkstraEkle(UrunEkstralar urunEkstraBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    UrunEkstralar _urunEkstra = new UrunEkstralar
                    {
                        urunId = urunEkstraBilgisi.urunId,
                        urunEkstraAd = urunEkstraBilgisi.urunEkstraAd,
                        urunEkstraFiyat = urunEkstraBilgisi.urunEkstraFiyat
                    };
                    yonetimModel.UrunEkstralar.Add(_urunEkstra);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult EkstraSil(int urunEkstraId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //Sipariş Ürünleri Ekstralarını Sil.
                    yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.urunEkstraId == urunEkstraId));
                    //Ürün ekstra Sil.
                    yonetimModel.UrunEkstralar.RemoveRange(yonetimModel.UrunEkstralar.Where(d => d.urunEkstraId == urunEkstraId));
                        
                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region // Departmanlar

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult DepartmanEkle(Departmanlar departmanBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == departmanBilgisi.subeId)
            {
                try
                {
                    Departmanlar _departman = new Departmanlar
                    {
                        departmanAd = departmanBilgisi.departmanAd,
                        departmanIp = departmanBilgisi.departmanIp,
                        aktifMi = true,
                        subeId = Convert.ToInt32(Session["subeId"])
                    };
                    yonetimModel.Departmanlar.Add(_departman);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult DepartmanGuncelle(Departmanlar departmanBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == departmanBilgisi.subeId)
            {
                try
                {
                    var a = yonetimModel.Departmanlar.Where(x => x.departmanId == departmanBilgisi.departmanId).First();
                    a.departmanAd = departmanBilgisi.departmanAd;
                    a.departmanIp = departmanBilgisi.departmanIp;
                    a.aktifMi = departmanBilgisi.aktifMi;
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult DepartmanSil(int departmanId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //Ürün Sipariş Ürünleri Ekstralarını Temizle
                    yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.Urunler.UrunKategoriler.departmanId == departmanId));
                    //Ürün Sipariş Ürünlerini Temizle
                    yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.Urunler.UrunKategoriler.departmanId == departmanId));
                    //Ürün Ekstralarını Temizle
                    yonetimModel.UrunEkstralar.RemoveRange(yonetimModel.UrunEkstralar.Where(d => d.Urunler.UrunKategoriler.departmanId == departmanId));
                    //Ürün Özelliklerini Temizle
                    yonetimModel.UrunOzellikler.RemoveRange(yonetimModel.UrunOzellikler.Where(d => d.Urunler.UrunKategoriler.departmanId == departmanId));
                    //Ürünleri Temizle
                    yonetimModel.Urunler.RemoveRange(yonetimModel.Urunler.Where(d => d.UrunKategoriler.departmanId == departmanId));
                    //Ürün Kategorileri Temizle
                    yonetimModel.UrunKategoriler.RemoveRange(yonetimModel.UrunKategoriler.Where(d => d.departmanId == departmanId));
                    //En son Departmanları Temizle
                    yonetimModel.Departmanlar.RemoveRange(yonetimModel.Departmanlar.Where(d => d.departmanId == departmanId));
                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        #endregion

        #endregion

        #region // Katlar

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KatEkle(Katlar katBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == katBilgisi.subeId)
            {
                try
                {
                    Katlar _kat = new Katlar
                    {
                        katAd = katBilgisi.katAd,
                        subeId = Convert.ToInt32(Session["subeId"]),
                        silindi=false
                    };
                    yonetimModel.Katlar.Add(_kat);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KatGuncelle(Katlar katBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == katBilgisi.subeId)
            {
                try
                {
                    var kat = yonetimModel.Katlar.Where(x => x.katId == katBilgisi.katId).FirstOrDefault();
                    kat.katAd = katBilgisi.katAd;
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KatSil(int katId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    /*
                    //Önce Sipariş Ürün Ekstralarını Temizle
                    yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.Siparisler.Oturumlar.Masalar.Katlar.katId == katId));
                    //Sonra Sipariş Ürünlerini Temizle
                    yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.Siparisler.Oturumlar.Masalar.Katlar.katId == katId));
                    //Daha Sonra Siparişleri Temizle
                    yonetimModel.Siparisler.RemoveRange(yonetimModel.Siparisler.Where(d => d.Oturumlar.Masalar.Katlar.katId == katId));
                    //Daha daha sonra Oturumları Temizle
                    yonetimModel.Oturumlar.RemoveRange(yonetimModel.Oturumlar.Where(d => d.Masalar.Katlar.katId == katId));
                    //Daha da daha sonra Masaları Temizle
                    yonetimModel.Masalar.RemoveRange(yonetimModel.Masalar.Where(d => d.Katlar.katId == katId));
                    //En son Katı Temizle
                    yonetimModel.Katlar.RemoveRange(yonetimModel.Katlar.Where(d => d.katId == katId));
                    */
                    

                    yonetimModel.Katlar.Find(katId).silindi = true;
                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        #endregion

        #region // Menüler

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult MenuEkle(Menuler menuBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == menuBilgisi.subeId)
            {
                try
                {
                    Menuler _menu = new Menuler
                    {
                        menuAd = menuBilgisi.menuAd,
                        paraBirimiId = menuBilgisi.paraBirimiId,
                        subeId = Convert.ToInt32(Session["subeId"])
                    };
                    yonetimModel.Menuler.Add(_menu);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult MenuGuncelle(Menuler menuBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == menuBilgisi.subeId)
            {
                try
                {
                    var a = yonetimModel.Menuler.Where(x => x.menuId == menuBilgisi.menuId).FirstOrDefault();
                    a.menuAd = menuBilgisi.menuAd;
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult MenuSil(int menuId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //Önce Sipariş Ürün Ekstralarını Temizle
                    yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.Urunler.UrunKategoriler.menuId == menuId));
                    //Sonra Sipariş Ürünlerini Temizle
                    yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.Urunler.UrunKategoriler.menuId == menuId));
                    //Kampanya ürünlerini temizle
                    yonetimModel.KampanyaUrunleri.RemoveRange(yonetimModel.KampanyaUrunleri.Where(d => d.Urunler.UrunKategoriler.menuId == menuId));
                    //Kampanyaları temizle
                    yonetimModel.Kampanyalar.RemoveRange(yonetimModel.Kampanyalar.Where(d => d.menuId == menuId));
                    //Ürün ekstralarını temizle
                    yonetimModel.UrunEkstralar.RemoveRange(yonetimModel.UrunEkstralar.Where(d => d.Urunler.UrunKategoriler.menuId == menuId));
                    //Ürün özelliklerini temizle
                    yonetimModel.UrunOzellikler.RemoveRange(yonetimModel.UrunOzellikler.Where(d => d.Urunler.UrunKategoriler.menuId == menuId));
                    //Ürün Stok girdilerini temizle
                    yonetimModel.UrunStokGirdiler.RemoveRange(yonetimModel.UrunStokGirdiler.Where(x => x.UrunStoklar.Urunler.UrunKategoriler.menuId == menuId));
                    //Ürün Stok girdilerini temizle
                    yonetimModel.UrunStoklar.RemoveRange(yonetimModel.UrunStoklar.Where(x => x.Urunler.UrunKategoriler.menuId == menuId));
                    //Ürünleri temizle
                    yonetimModel.Urunler.RemoveRange(yonetimModel.Urunler.Where(d => d.UrunKategoriler.menuId == menuId));
                    //Ürün kategorilerini temizle
                    yonetimModel.UrunKategoriler.RemoveRange(yonetimModel.UrunKategoriler.Where(d => d.menuId == menuId));
                    //En son Menüyü temizle
                    yonetimModel.Menuler.RemoveRange(yonetimModel.Menuler.Where(d => d.menuId == menuId));
                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region // Masalar

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult MasaEkle(Masalar masaBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    Masalar _masa = new Masalar
                    {
                        masaAd = masaBilgisi.masaAd,
                        katId = masaBilgisi.katId
                    };
                    yonetimModel.Masalar.Add(_masa);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult MasaGuncelle(Masalar masaBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    var a = yonetimModel.Masalar.Where(x => x.masaId == masaBilgisi.masaId).First();
                                a.masaAd = masaBilgisi.masaAd;
                                a.katId = masaBilgisi.katId;
                                a.durum = false;
                                yonetimModel.SaveChanges();
                                return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult MasaSil(int masaId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //Önce Sipariş Ürün Ekstralarını Temizle
                    yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.Siparisler.Oturumlar.Masalar.masaId == masaId));
                    //Sonra Sipariş Ürünlerini Temizle
                    yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.Siparisler.Oturumlar.Masalar.masaId == masaId));
                    //Daha Sonra Siparişleri Temizle
                    yonetimModel.Siparisler.RemoveRange(yonetimModel.Siparisler.Where(d => d.Oturumlar.Masalar.masaId == masaId));
                    //Daha daha sonra Oturumları Temizle
                    yonetimModel.Oturumlar.RemoveRange(yonetimModel.Oturumlar.Where(d => d.Masalar.masaId == masaId));
                    //Daha da daha sonra Masaları Temizle
                    yonetimModel.Masalar.RemoveRange(yonetimModel.Masalar.Where(d => d.masaId == masaId));

                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        #endregion

        #region // Ürün Kategoriler

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunKategoriEkle(UrunKategoriler urunKategoriBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    UrunKategoriler _urunKategori = new UrunKategoriler
                    {
                        urunKategoriAd = urunKategoriBilgisi.urunKategoriAd,
                        departmanId = urunKategoriBilgisi.departmanId,
                        vergiYuzde = urunKategoriBilgisi.vergiYuzde
                    };
                    yonetimModel.UrunKategoriler.Add(_urunKategori);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunKategoriGuncelle(UrunKategoriler urunKategoriBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    var a = yonetimModel.UrunKategoriler.Where(x => x.urunKategoriId == urunKategoriBilgisi.urunKategoriId).First();
                    a.urunKategoriAd = urunKategoriBilgisi.urunKategoriAd;
                    a.departmanId = urunKategoriBilgisi.departmanId;
                    a.vergiYuzde = urunKategoriBilgisi.vergiYuzde;
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult UrunKategoriSil(int urunKategoriId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //Sipariş Ürün Ekstralarını Temizle
                    yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.Urunler.urunKategoriId==urunKategoriId));
                    //Sipariş Ürünlerini Temizle
                    yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.Urunler.urunKategoriId==urunKategoriId));
                    //Ürün Özelliklerini Temizle
                    yonetimModel.UrunOzellikler.RemoveRange(yonetimModel.UrunOzellikler.Where(d => d.Urunler.urunKategoriId == urunKategoriId));
                    //Ürün Ekstralarını Temizle
                    yonetimModel.UrunEkstralar.RemoveRange(yonetimModel.UrunEkstralar.Where(d => d.Urunler.urunKategoriId == urunKategoriId));
                    //Ürünleri Temizle
                    yonetimModel.Urunler.RemoveRange(yonetimModel.Urunler.Where(d => d.urunKategoriId == urunKategoriId));
                    //Ürün Kategorilerini Temizle
                    yonetimModel.UrunKategoriler.RemoveRange(yonetimModel.UrunKategoriler.Where(d => d.urunKategoriId == urunKategoriId));
                    
                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        #endregion

        #region // Personeller

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult PersonelEkle(Personeller personelBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == personelBilgisi.subeId)
            {
                try
                {
                    if(personelBilgisi.personelAd!=null && personelBilgisi.personelEmail!=null && personelBilgisi.personelParola!=null && personelBilgisi.personelTelefon != null && personelBilgisi.subeId > 0)
                    {
                        Personeller _personel = new Personeller
                        {
                            personelAd = personelBilgisi.personelAd,
                            personelEmail = personelBilgisi.personelEmail,
                            personelTelefon = personelBilgisi.personelTelefon,
                            yetki = false,
                            masaModuMu = personelBilgisi.masaModuMu,
                            subeId = Convert.ToInt32(Session["subeId"])
                        };
                        if (personelBilgisi.personelParola.Length >= 6 && personelBilgisi.personelParola.Length <= 12)
                        {
                            _personel.personelParola = personelBilgisi.personelParola.ToSHA(Crypto.SHA_Type.SHA256);
                        }
                        else
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                        yonetimModel.Personeller.Add(_personel);
                        yonetimModel.SaveChanges();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }

                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult PersonelGuncelle(Personeller personelBilgisi)
        {
            if (Convert.ToInt32(Session["subeId"]) == personelBilgisi.subeId)
            {
                if (personelBilgisi.personelAd != null && personelBilgisi.personelEmail != null && personelBilgisi.personelParola != null && personelBilgisi.personelTelefon != null && personelBilgisi.subeId>0)
                {
                    try
                    {
                        Personeller personel = yonetimModel.Personeller.Where(x => x.personelId == personelBilgisi.personelId).First();
                        personel.personelAd = personelBilgisi.personelAd;
                        personel.personelEmail = personelBilgisi.personelEmail;
                        if (personelBilgisi.personelParola.Length >= 6 && personelBilgisi.personelParola.Length <= 12)
                        {
                            personel.personelParola = personelBilgisi.personelParola.ToSHA(Crypto.SHA_Type.SHA256);
                        }
                        else
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                        personel.masaModuMu = personel.masaModuMu;
                        personel.personelTelefon = personelBilgisi.personelTelefon;
                        yonetimModel.SaveChanges();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult PersonelSil(int personelId, int subeId, int masaModuPersonelId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    if (personelId != 0)
                    {
                        if (yonetimModel.Personeller.Where(x => x.personelId == personelId && x.yetki == true).Count() == 1)
                        {
                            //Yetkisi olan kişiyi kaldıramazsınız.
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            
                            //Personeli silindi=true yap
                            foreach(var personel in yonetimModel.Personeller.Where(d => d.personelId == personelId && d.yetki == false && d.silindi == false))
                            {
                                personel.silindi = true;
                            }
                            //Veritabanını Kaydet
                            yonetimModel.SaveChanges();
                            return Json(0, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    return Json(3, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult YetkiDegistir(int personelId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    if (Convert.ToInt32(Session["girenid"]) == personelId)
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                    var a = yonetimModel.Personeller.Where(d => d.personelId == personelId).FirstOrDefault();
                    if (a.yetki == true) { a.yetki = false; }
                    else if (a.yetki == false) { a.yetki = true; }
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region // Duyurular

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult DuyuruEkle(Duyurular duyuruBilgisi, HttpPostedFileBase duyuruResimFile)
        {
            if (Convert.ToInt32(Session["subeId"]) == duyuruBilgisi.subeId)
            {
                try
                {
                    Duyurular _duyuru = new Duyurular
                    {
                        duyuruBaslik = duyuruBilgisi.duyuruBaslik,
                        duyuruAciklama = duyuruBilgisi.duyuruAciklama,
                        yayinda = duyuruBilgisi.yayinda,
                        subeId = Convert.ToInt32(Session["subeId"]),
                        duyuruResim = "resimyok.jpg"
                    };
                    var duyuru = yonetimModel.Duyurular.Add(_duyuru);
                    yonetimModel.SaveChanges();
                    if (duyuruResimFile != null && duyuruResimFile.ContentLength > 0)
                    {
                        try
                        {
                            string yol = Path.Combine(Server.MapPath("~/assets/duyururesimleri/"), (duyuru.duyuruId + "." + duyuruResimFile.ContentType.Substring(6)));
                            duyuruResimFile.SaveAs(yol);
                            duyuru.duyuruResim = _duyuru.duyuruId + "." + duyuruResimFile.ContentType.Substring(6);
                            ViewBag.Message = "File uploaded successfully";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                        
                    }
                    else
                    {
                        duyuru.duyuruResim = "resimyok.jpg";
                    }
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult DuyuruGuncelle(Duyurular duyuruBilgisi, HttpPostedFileBase duyuruResimFile)
        {
            if (Convert.ToInt32(Session["subeId"]) == duyuruBilgisi.subeId)
            {
                try
                {
                    var a = yonetimModel.Duyurular.Where(x => x.duyuruId == duyuruBilgisi.duyuruId).First();
                    a.duyuruBaslik = duyuruBilgisi.duyuruBaslik;
                    a.duyuruAciklama = duyuruBilgisi.duyuruAciklama;
                    a.yayinda = duyuruBilgisi.yayinda;
                    if (duyuruResimFile != null && duyuruResimFile.ContentLength > 0)
                    {
                        try
                        {
                            string yol = Path.Combine(Server.MapPath("~/assets/duyururesimleri/"), a.duyuruId + "." + duyuruResimFile.ContentType.Substring(6));
                            duyuruResimFile.SaveAs(yol);
                            ViewBag.Message = "File uploaded successfully";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                        a.duyuruResim = a.duyuruId + "." + duyuruResimFile.ContentType.Substring(6);
                    }
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult DuyuruSil(int duyuruId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //En son ürünleri Temizle
                    yonetimModel.Duyurular.RemoveRange(yonetimModel.Duyurular.Where(d => d.duyuruId == duyuruId));

                    //Veritabanını Kaydet
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        #endregion

        #region // Kampanyalar

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KampanyaEkle(Kampanyalar kampanyaBilgisi, HttpPostedFileBase kampanyaResimFile)
        {
            if (kampanyaBilgisi.subeId == Convert.ToInt32(Session["subeId"]))
            {
                Kampanyalar _kampanya = new Kampanyalar
                {
                    kampanyaAd = kampanyaBilgisi.kampanyaAd,
                    kampanyaFiyat = kampanyaBilgisi.kampanyaFiyat,
                    yayindaMi = kampanyaBilgisi.yayindaMi,
                    kampanyaAciklama = kampanyaBilgisi.kampanyaAciklama,
                    subeId = Convert.ToInt32(Session["subeId"]),
                    menuId =kampanyaBilgisi.menuId,
                    kampanyaResim = "resimyok.jpg"
                };
                var yeniKampanya = yonetimModel.Kampanyalar.Add(_kampanya);
                yonetimModel.SaveChanges();
                if (kampanyaResimFile != null && kampanyaResimFile.ContentLength > 0)
                {
                    try
                    {
                        string yol = Path.Combine(Server.MapPath("~/assets/kampanyaresimleri"), yeniKampanya.kampanyaId.ToString() + Path.GetExtension(kampanyaResimFile.FileName));
                        kampanyaResimFile.SaveAs(yol);
                        ViewBag.Message = "File uploaded successfully";
                        yeniKampanya.kampanyaResim = yeniKampanya.kampanyaId.ToString() + Path.GetExtension(kampanyaResimFile.FileName);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        yeniKampanya.kampanyaResim = "resimyok.jpg";
                        yonetimModel.SaveChanges();
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                else
                {
                    yeniKampanya.kampanyaResim = "resimyok.jpg";
                }
                yonetimModel.SaveChanges();

                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KampanyaGuncelle(Kampanyalar kampanyaBilgisi, HttpPostedFileBase kampanyaResimFile)
        {
            if (kampanyaBilgisi.subeId == Convert.ToInt32(Session["subeId"]))
            {
                foreach (var kampanya in yonetimModel.Kampanyalar.Where(x => x.kampanyaId == kampanyaBilgisi.kampanyaId).ToList())
                {
                    kampanya.kampanyaAd = kampanyaBilgisi.kampanyaAd;
                    kampanya.kampanyaFiyat = kampanyaBilgisi.kampanyaFiyat;
                    kampanya.yayindaMi = kampanyaBilgisi.yayindaMi;
                    kampanya.kampanyaAciklama = kampanyaBilgisi.kampanyaAciklama;
                    if (kampanyaResimFile != null && kampanyaResimFile.ContentLength > 0)
                    {
                        try
                        {
                            string yol = Path.Combine(Server.MapPath("~/assets/kampanyaresimleri"), kampanya.kampanyaId.ToString() + Path.GetExtension(kampanyaResimFile.FileName));
                            kampanyaResimFile.SaveAs(yol);
                            ViewBag.Message = "File uploaded successfully";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                            return Json(1, JsonRequestBehavior.AllowGet);
                        }
                        kampanya.kampanyaResim = kampanya.kampanyaId.ToString() + Path.GetExtension(kampanyaResimFile.FileName);
                    }
                    else
                    {

                    }
                }
                yonetimModel.SaveChanges();
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KampanyaSil(int kampanyaId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                foreach (var kampanya in (yonetimModel.Kampanyalar.Where(d => d.kampanyaId == kampanyaId)))
                {
                    try
                    {
                        string resimYol = Request.MapPath("~/assets/kampanyaresimleri/" + kampanya.kampanyaResim);
                        if (System.IO.File.Exists(resimYol))
                        {
                            System.IO.File.Delete(resimYol);
                        }
                        //Kampanyayı Temizle
                        yonetimModel.Kampanyalar.RemoveRange(yonetimModel.Kampanyalar.Where(d => d.kampanyaId == kampanyaId));
                        yonetimModel.SaveChanges();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        #endregion

        #region // Kampanya Ürünleri

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KampanyaUrunEkle(KampanyaUrunleri kampanyaUrunBilgisi, HttpPostedFileBase kampanyaResimFile, int subeId)
        {
            if (subeId == Convert.ToInt32(Session["subeId"]))
            {
                try
                {
                    KampanyaUrunleri _kampanyaUrun = new KampanyaUrunleri
                    {
                        kampanyaId = kampanyaUrunBilgisi.kampanyaId,
                        urunId = kampanyaUrunBilgisi.urunId
                    };
                    var yeniKampanyaUrun = yonetimModel.KampanyaUrunleri.Add(_kampanyaUrun);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KampanyaUrunSil(int kampanyaUrunleriId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    //Kampanya Ürününü Temizle
                    yonetimModel.KampanyaUrunleri.RemoveRange(yonetimModel.KampanyaUrunleri.Where(d => d.kampanyaUrunleriId == kampanyaUrunleriId));
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region // ÖdemeTürleri

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OdemeTurEkle(OdemeTurleri odemeTurBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    OdemeTurleri _odemeTur = new OdemeTurleri
                    {
                        odemeTurAd = odemeTurBilgisi.odemeTurAd,
                        paraBirimiId = odemeTurBilgisi.paraBirimiId,
                        subeId = odemeTurBilgisi.subeId
                    };
                    yonetimModel.OdemeTurleri.Add(_odemeTur);
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OdemeTurGuncelle(OdemeTurleri odemeTurBilgisi, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    var a = yonetimModel.OdemeTurleri.Where(x => x.odemeTurId == odemeTurBilgisi.odemeTurId).First();
                    a.odemeTurAd = odemeTurBilgisi.odemeTurAd;
                    a.paraBirimiId = odemeTurBilgisi.paraBirimiId;
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [Attributes.SubeYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OdemeTurSil(int odemeTurId, int subeId)
        {
            if (Convert.ToInt32(Session["subeId"]) == subeId)
            {
                try
                {
                    OdemeTurleri odemeTuru = yonetimModel.OdemeTurleri.Where(x => x.odemeTurId == odemeTurId).First();
                    odemeTuru.silindi = true;
                    yonetimModel.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #endregion

    }
}