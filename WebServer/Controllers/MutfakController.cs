using iRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rework;
using iRestaurant.Attributes;

namespace iRestaurant.Controllers
{
    public class MutfakController : Controller
    {
        iRestaurantDataBaseClass mutfakModel = new iRestaurantDataBaseClass();
        #region // SAYFA ÇAĞIRIMLARI

        public ActionResult YoneticiGiris(PersonelHesapViewModel LoginModel, bool? webView)
        {
            PersonelCookieControl personelCookie = new PersonelCookieControl();
            if (personelCookie.CookieGetir() != null)
            {
                HttpCookie cookie = personelCookie.CookieGetir();
                string personelEmail = cookie["personelEmail"].ToString();
                string personelParola = cookie["personelParola"].ToString();
                Personeller personel = mutfakModel.Personeller.Where(x => x.personelEmail == LoginModel.personelEmail && x.personelParola == personelParola).FirstOrDefault();
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
                        Session["subeAd"] = personel.Subeler.subeAd;
                        Session["personelEmail"] = personel.personelEmail;
                        Session["login"] = true;
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
                        Session["GirilenYer"] = "Mutfak";

                        return RedirectToAction("YeniGelenSiparisler", "Mutfak");

                    }
                    else
                    {
                        TempData["Mesaj"] = "Bu panele erişim izniniz bulunmamaktadır. Lütfen şube yetkilisi ile iletişime geçiniz.";
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
                    List<Personeller> personelList = mutfakModel.Personeller.Where(x => x.personelEmail == LoginModel.personelEmail && x.personelParola == personelParola).ToList();
                    if (personelList.Count() > 0)
                    {

                        Personeller personel = personelList.FirstOrDefault();
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
                            Session["subeAd"] = personel.Subeler.subeAd;
                            Session["personelEmail"] = personel.personelEmail;
                            Session["login"] = true;
                            Session["GirilenYer"] = "Mutfak";
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
                            return RedirectToAction("YeniGelenSiparisler", "Mutfak");
                        }
                        else
                        {
                            TempData["Mesaj"] = "Bu panele erişim izniniz bulunmamaktadır. Lütfen şube yetkilisi ile iletişime geçiniz.";
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

        [HttpPost]
        public JsonResult CikisKontrol(int personelId, string personelParola, int subeId)
        {
            if (ModelState.IsValid)
            {
                if (subeId == Convert.ToInt32(Session["subeId"]))
                {
                    personelParola = personelParola.ToSHA(Crypto.SHA_Type.SHA256);
                    var personel = mutfakModel.Personeller.Where(x => x.personelId == personelId && x.personelParola == personelParola && x.yetki == true).FirstOrDefault();
                    if (personel != null)
                    {
                        Session.RemoveAll();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Kasa(int? id)
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = mutfakModel.Katlar.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Masalar = mutfakModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Siparisler = mutfakModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = mutfakModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = mutfakModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = mutfakModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = mutfakModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = mutfakModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = mutfakModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = mutfakModel.Personeller.Where(x => x.subeId == subeId).ToList(),
                Departmanlar = mutfakModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = mutfakModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = mutfakModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = mutfakModel.OdemeTurleri.Where(x => x.subeId == subeId).ToList(),
                OturumKampanyalari = mutfakModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = mutfakModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = mutfakModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = mutfakModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = mutfakModel.ParaBirimleri.ToList()
            };
            if ((id == 0 || id == null))
            {
                vm.Oturumlar = mutfakModel.Oturumlar.Where(x => x.subeId == subeId && x.odendi == false).ToList();
                ViewBag.katId = 0;
            }
            else
            {
                vm.Oturumlar = mutfakModel.Oturumlar.Where(x => x.subeId == subeId && x.odendi == false && x.Masalar.katId == id).ToList();
                ViewBag.katId = id;
            }
            return View(vm);

        }

        #endregion
        

        [MutfakRoleControl]
        public ActionResult YeniGelenSiparisler()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            var yeniGelenSiparisler = mutfakModel.Siparisler.Where(x =>x.subeId == subeId && x.siparisDurumu == false && x.siparisTarihi.Year == DateTime.Now.Year && x.siparisTarihi.Month == DateTime.Now.Month && x.siparisTarihi.Day == DateTime.Now.Day).ToList(); //

            ViewBag.AcikSayfa = "YeniGelenSiparisler";
            ViewBag.Title = "Yeni Gelen Siparişler";

            return View(yeniGelenSiparisler);
        }

        [MutfakRoleControl]
        public ActionResult TamamlananSiparisler()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);

            var tamamlananSiparisler = mutfakModel.Siparisler.Where(x => x.subeId == subeId && x.siparisDurumu == true && x.siparisTarihi.Year == DateTime.Now.Year && x.siparisTarihi.Month == DateTime.Now.Month && x.siparisTarihi.Day == DateTime.Now.Day).ToList(); //

            ViewBag.AcikSayfa = "TamamlananSiparisler";
            ViewBag.Title = "Tamamlanan Siparisler";

            return View(tamamlananSiparisler);
            
        }


        [MutfakAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult TumunuOnayla(int siparislerId)
        {
            int subeId = Convert.ToInt32(Session["subeId"]);

            foreach (var siparisUrunleri in mutfakModel.SiparisUrunleri.Where(x => x.siparislerId == siparislerId).ToList())
            {
                siparisUrunleri.onaylandi = true;
            }
            foreach (var siparis in mutfakModel.Siparisler.Where(x => x.subeId == subeId && x.siparislerId == siparislerId))
            {
                siparis.siparisDurumu = true;
            }
            mutfakModel.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [MutfakAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult TumunuOnaylama(int siparislerId)
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            foreach (var siparisUrunleri in mutfakModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId && x.siparislerId == siparislerId).ToList())
            {
                siparisUrunleri.onaylandi = false;
            }
            foreach (var siparis in mutfakModel.Siparisler.Where(x => x.subeId == subeId && x.siparislerId == siparislerId))
            {
                siparis.siparisDurumu = false;
            }
            mutfakModel.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [MutfakAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult SiparisUrunleriDurumunuDegistir(int siparisUrunlerId)
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            SiparisUrunleri siparisUrunu = mutfakModel.SiparisUrunleri.Where(o => o.Siparisler.subeId == subeId && o.siparisUrunlerId == siparisUrunlerId).First();
            if (siparisUrunu == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            if (siparisUrunu.onaylandi)
                siparisUrunu.onaylandi = false;
            else
                siparisUrunu.onaylandi = true;
            mutfakModel.SaveChanges();

            return Json(siparisUrunu.onaylandi, JsonRequestBehavior.AllowGet);
        }

        [MutfakAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult SiparisleriSay()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            return Json(mutfakModel.Siparisler.Where(x => x.subeId == subeId && x.siparisDurumu == false).Count(), JsonRequestBehavior.AllowGet);
        }
    }
}