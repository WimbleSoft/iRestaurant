using Rework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using iRestaurant.Models;
using iRestaurant;
using System.Threading.Tasks;

namespace iRestaurant.Controllers
{
    public class SirketYonetimController : Controller
    {
        iRestaurantDataBaseClass yonetimModel = new iRestaurantDataBaseClass();
        // GET: Yonetim
        #region // Sayfa Çağırımları
        
        [Attributes.SirketYonetimRoleControl]
        public ActionResult Anasayfa()
        {
            int sirketId = Convert.ToInt32(Session["sirketId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.Subeler.sirketId == sirketId).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.Subeler.sirketId == sirketId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == sirketId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList(),
                Ulkeler = yonetimModel.Ulkeler.ToList(),
                Iller = yonetimModel.Iller.ToList(),
                Ilceler = yonetimModel.Ilceler.ToList(),
                Sirketler = yonetimModel.Sirketler.Where(x => x.sirketId == sirketId).ToList(),
                Subeler = yonetimModel.Subeler.Where(x => x.sirketId == sirketId).ToList()
            };
            return View(vm);
        }

        public ActionResult Cikis(PersonelHesapViewModel LoginModel)
        {
            SirketCookieControl sirketCookie = new SirketCookieControl();
            sirketCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("YoneticiGiris", "SirketYonetim");
        }

        public ActionResult YoneticiGiris(SirketHesapViewModel LoginModel)
        {
            SirketCookieControl sirketCookie = new SirketCookieControl();
            
            if (LoginModel.sirketEmail !=null && LoginModel.sirketParola !=null)
            {
                string sirketParola = LoginModel.sirketParola.ToSHA(Crypto.SHA_Type.SHA256);
                var patron = yonetimModel.Sirketler.Where(x => x.sirketEmail == LoginModel.sirketEmail && x.sirketParola == sirketParola).FirstOrDefault();
                if (patron != null)
                {
                    if (patron.aktiflestirildiMi == true)
                    {
                        sirketCookie.CookieSil();
                        Sirketler sirketBilgisi = new Sirketler
                        {
                            sirketAd = patron.sirketAd,
                            sirketEmail = patron.sirketEmail,
                            sirketParola = patron.sirketParola,
                        };
                        sirketCookie.CookieKaydet(sirketBilgisi);
                        Session["aktiflestirildiMi"] = patron.aktiflestirildiMi;
                        Session["sirketId"] = patron.sirketId;
                        Session["sirketAd"] = patron.sirketAd;
                        Session["sirketEmail"] = patron.sirketEmail;
                        Session["login"] = true;
                        Session["GirilenYer"] = "SirketYonetim";
                        if (LoginModel.returnUrl == null)
                        {
                            return RedirectToAction("Anasayfa", "SirketYonetim");
                        }
                        else
                        {
                            return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "SirketYonetim");
                        }
                    }
                    else
                    {
                        if (sirketCookie.CookieGetir() != null)
                        {
                            sirketCookie.CookieSil();
                        }
                        Session.RemoveAll();
                        TempData["Mesaj"] = "Hesabınız aktifleştirilmeyi bekliyor.";
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Email / şifre bilgileriniz hatalıdır. Lütfen yeniden giriş yapınız.";
                }
                
            }
            else
            {
                if (sirketCookie.CookieGetir() != null)
                {
                    if (LoginModel.returnUrl != null)
                    {//Eğer çıkış yap butonundan buraya getirilmediyse

                        HttpCookie cookie = sirketCookie.CookieGetir();
                        string sirketEmail = cookie["sirketEmail"].ToString();
                        string sirketParola = cookie["sirketParola"].ToString();
                        Sirketler patron = yonetimModel.Sirketler.Where(x => x.sirketEmail == sirketEmail && x.sirketParola == sirketParola).FirstOrDefault();
                        if (patron != null)
                        {
                            if (patron.aktiflestirildiMi == true)
                            {
                                sirketCookie.CookieSil();
                                Sirketler sirketBilgisi = new Sirketler
                                {
                                    sirketAd = patron.sirketAd,
                                    sirketEmail = patron.sirketEmail,
                                    sirketParola = patron.sirketParola,
                                };
                                sirketCookie.CookieKaydet(sirketBilgisi);
                                Session["aktiflestirildiMi"] = patron.aktiflestirildiMi;
                                Session["sirketId"] = patron.sirketId;
                                Session["sirketAd"] = patron.sirketAd;
                                Session["sirketEmail"] = patron.sirketEmail;
                                Session["login"] = true;
                                Session["GirilenYer"] = "SirketYonetim";
                                if (LoginModel.returnUrl == null)
                                {
                                    return RedirectToAction("Anasayfa", "SirketYonetim");
                                }
                                else
                                {
                                    return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "SirketYonetim");
                                }
                            }
                            else
                            {
                                if (sirketCookie.CookieGetir() != null)
                                {
                                    sirketCookie.CookieSil();
                                }
                                Session.RemoveAll();
                                TempData["Mesaj"] = "Hesabınız aktifleştirilmeyi bekliyor.";
                            }
                        }
                        else
                        {
                            TempData["Mesaj"] = "Email / şifre bilgileriniz hatalıdır. Lütfen yeniden giriş yapınız.";
                        }
                        return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "SirketYonetim");
                    }
                    else
                    {//Eğer çıkış yap butonundan buraya getirildiyse
                     //return RedirectToAction("YoneticiGiris", "SirketYonetim");
                    }
                }
            }
            return View("YoneticiGiris");
        }
        
        [Attributes.SirketYonetimRoleControl]
        public ActionResult Personeller()
        {
            int sirketId = Convert.ToInt32(Session["sirketId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.Subeler.sirketId == sirketId).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.Subeler.sirketId == sirketId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == sirketId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList(),
                Ulkeler = yonetimModel.Ulkeler.ToList(),
                Iller = yonetimModel.Iller.ToList(),
                Ilceler = yonetimModel.Ilceler.ToList(),
                Sirketler = yonetimModel.Sirketler.Where(x => x.sirketId == sirketId).ToList(),
                Subeler = yonetimModel.Subeler.Where(x => x.sirketId == sirketId).ToList()
            };
            return View(vm);
        }

        [Attributes.SirketYonetimRoleControl]
        public ActionResult Subeler()
        {
            int sirketId = Convert.ToInt32(Session["sirketId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.Subeler.sirketId == sirketId).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.Subeler.sirketId == sirketId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == sirketId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList(),
                Ulkeler = yonetimModel.Ulkeler.ToList(),
                Iller = yonetimModel.Iller.ToList(),
                Ilceler = yonetimModel.Ilceler.ToList(),
                Sirketler = yonetimModel.Sirketler.Where(x => x.sirketId == sirketId).ToList(),
                Subeler = yonetimModel.Subeler.Where(x => x.sirketId == sirketId).ToList()
            };
            return View(vm);
        }

        [Attributes.SirketYonetimRoleControl]
        public ActionResult Ayarlar()
        {
            int sirketId = Convert.ToInt32(Session["sirketId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.Subeler.sirketId == sirketId).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.Subeler.sirketId == sirketId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == sirketId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList(),
                Ulkeler = yonetimModel.Ulkeler.ToList(),
                Iller = yonetimModel.Iller.ToList(),
                Ilceler = yonetimModel.Ilceler.ToList(),
                Sirketler = yonetimModel.Sirketler.Where(x => x.sirketId == sirketId).ToList(),
                Subeler = yonetimModel.Subeler.Where(x => x.sirketId == sirketId).ToList()
            };
            return View(vm);
        }

        [Attributes.SirketYonetimRoleControl]
        public ActionResult Lisanslar()
        {
            int sirketId = Convert.ToInt32(Session["sirketId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.Subeler.sirketId == sirketId).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.Subeler.sirketId == sirketId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == sirketId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList(),
                Ulkeler = yonetimModel.Ulkeler.ToList(),
                Iller = yonetimModel.Iller.ToList(),
                Ilceler = yonetimModel.Ilceler.ToList(),
                Sirketler = yonetimModel.Sirketler.Where(x => x.sirketId == sirketId).ToList(),
                Subeler = yonetimModel.Subeler.Where(x => x.sirketId == sirketId).ToList(),
                FaturadakiHizmetler = yonetimModel.FaturadakiHizmetler.Where(x => x.Faturalar.sirketId == sirketId).ToList(),
                FaturaOdemeleri=yonetimModel.FaturaOdemeleri.Where(x=>x.Faturalar.sirketId==sirketId).ToList(),
                HizmetKampanyaHizmetleri = yonetimModel.HizmetKampanyaHizmetleri.ToList(),
                HizmetKampanyalari = yonetimModel.HizmetKampanyalari.ToList(),
                Hizmetler = yonetimModel.Hizmetler.ToList(),
                HizmetLisanslari = yonetimModel.HizmetLisanslari.Where(x=>x.Faturalar.sirketId==sirketId).ToList()
                
            };
            return View(vm);
        }
        [Attributes.SirketYonetimRoleControl]
        public ActionResult Faturalar()
        {
            int sirketId = Convert.ToInt32(Session["sirketId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = yonetimModel.Katlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Masalar = yonetimModel.Masalar.Where(x => x.Katlar.Subeler.sirketId == sirketId).ToList(),
                Oturumlar = yonetimModel.Oturumlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Siparisler = yonetimModel.Siparisler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleri = yonetimModel.SiparisUrunleri.Where(x => x.Siparisler.Subeler.sirketId == sirketId).ToList(),
                SiparisUrunleriEkstralari = yonetimModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.Subeler.sirketId == sirketId).ToList(),
                Urunler = yonetimModel.Urunler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                UrunKategoriler = yonetimModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == sirketId).ToList(),
                UrunEkstralar = yonetimModel.UrunEkstralar.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                UrunOzellikler = yonetimModel.UrunOzellikler.Where(x => x.Urunler.Subeler.sirketId == sirketId).ToList(),
                Personeller = yonetimModel.Personeller.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Departmanlar = yonetimModel.Departmanlar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Duyurular = yonetimModel.Duyurular.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                Odemeler = yonetimModel.Odemeler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OdemeTurleri = yonetimModel.OdemeTurleri.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                OturumKampanyalari = yonetimModel.OturumKampanyalari.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Kampanyalar = yonetimModel.Kampanyalar.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                KampanyaUrunleri = yonetimModel.KampanyaUrunleri.Where(x => x.Kampanyalar.Subeler.sirketId == sirketId).ToList(),
                Menuler = yonetimModel.Menuler.Where(x => x.Subeler.sirketId == sirketId).ToList(),
                ParaBirimleri = yonetimModel.ParaBirimleri.ToList(),
                Ulkeler = yonetimModel.Ulkeler.ToList(),
                Iller = yonetimModel.Iller.ToList(),
                Ilceler = yonetimModel.Ilceler.ToList(),
                Sirketler = yonetimModel.Sirketler.Where(x => x.sirketId == sirketId).ToList(),
                Subeler = yonetimModel.Subeler.Where(x => x.sirketId == sirketId).ToList(),
                Faturalar = yonetimModel.Faturalar.Where(x => x.sirketId == sirketId).ToList(),
                FaturadakiHizmetler = yonetimModel.FaturadakiHizmetler.Where(x => x.Faturalar.sirketId == sirketId).ToList(),
                FaturaOdemeleri = yonetimModel.FaturaOdemeleri.Where(x => x.Faturalar.sirketId == sirketId).ToList(),
                HizmetKampanyaHizmetleri = yonetimModel.HizmetKampanyaHizmetleri.ToList(),
                HizmetKampanyalari = yonetimModel.HizmetKampanyalari.ToList(),
                Hizmetler = yonetimModel.Hizmetler.ToList(),
                HizmetLisanslari = yonetimModel.HizmetLisanslari.Where(x => x.Faturalar.sirketId == sirketId).ToList()

            };
            return View(vm);
        }

        public bool SirketAsyncAttributeCookieSessionLogin(SirketHesapViewModel LoginModel)
        {
            if (LoginModel.sirketEmail!=null && LoginModel.sirketParola!=null)
            {
                Sirketler patron = yonetimModel.Sirketler.Where(x => x.sirketEmail == LoginModel.sirketEmail && x.sirketParola == LoginModel.sirketParola && x.aktiflestirildiMi==true).FirstOrDefault();
                if (patron != null)
                {
                    Session["aktiflestirildiMi"] = patron.aktiflestirildiMi;
                    Session["sirketId"] = patron.sirketId;
                    Session["sirketAd"] = patron.sirketAd;
                    Session["sirketEmail"] = patron.sirketEmail;
                    Session["login"] = true;
                    Session["GirilenYer"] = "SirketYonetim";

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
        #endregion

        #region // ASENKRON SORGULAR

        #region // Public Sorgular

        [HttpPost]
        public JsonResult IlcelerGetir(int? ilId)
        {
            List<Ilceler> ilceler = new List<Ilceler>();
            if (ilId.HasValue)
            {
                foreach (var ilce in yonetimModel.Ilceler.Where(x => x.ilId == ilId).ToList())
                {
                    Ilceler yeniIlce = new Ilceler
                    {
                        ilceId = ilce.ilceId,
                        ilceAd = ilce.ilceAd
                    };
                    ilceler.Add(yeniIlce);
                }
            }

            return Json(ilceler.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IllerGetir(int? ulkeId)
        {
            List<Iller> iller = new List<Iller>();
            if (ulkeId.HasValue)
            {
                foreach (var il in yonetimModel.Iller.Where(x => x.ulkeId == ulkeId).ToList())
                {
                    Iller yeniIl = new Iller
                    {
                        ilId = il.ilId,
                        ilAd = il.ilAd
                    };
                    iller.Add(yeniIl);
                }
            }

            return Json(iller.ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region // Şubeler

        [Attributes.SirketYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult SubeEkle(Subeler subeBilgisi)
        {
            if (Convert.ToInt32(Session["sirketId"]) == subeBilgisi.sirketId)
            {
                try
                {
                    Subeler _Sube = new Subeler
                    {
                        subeAd = subeBilgisi.subeAd,
                        subeAdres = subeBilgisi.subeAdres,
                        ilkKurulumYapildiMi = subeBilgisi.ilkKurulumYapildiMi,
                        ilceId = subeBilgisi.ilceId,
                        sirketId = Convert.ToInt32(Session["sirketId"]),
                        duyuruAlaniAktifMi = subeBilgisi.duyuruAlaniAktifMi,
                        kasaYaziciAd = subeBilgisi.kasaYaziciAd,
                        kasaYaziciIp = subeBilgisi.kasaYaziciIp,
                        personelYaziciAd = subeBilgisi.personelYaziciAd,
                        personelYaziciIp = subeBilgisi.personelYaziciIp,
                        personelYazicisiOtomatikMi = subeBilgisi.personelYazicisiOtomatikMi
                    };
                    
                     
                    yonetimModel.Subeler.Add(_Sube);
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

        [Attributes.SirketYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult SubeGuncelle(Subeler subeBilgisi)
        {
            if (Convert.ToInt32(Session["sirketId"]) == subeBilgisi.sirketId)
            {
                try
                {
                    Subeler sube = yonetimModel.Subeler.Where(x => x.subeId == subeBilgisi.subeId).FirstOrDefault();
                    sube.subeAd = subeBilgisi.subeAd;
                    sube.ilceId = subeBilgisi.ilceId;
                    sube.ilkKurulumYapildiMi = subeBilgisi.ilkKurulumYapildiMi;
                    sube.subeAdres = subeBilgisi.subeAdres;
                    sube.personelYazicisiOtomatikMi = subeBilgisi.personelYazicisiOtomatikMi;
                    sube.personelYaziciAd = subeBilgisi.personelYaziciAd;
                    sube.personelYaziciIp = subeBilgisi.personelYaziciIp;
                    sube.kasaYaziciAd = subeBilgisi.kasaYaziciAd;
                    sube.kasaYaziciIp = subeBilgisi.kasaYaziciIp;
                    sube.duyuruAlaniAktifMi = subeBilgisi.duyuruAlaniAktifMi;
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

        [Attributes.SirketYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult SubeSil(int subeId, int sirketId, string sirketParola, string sirketEmail)
        {
            if (Convert.ToInt32(Session["sirketId"]) == sirketId)
            {
                MD5CryptoServiceProvider pwd = new MD5CryptoServiceProvider();
                byte[] byteDegeri = System.Text.Encoding.UTF8.GetBytes(sirketParola);
                byte[] sifreliByte = pwd.ComputeHash(byteDegeri);
                string sifreliSirketParola = Convert.ToBase64String(sifreliByte);
                var sirket = yonetimModel.Sirketler.Where(x => x.sirketId == sirketId && x.sirketParola == sifreliSirketParola && x.sirketEmail == sirketEmail);
                if(sirket.Count()>0)
                {
                    try
                    {
                        //Önce Sipariş Ürün Ekstralarını Temizle
                        yonetimModel.SiparisUrunleriEkstralari.RemoveRange(yonetimModel.SiparisUrunleriEkstralari.Where(d => d.SiparisUrunleri.Siparisler.Oturumlar.Masalar.Katlar.subeId == subeId));
                        yonetimModel.UrunEkstralar.RemoveRange(yonetimModel.UrunEkstralar.Where(d => d.Urunler.subeId == subeId));
                        //Sonra Sipariş Ürünlerini Temizle
                        yonetimModel.SiparisUrunleri.RemoveRange(yonetimModel.SiparisUrunleri.Where(d => d.Siparisler.Oturumlar.Masalar.Katlar.subeId == subeId));
                        yonetimModel.UrunOzellikler.RemoveRange(yonetimModel.UrunOzellikler.Where(d => d.Urunler.subeId == subeId));
                        yonetimModel.KampanyaUrunleri.RemoveRange(yonetimModel.KampanyaUrunleri.Where(d => d.Urunler.subeId == subeId));
                        yonetimModel.Urunler.RemoveRange(yonetimModel.Urunler.Where(d => d.subeId == subeId));
                        //Daha Sonra Siparişleri Temizle
                        yonetimModel.Siparisler.RemoveRange(yonetimModel.Siparisler.Where(d => d.Oturumlar.Masalar.Katlar.Subeler.subeId == subeId));
                        yonetimModel.OturumKampanyalari.RemoveRange(yonetimModel.OturumKampanyalari.Where(d => d.Oturumlar.subeId == subeId));
                        yonetimModel.Kampanyalar.RemoveRange(yonetimModel.Kampanyalar.Where(d => d.subeId == subeId));
                        yonetimModel.UrunKategoriler.RemoveRange(yonetimModel.UrunKategoriler.Where(d => d.Menuler.subeId == subeId));
                        yonetimModel.Departmanlar.RemoveRange(yonetimModel.Departmanlar.Where(d => d.subeId == subeId));
                        yonetimModel.Menuler.RemoveRange(yonetimModel.Menuler.Where(d => d.subeId == subeId));
                        yonetimModel.Odemeler.RemoveRange(yonetimModel.Odemeler.Where(d => d.subeId == subeId));
                        //Daha daha sonra Oturumları Temizle
                        yonetimModel.Oturumlar.RemoveRange(yonetimModel.Oturumlar.Where(d => d.subeId == subeId));
                        //Daha da daha sonra Masaları Temizle
                        yonetimModel.Masalar.RemoveRange(yonetimModel.Masalar.Where(d => d.Katlar.subeId == subeId));
                        yonetimModel.Katlar.RemoveRange(yonetimModel.Katlar.Where(d => d.subeId == subeId));
                        yonetimModel.HizmetLisanslari.RemoveRange(yonetimModel.HizmetLisanslari.Where(d => d.subeId == subeId));
                        yonetimModel.Duyurular.RemoveRange(yonetimModel.Duyurular.Where(d => d.subeId == subeId));
                        //En son Şubeyi Temizle
                        yonetimModel.Subeler.RemoveRange(yonetimModel.Subeler.Where(d => d.subeId == subeId));
                        //Veritabanını Kaydet
                        //yonetimModel.SaveChanges();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
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

        #endregion

        #region // Personeller
        [Attributes.SirketYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult PersonelEkle(Personeller personelBilgisi, int sirketId)
        {
            if (Convert.ToInt32(Session["sirketId"]) == sirketId)
            {
                if (personelBilgisi.personelAd != null && personelBilgisi.personelEmail != null && personelBilgisi.personelParola != null && personelBilgisi.personelTelefon != null && personelBilgisi.subeId > 0)
                {
                    try
                    {
                        string sifreliPersonelParola = personelBilgisi.personelParola.ToSHA(Crypto.SHA_Type.SHA256);
                        Personeller _personel = new Personeller
                        {
                            personelAd = personelBilgisi.personelAd,
                            personelEmail = personelBilgisi.personelEmail,
                            personelTelefon = personelBilgisi.personelTelefon,
                            yetki = true,
                            subeId = personelBilgisi.subeId
                        };
                        if (personelBilgisi.personelParola.Length >= 6 && personelBilgisi.personelParola.Length <= 12)
                        {
                            _personel.personelParola = sifreliPersonelParola;
                        }
                        else
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                        yonetimModel.Personeller.Add(_personel);
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

        [Attributes.SirketYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult PersonelGuncelle(Personeller personelBilgisi, int sirketId)
        {
            if (Convert.ToInt32(Session["sirketId"]) == sirketId)
            {
                if (personelBilgisi.personelAd != null && personelBilgisi.personelEmail != null && personelBilgisi.personelParola != null && personelBilgisi.personelTelefon != null && personelBilgisi.subeId > 0)
                {
                    try
                    {
                        
                        string sifreliPersonelParola = personelBilgisi.personelParola.ToSHA(Crypto.SHA_Type.SHA256);
                        var a = yonetimModel.Personeller.Where(x => x.personelId == personelBilgisi.personelId).First();
                        a.personelAd = personelBilgisi.personelAd;
                        a.personelEmail = personelBilgisi.personelEmail;
                        a.personelTelefon = personelBilgisi.personelTelefon;
                        a.subeId = personelBilgisi.subeId;
                        if (personelBilgisi.personelParola.Length > 0)
                        {
                            if (personelBilgisi.personelParola.Length >= 6 && personelBilgisi.personelParola.Length <= 12)
                            {
                                a.personelParola = sifreliPersonelParola;
                            }
                            else
                            {
                                return Json(2, JsonRequestBehavior.AllowGet);
                            }
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
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        [Attributes.SirketYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult PersonelSil(int personelId, int sirketId)
        {
            if (Convert.ToInt32(Session["sirketId"]) == sirketId)
            {
                try
                {
                    //Personeli silindi yap
                    Personeller personel = yonetimModel.Personeller.Where(x => x.personelId == personelId).First();
                    personel.silindi = true;



                    /* DEPRECATED //YÖNETİCİ OLANLAR SİPARİŞ ALAMAYACAĞI İÇİN NO PROBLEM Personeli sil
                        yonetimModel.Personeller.RemoveRange(yonetimModel.Personeller.Where(d => d.personelId == personelId && d.yetki==true));
                    */

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
        public JsonResult PersonelGeriAl(int personelId, int sirketId)
        {
            if (Convert.ToInt32(Session["sirketId"]) == sirketId)
            {
                try
                {
                    //Personel silindisini geri al
                    Personeller personel = yonetimModel.Personeller.Where(x => x.personelId == personelId).First();
                    personel.silindi = false;



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

        #region // Hizmet Lisansları
        [Attributes.SirketYonetimAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult HizmetLisansiGuncelle(int sirketId, int? subeId, int hizmetLisansId)
        {
            sirketId = (int)Session["sirketId"];
            try
            {
                if (subeId != null)
                {
                    HizmetLisanslari hizmetLisansi = yonetimModel.HizmetLisanslari.Where(x => x.hizmetLisansId == hizmetLisansId).First();
                    Subeler sube = yonetimModel.Subeler.Where(x => x.subeId == subeId && x.sirketId == sirketId).FirstOrDefault();
                    if (sube != null && hizmetLisansi != null)
                    {
                        if (hizmetLisansi.subeId == null)//Bu hizmetlisansına hiç şube atanmamış
                        {
                            if (sube.HizmetLisanslari.Where(x => x.hizmetLisansId == hizmetLisansId).Count() > 0)//Eğer seçilen bu şubede zaten bu lisans varsa
                            {
                                return Json(2, JsonRequestBehavior.AllowGet);
                            }
                            else//eğer seçilen şubede bu lisans yoksa
                            {
                                if (sube.HizmetLisanslari.Where(x => x.hizmetLisansBitisTarihi >= DateTime.Now && x.Hizmetler.hizmetTuru == hizmetLisansi.Hizmetler.hizmetTuru).Count() > 0)//Eğer bu şubede buhizmet türünden süresi henüz bitmemiş zaten varsa
                                {
                                    return Json(3, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    if (hizmetLisansi.Hizmetler.hizmetTuru == 1) //Eğer gelen hizmet lisansının hizmet türü 1 (BULUT SUNUCU) ise atamayı yap
                                    {
                                        hizmetLisansi.subeId = sube.subeId;
                                    }
                                    else                                         //Eğer gelen hizmet lisansının hizmet türü 1 (BULUT SUNUCU) DEĞİL ise, önce o şubede bulut sunucu var mı diye kontrol et ona göre işlem yap
                                    {
                                        if (sube.HizmetLisanslari.Where(x => x.hizmetLisansBitisTarihi >= DateTime.Now && x.Hizmetler.hizmetTuru == 1).Count() == 0) // Eğer bu şubede süresi henüz bitmemiş, hizmetTürü 1 (BULUT SUNUCU) olan hizmet bulunMUyorsa önce bulut sunucu ata diye cevap gönder
                                        {
                                            return Json(6, JsonRequestBehavior.AllowGet);
                                        }
                                        else                                                                                                                         // Eğer bu şubede süresi henüz bitmemiş, hizmetTürü 1 (BULUT SUNUCU) olan hizmet BULUNUYORSA ve gelen hizmet türü numarası 1 değilse
                                        {
                                            hizmetLisansi.subeId = sube.subeId;
                                        }
                                    }
                                    
                                }

                            }
                            if (sube != null)
                            {
                                hizmetLisansi.subeId = subeId;
                            }
                            else
                            {
                                return Json(-1, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            if (sube.HizmetLisanslari.Where(x => x.hizmetLisansId == hizmetLisansId).Count() > 0)//Eğer seçilen bu şubede zaten bu lisans varsa
                            {
                                return Json(2, JsonRequestBehavior.AllowGet);
                            }
                            else//eğer seçilen şubede bu lisans yoksa
                            {
                                if (sube.HizmetLisanslari.Where(x => x.hizmetLisansBitisTarihi <= DateTime.Now && x.Hizmetler.hizmetTuru == hizmetLisansi.Hizmetler.hizmetTuru).Count() == 0)
                                {
                                    hizmetLisansi.subeId = sube.subeId;
                                }
                                else
                                {
                                    return Json(3, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }
                    }
                    else
                    {
                        return Json(4, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HizmetLisanslari hizmetLisansi = yonetimModel.HizmetLisanslari.Where(x => x.hizmetLisansId == hizmetLisansId).First();
                    if(hizmetLisansi != null)
                    {
                        if (hizmetLisansi.Hizmetler.hizmetTuru == 1)//EĞER KALDIRILMAYA ÇALIŞILAN LİSANSIN HİZMETTÜRÜ 1 (BULUT SUNUCU) İSE, BU ŞUBEDEN SÜRESİ GEÇMEMİŞ TÜM LİSANSLARI KLADIR
                        {

                            foreach(var hizmetLisans in hizmetLisansi.Subeler.HizmetLisanslari.Where(x => x.hizmetLisansBitisTarihi >= DateTime.Now))
                            {
                                hizmetLisans.subeId = null;
                            }
                            yonetimModel.SaveChanges();
                            return Json(7, JsonRequestBehavior.AllowGet);
                        }
                        else //EĞER KALDIRILMAYA ÇALIŞILAN LİSANSIN HİZMETTÜRÜ 1 (BULUT SUNUCU) ///DEĞİL\\\ İSE, BU ŞUBEDEN BU LİSANSI KLADIR
                        {
                            hizmetLisansi.subeId = null;
                        }
                        
                    }
                    else
                    {
                        return Json(5, JsonRequestBehavior.AllowGet);
                    }
                }


                yonetimModel.SaveChanges();
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

    }
}