using Rework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using iRestaurant.Attributes;
using iRestaurant.Models;

namespace iRestaurant.Controllers
{
    public class KasaController : Controller
    {
        iRestaurantDataBaseClass kasaModel = new iRestaurantDataBaseClass();
        YazdiriciController yaziciController = new YazdiriciController();
        #region // SAYFA ÇAĞIRIMLARI

        public ActionResult YoneticiGiris(PersonelHesapViewModel LoginModel, bool? webView)
        {
            PersonelCookieControl personelCookie = new PersonelCookieControl();
            if (personelCookie.CookieGetir() != null)
            {
                HttpCookie cookie = personelCookie.CookieGetir();
                string personelEmail = cookie["personelEmail"].ToString();
                string personelParola = cookie["personelParola"].ToString();
                Personeller personel = kasaModel.Personeller.Where(x => x.personelEmail == LoginModel.personelEmail && x.personelParola == personelParola).FirstOrDefault();
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
                        Session["GirilenYer"] = "Kasa";

                        return RedirectToAction("Kasa", "Kasa");
                        
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
                    List<Personeller> personelList = kasaModel.Personeller.Where(x => x.personelEmail == LoginModel.personelEmail && x.personelParola == personelParola).ToList();
                    if (personelList.Count()>0)
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
                            Session["GirilenYer"] = "Kasa";
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
                            return RedirectToAction("Kasa", "Kasa");
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
                    var personel = kasaModel.Personeller.Where(x => x.personelId == personelId && x.personelParola == personelParola && x.yetki == true).FirstOrDefault();
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

        [KasaRoleControl]
        public ActionResult Kasa(int? id)
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            yaziciController.Initialize(subeId);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = kasaModel.Katlar.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Masalar = kasaModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Siparisler = kasaModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = kasaModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = kasaModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = kasaModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = kasaModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = kasaModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = kasaModel.Personeller.Where(x => x.subeId == subeId).ToList(),
                Departmanlar = kasaModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = kasaModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = kasaModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri = kasaModel.OdemeTurleri.Where(x => x.subeId == subeId).ToList(),
                OturumKampanyalari = kasaModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = kasaModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = kasaModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = kasaModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = kasaModel.ParaBirimleri.ToList()
            };
            if ((id == 0 || id == null))
            {
                vm.Oturumlar = kasaModel.Oturumlar.Where(x => x.subeId == subeId && x.odendi == false).ToList();
                ViewBag.katId = 0;
            }
            else
            {
                vm.Oturumlar = kasaModel.Oturumlar.Where(x => x.subeId == subeId && x.odendi == false && x.Masalar.katId == id).ToList();
                ViewBag.katId = id;
            }
            return View(vm);
            
        }

        [KasaRoleControl]
        public ActionResult Masalar()
        {
            int subeId = Convert.ToInt32(Session["subeId"]);
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Katlar = kasaModel.Katlar.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                Masalar = kasaModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                Oturumlar = kasaModel.Oturumlar.Where(x => x.subeId == subeId && x.odendi == false).ToList(),
                Siparisler = kasaModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                SiparisUrunleri = kasaModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                SiparisUrunleriEkstralari = kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                Urunler = kasaModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                UrunKategoriler = kasaModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                UrunEkstralar = kasaModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                UrunOzellikler = kasaModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                Personeller = kasaModel.Personeller.Where(x => x.subeId == subeId).ToList(),
                Departmanlar = kasaModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                Duyurular = kasaModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                Odemeler = kasaModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                OdemeTurleri= kasaModel.OdemeTurleri.Where(x=>x.subeId==subeId).ToList(),
                OturumKampanyalari = kasaModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Kampanyalar = kasaModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                KampanyaUrunleri = kasaModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                Menuler = kasaModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                ParaBirimleri = kasaModel.ParaBirimleri.ToList()
            };
            return View(vm);
        }

        [KasaRoleControl]
        [HttpPost]
        public ActionResult Oturumlar(int oturumId, int subeId)
        {
            if(subeId == Convert.ToInt32(Session["subeId"]))
            {
                ViewModelDemoVM vm = new ViewModelDemoVM
                {
                    Katlar = kasaModel.Katlar.Where(x => x.subeId == subeId && x.silindi == false).ToList(),
                    Masalar = kasaModel.Masalar.Where(x => x.Katlar.subeId == subeId && x.Katlar.silindi == false).ToList(),
                    Oturumlar = kasaModel.Oturumlar.Where(x => x.subeId == subeId && x.odendi == false && x.oturumId == oturumId).ToList(),
                    Siparisler = kasaModel.Siparisler.Where(x => x.subeId == subeId).ToList(),
                    SiparisUrunleri = kasaModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId).ToList(),
                    SiparisUrunleriEkstralari = kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId).ToList(),
                    Urunler = kasaModel.Urunler.Where(x => x.subeId == subeId).ToList(),
                    UrunKategoriler = kasaModel.UrunKategoriler.Where(x => x.Departmanlar.subeId == subeId).ToList(),
                    UrunEkstralar = kasaModel.UrunEkstralar.Where(x => x.Urunler.subeId == subeId).ToList(),
                    UrunOzellikler = kasaModel.UrunOzellikler.Where(x => x.Urunler.subeId == subeId).ToList(),
                    Personeller = kasaModel.Personeller.Where(x => x.subeId == subeId).ToList(),
                    Departmanlar = kasaModel.Departmanlar.Where(x => x.subeId == subeId).ToList(),
                    Duyurular = kasaModel.Duyurular.Where(x => x.subeId == subeId).ToList(),
                    Odemeler = kasaModel.Odemeler.Where(x => x.subeId == subeId).ToList(),
                    OdemeTurleri = kasaModel.OdemeTurleri.Where(x => x.subeId == subeId).ToList(),
                    OturumKampanyalari = kasaModel.OturumKampanyalari.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                    Kampanyalar = kasaModel.Kampanyalar.Where(x => x.subeId == subeId).ToList(),
                    KampanyaUrunleri = kasaModel.KampanyaUrunleri.Where(x => x.Kampanyalar.subeId == subeId).ToList(),
                    Menuler = kasaModel.Menuler.Where(x => x.subeId == subeId).ToList(),
                    ParaBirimleri = kasaModel.ParaBirimleri.ToList()
                };
                return View(vm);
            }
            return null;
            
        }
        #endregion
        
        //HTTPPOST AJAX JSON FONKSİYONLAR
        #region // JSON SORGULAR

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KasaDokumu(DateTime baslangicTarihi, DateTime bitisTarihi, int subeId)
        {
            if (subeId == Convert.ToInt32(Session["subeId"]))
            {
                try
                {
                    List<Ciro> ciroList = new List<Ciro>();
                    
                    //Silinmemiş ödeme türlerini listeye ekle, toplam ödeme miktarlarını getir.
                    foreach(var odemeTuru in kasaModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi==false))
                    {
                        ciroList.Add(new Ciro { odemeTurAd = odemeTuru.odemeTurAd, miktar = odemeTuru.Odemeler.Where(x => x.subeId == subeId).Sum(x => x.odemeMiktar), paraBirimiAd = odemeTuru.ParaBirimleri.paraBirimiAd });
                    }

                    //Silinmiş ödeme türlerini paraBirimiId'lerine göre grupla, her grubu Silinmiş (parabirimiad) adıyla sırala, toplam ödeme miktarlarını getir.
                    foreach (var odemeTuru in kasaModel.OdemeTurleri.Where(x => x.subeId == subeId && x.silindi == true).GroupBy(x=>x.paraBirimiId))
                    {
                        ParaBirimleri paraBirimi = kasaModel.ParaBirimleri.Find(odemeTuru.Key);
                        string paraBirimiAd = "xx";
                        if (paraBirimi != null)
                        {
                            paraBirimiAd = paraBirimi.paraBirimiAd;
                        }
                        ciroList.Add(new Ciro {
                            odemeTurAd = "Silinen Tür "/* + kasaModel.ParaBirimleri.Find(odemeTuru.Key).paraBirimiAd*/,
                            miktar = kasaModel.Odemeler.Where(x => x.subeId == subeId && x.OdemeTurleri.silindi == true && x.OdemeTurleri.paraBirimiId == odemeTuru.Key).Sum(x => x.odemeMiktar),
                            paraBirimiAd = paraBirimiAd
                        });
                    }

                    return Json(ciroList, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult Footer(int subeId)
        {
            if(subeId == Convert.ToInt32(Session["subeId"]))
            {
                try
                {
                    DateTime sabahTarihSaat = DateTime.Now.Date.AddHours(3);//KASADA GÖZÜKEN O GÜN SABAH SAAT 6'DAN
                    DateTime geceTarihSaat = DateTime.Now.Date.AddDays(1).AddHours(3);//KASADA GÖZÜKEN O GÜN GECE 24'TEN SONRAKİ 3 SAATE KADAR
                    if (DateTime.Now.TimeOfDay <= new TimeSpan(3, 0, 0) && DateTime.Now.TimeOfDay >= new TimeSpan(0, 0, 0))
                    {
                        sabahTarihSaat = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));
                        geceTarihSaat = DateTime.Now;
                    }


                    double? masalardaToplam = 0;
                    masalardaToplam +=
                       kasaModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId && x.Siparisler.siparisTarihi >= sabahTarihSaat && x.Siparisler.siparisTarihi <= geceTarihSaat && x.odendi == false && x.oturumKampanyaId == null && x.Siparisler.Oturumlar.odendi == false).Sum(x => (double?)x.Urunler.urunFiyat) +
                       kasaModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId && x.Siparisler.siparisTarihi >= sabahTarihSaat && x.Siparisler.siparisTarihi <= geceTarihSaat && x.odendi == false && x.Siparisler.Oturumlar.odendi == false).Sum(y => (double?)y.UrunOzellikler.urunOzellikFiyat) +
                       kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.subeId == subeId && x.SiparisUrunleri.Siparisler.siparisTarihi >= sabahTarihSaat && x.SiparisUrunleri.Siparisler.siparisTarihi <= geceTarihSaat && x.odendi == false && x.SiparisUrunleri.Siparisler.Oturumlar.odendi == false).Sum(x => (double?)x.UrunEkstralar.urunEkstraFiyat);
                    double? kampanyaToplam = 0;
                    foreach (var siparisUrunGroupByOturumKampanyaId in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.subeId == subeId && x.Siparisler.siparisTarihi >= sabahTarihSaat && x.Siparisler.siparisTarihi <= geceTarihSaat && x.odendi == false && x.oturumKampanyaId != null && x.Siparisler.Oturumlar.odendi == false).GroupBy(x => x.oturumKampanyaId))
                    {
                        kampanyaToplam += kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == siparisUrunGroupByOturumKampanyaId.Key).Sum(x => (double?)x.Kampanyalar.kampanyaFiyat);
                    }
                    masalardaToplam += kampanyaToplam;

                    double? kasadaToplam = 0;
                    kasadaToplam += kasaModel.Odemeler.Where(x => x.subeId == subeId && x.odemeTarihi >= sabahTarihSaat && x.odemeTarihi <= geceTarihSaat).Sum(x => (double?)x.odemeMiktar);

                    if (masalardaToplam == null) masalardaToplam = 0;
                    if (kasadaToplam == null) kasadaToplam = 0;
                    object obj = new
                    {
                        masalardaToplamObj = masalardaToplam,
                        kasadaToplamObj = kasadaToplam
                    };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }
        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult TamamiOdendiMi(int oturumId, int subeId)
        {
            bool durum = false;
            try
            {
                durum = TamamiOdendiMiAcaba(oturumId, subeId);
            }
            catch (Exception error) {
                Console.Write(error);
            };

            return Json(durum, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OturumlariSay(int? id, int subeId)
        {
            int a = kasaModel.Oturumlar.Where(x => x.odendi == false).Count();
            if (id != null)
                a = kasaModel.Oturumlar.Where(x => x.odendi == false && x.Masalar.katId == id).Count();

            return Json(a, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult EkstralilarinCheckboxDegerleriDiziHandler(List<EkstraliCheckbox> ekstralilarinCheckboxDegerleriDizi, int oturumId, int subeId)
        {
            if (ekstralilarinCheckboxDegerleriDizi != null)
            {
                foreach (var b in ekstralilarinCheckboxDegerleriDizi.ToList())
                {
                    foreach(var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.siparisUrunlerId == b.siparisUrunlerId).ToList())
                    {
                        foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId).ToList())
                        {
                            //Sipariş Ürün Ekstralarını Ödendi=true yapar
                            siparisUrunEkstrasi.odendi = true;
                            //kasaModel.SaveChanges();
                        }
                        siparisUrunu.odendi = true;
                        if (siparisUrunu.odenmeTarihi == null)
                            siparisUrunu.odenmeTarihi = DateTime.Now;
                        kasaModel.SaveChanges();
                    }
                    kasaModel.SaveChanges();
                }
                
                //int siparisUrunlerId = ekstralilarinCheckboxDegerleriDizi.Take(1).FirstOrDefault().siparisUrunlerId;
                //TamamiOdendiMiAcaba(oturumId);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KampanyalilarinCheckboxDegerleriDiziHandler(List<KampanyaliCheckbox> kampanyalilarinCheckboxDegerleriDizi, int oturumId, int subeId)
        {
            if (kampanyalilarinCheckboxDegerleriDizi != null)
            {
                foreach (var b in kampanyalilarinCheckboxDegerleriDizi.ToList())
                {
                    foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == b.oturumKampanyaId))
                    {
                        foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId).ToList())
                        {
                            //Sipariş Ürün Ekstralarını Ödendi=true yapar
                            siparisUrunEkstrasi.odendi = true;
                            //kasaModel.SaveChanges();
                        }
                        siparisUrunu.odendi = true;
                        if (siparisUrunu.odenmeTarihi == null)
                            siparisUrunu.odenmeTarihi = DateTime.Now;
                    }
                    kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == b.oturumKampanyaId).FirstOrDefault().odendi = true;
                    //kasaModel.SaveChanges();
                }
                kasaModel.SaveChanges();
                //int oturumKampanyaId = kampanyalilarinCheckboxDegerleriDizi.Take(1).FirstOrDefault().oturumKampanyaId;
                
                //TamamiOdendiMiAcaba(oturumId);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public bool KasaAsyncAttributeCookieSessionLogin(PersonelHesapViewModel LoginModel, bool? webView)
        {
            if (ModelState.IsValid)
            {
                List<Personeller> personelList = kasaModel.Personeller.Where(x => x.Subeler.Sirketler.sirketEmail == LoginModel.sirketEmail && x.personelEmail == LoginModel.personelEmail && x.personelParola == LoginModel.personelParola && x.yetki == true).ToList();
                if (personelList.Count == 1)
                {
                    Personeller personel = kasaModel.Personeller.Where(x => x.Subeler.Sirketler.sirketEmail == LoginModel.sirketEmail && x.personelEmail == LoginModel.personelEmail && x.personelParola == LoginModel.personelParola && x.yetki == true).FirstOrDefault();
                    if (personel != null)
                    {
                        System.Web.HttpContext.Current.Session["personelAd"] = personel.personelAd;
                        System.Web.HttpContext.Current.Session["yetki"] = personel.yetki;
                        System.Web.HttpContext.Current.Session["girenid"] = personel.personelId;
                        System.Web.HttpContext.Current.Session["subeId"] = personel.subeId;
                        System.Web.HttpContext.Current.Session["sirketAd"] = personel.Subeler.Sirketler.sirketAd;
                        System.Web.HttpContext.Current.Session["sirketEmail"] = personel.Subeler.Sirketler.sirketEmail;
                        System.Web.HttpContext.Current.Session["subeAd"] = personel.Subeler.subeAd;
                        System.Web.HttpContext.Current.Session["personelEmail"] = personel.personelEmail;
                        System.Web.HttpContext.Current.Session["login"] = true;
                        if (webView.HasValue)
                        {
                            if (webView.Value == true)
                            {
                                System.Web.HttpContext.Current.Session["webView"] = true;
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session["webView"] = false;
                            }
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session["webView"] = false;
                        }

                        System.Web.HttpContext.Current.Session["GirilenYer"] = "Kasa";

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
            else
            {
                return false;
            }
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult EkstrasizlarHandler(Ekstrasiz[] ekstrasizlar, int oturumId, int subeId)
        {
            if (ekstrasizlar != null)
            {
                foreach (var ekstrasizUrun in ekstrasizlar)
                {
                    foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.urunOzellikId == ekstrasizUrun.urunOzellikId && x.odendi == false && x.Siparisler.oturumId == ekstrasizUrun.oturumId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId && x.odendi == false).Count() == 0).Take(ekstrasizUrun.siparisAdedi).ToList())
                    {
                        //Gönderilen Sipariş adedi kadar siparişÜrününü ödendi=true yapar
                        siparisUrunu.odendi = true;
                        if(siparisUrunu.odenmeTarihi==null)
                            siparisUrunu.odenmeTarihi = DateTime.Now;
                        /*Zaten ekstrasız sipariş ürünleri listelenmekte
                        foreach (var siparisUrunuEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId).ToList())
                        {
                            siparisUrunuEkstrasi.odendi = true;
                        }
                        */
                    }
                }
                kasaModel.SaveChanges();
                //TamamiOdendiMiAcaba(oturumId);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult MasaDegistir(int oturumId, int yeniMasaId, int subeId)
        {
           
            foreach(var oturum in kasaModel.Oturumlar.Where(x => x.oturumId == oturumId))
            {
                foreach(var masa in kasaModel.Masalar.Where(x => x.masaId == oturum.masaId))
                {
                    masa.durum = false;
                }
                oturum.masaId = yeniMasaId;
                
            }
            foreach (var masa in kasaModel.Masalar.Where(x => x.masaId == yeniMasaId))
            {
                masa.durum = true;
            }
            /*
            kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).FirstOrDefault().Masalar.durum = false;
            kasaModel.Masalar.Where(x => x.masaId == yeniMasaId).FirstOrDefault().durum = true;
            kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).FirstOrDefault().masaId = yeniMasaId;
            */
            kasaModel.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult DokumAl(DateTime baslangicTarihi, DateTime bitisTarihi, int subeId)
        {
            List<PersonelPerformans> personelPerformansList = new List<PersonelPerformans>();
            List<OdemeYontemiDokum> odemeYontemiDokumuList = new List<OdemeYontemiDokum>();
            bitisTarihi = bitisTarihi.AddSeconds(59).AddMilliseconds(999);
            #region // PERSONELPERFORMANS

            #region // silinmemiş personel performansı
            foreach (var personel in kasaModel.Personeller.Where(x=>x.subeId == subeId && x.silindi==false).ToList())
            {
                PersonelPerformans personelPerformans = new PersonelPerformans();
                double personelToplamMiktar=0;
                double toplamMiktar = 0;

                //PERSONELE AİT TOPLAM 

                //KAMPANYASIZ SİPARİŞ ÜRÜNLERİNİN FİYATLARI, ÜRÜNLERİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    personelToplamMiktar += siparisurunu.UrunOzellikler.urunOzellikFiyat; //Personele ait Kampanyasız girilen bütün özellikler toplamı
                    personelToplamMiktar += siparisurunu.Urunler.urunFiyat; //Personele ait Kampanyasız girilen ürünlerin fiyatını toplar
                    foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisurunu.siparisUrunlerId).ToList())
                    {
                        personelToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Personele ait Kampanyasız girilen bütün ekstraların toplamı
                    }

                }

                //\\//\\

                //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    // Aşağıdaki satır Personele ait Kampanyalı girilen ürünlerin aynı kampanyaya ait olanlarını gruplayıp, o gruba ait ürünlerin ürün özelliklerin toplar
                    personelToplamMiktar += kasaModel.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId == urunGrubu.Key && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).Sum(x => x.UrunOzellikler.urunOzellikFiyat); 
                    
                    foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.personelId == personel.personelId && x.SiparisUrunleri.oturumKampanyaId == urunGrubu.Key).ToList())
                    {
                        personelToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Personele ait Kampanyalı girilen bütün ekstraların toplamı
                    }

                }
                foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    personelToplamMiktar += kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == urunGrubu.Key).FirstOrDefault().Kampanyalar.kampanyaFiyat; //Aynı Kampanyaya ait ürünleri gruplar ve kampanyalı fiyatını ekler
                }
                /*
                foreach(var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    personelToplamMiktar += siparisurunu.Urunler.urunFiyat; //Kampanyasız girilen ürünlerin fiyatını toplar
                }
                */
                //PERSONELLER TOPLAMI

                //KAMPANYASIZ SİPARİŞ ÜRÜNLERİNİN FİYATLARI, ÜRÜNLERİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    toplamMiktar += siparisurunu.UrunOzellikler.urunOzellikFiyat; //Tüm Personele ait bütün özellikler toplamı
                    toplamMiktar += siparisurunu.Urunler.urunFiyat; // Tüm Personele ait bütün ürün fiyatlar toplamı
                    foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisurunu.siparisUrunlerId).ToList())
                    {
                        toplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Tüm Personele ait bütün ekstraların toplamı
                    }

                }

                //\\//\\

                //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    //Aşağıdaki satır Tüm Personele ait Kampanyalı girilen ürünlerin aynı kampanyaya ait olanlarını gruplayıp, o gruba ait ürünlerin ürün özelliklerin toplar
                    toplamMiktar += kasaModel.SiparisUrunleri.Where(x =>  x.oturumKampanyaId == urunGrubu.Key && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
                    foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.oturumKampanyaId == urunGrubu.Key).ToList())
                    {
                        toplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Tüm Personele ait bütün ekstraların toplamı
                    }
                }
                //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN KAMPANYA FİYATLARI TOPLAMI
                foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    toplamMiktar += kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == urunGrubu.Key).FirstOrDefault().Kampanyalar.kampanyaFiyat; //Aynı Kampanyaya ait ürünleri gruplar ve kampanyalı fiyatını ekler
                }
                /*
                foreach (var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    toplamMiktar += siparisurunu.Urunler.urunFiyat; //Kampanyasız girilen ürünlerin fiyatını toplar
                }
                */
                personelPerformans.personelId       = personel.personelId;
                personelPerformans.personelAd       = personel.personelAd;
                personelPerformans.personelMiktar   = personelToplamMiktar;
                personelPerformans.personelYuzde    = personelToplamMiktar * 100/toplamMiktar;
                personelPerformansList.Add(personelPerformans);                 
            }
            #endregion

            #region // silinen personel performansı

            string silinenPersonelAd = "SİLİNDİ";
            int silinenPersonelId = 0;

            /*
            foreach (var personel in kasaModel.Personeller.Where(x => x.subeId == subeId && x.silindi == true).ToList())
            {
            */
            PersonelPerformans silinenPersonelPerformans = new PersonelPerformans();
            double silinenPersonelToplamMiktar = 0;
            double silinenToplamMiktar = 0;

            //PERSONELE AİT TOPLAM 

            //KAMPANYASIZ SİPARİŞ ÜRÜNLERİNİN FİYATLARI, ÜRÜNLERİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
            foreach (var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.Personeller.silindi == true && x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
            {
                silinenPersonelToplamMiktar += siparisurunu.UrunOzellikler.urunOzellikFiyat; //Personele ait Kampanyasız girilen bütün özellikler toplamı
                silinenPersonelToplamMiktar += siparisurunu.Urunler.urunFiyat; //Personele ait Kampanyasız girilen ürünlerin fiyatını toplar
                foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisurunu.siparisUrunlerId).ToList())
                {
                    silinenPersonelToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Personele ait Kampanyasız girilen bütün ekstraların toplamı
                }

            }

            //\\//\\

            //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
            foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.Personeller.silindi == true && x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
            {
                // Personele ait Kampanyalı girilen ürünlerin aynı kampanyaya ait olanlarını gruplayıp, o gruba ait ürünlerin ürün özelliklerin fiyatını toplar
                silinenPersonelToplamMiktar += kasaModel.SiparisUrunleri.Where(x => x.Siparisler.Personeller.silindi == true && x.oturumKampanyaId == urunGrubu.Key && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).Sum(x => x.UrunOzellikler.urunOzellikFiyat);

                // Personele ait Kampanyalı girilen bütün ekstraların toplamı
                foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.Personeller.silindi == true && x.SiparisUrunleri.oturumKampanyaId == urunGrubu.Key).ToList())
                {
                    silinenPersonelToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  
                }

            }

            //Aynı Kampanyaya ait ürünleri gruplar ve kampanyalı fiyatını ekler
            foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.Personeller.silindi == true && x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
            {
                silinenPersonelToplamMiktar += kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == urunGrubu.Key).FirstOrDefault().Kampanyalar.kampanyaFiyat; 
            }

            /*
            foreach(var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
            {
                personelToplamMiktar += siparisurunu.Urunler.urunFiyat; //Kampanyasız girilen ürünlerin fiyatını toplar
            }
            */




            // TÜM PERSONELLERİN İŞLEMLERİ TOPLAMI


            //KAMPANYASIZ SİPARİŞ ÜRÜNLERİNİN FİYATLARI, ÜRÜNLERİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
            foreach (var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
            {
                silinenToplamMiktar += siparisurunu.UrunOzellikler.urunOzellikFiyat; //Tüm Personele ait bütün özellikler toplamı
                silinenToplamMiktar += siparisurunu.Urunler.urunFiyat; // Tüm Personele ait bütün ürün fiyatlar toplamı
                foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisurunu.siparisUrunlerId).ToList())
                {
                    silinenToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Tüm Personele ait bütün ekstraların toplamı
                }

            }

            //\\//\\

            //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
            foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
            {
                //Aşağıdaki satır Tüm Personele ait Kampanyalı girilen ürünlerin aynı kampanyaya ait olanlarını gruplayıp, o gruba ait ürünlerin ürün özelliklerin toplar
                silinenToplamMiktar += kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == urunGrubu.Key && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
                foreach (var siparisurunekstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.oturumKampanyaId == urunGrubu.Key).ToList())
                {
                    silinenToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Tüm Personele ait bütün ekstraların toplamı
                }
            }
            //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN KAMPANYA FİYATLARI TOPLAMI
            foreach (var urunGrubu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
            {
                silinenToplamMiktar += kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == urunGrubu.Key).FirstOrDefault().Kampanyalar.kampanyaFiyat; //Aynı Kampanyaya ait ürünleri gruplar ve kampanyalı fiyatını ekler
            }



            /*
            foreach (var siparisurunu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
            {
                toplamMiktar += siparisurunu.Urunler.urunFiyat; //KAMPANYASIZ SİPARİŞ ÜRÜNLERİNİN FİYATLARI TOPLAMI
            }
            */


            silinenPersonelPerformans.personelId = silinenPersonelId;
            silinenPersonelPerformans.personelAd = silinenPersonelAd;
            silinenPersonelPerformans.personelMiktar = silinenPersonelToplamMiktar;
            silinenPersonelPerformans.personelYuzde = silinenPersonelToplamMiktar * 100 / silinenToplamMiktar;
            personelPerformansList.Add(silinenPersonelPerformans);
            /*
            }
            */


            #endregion



            #endregion
            #region // ODEMEYONTEMIDOKUMU
            double toplamOdeme = 0;
            foreach (var odeme in kasaModel.Odemeler.Where(x => x.subeId == subeId && x.odemeTarihi >= baslangicTarihi && x.odemeTarihi <= bitisTarihi))
            {
                toplamOdeme += odeme.odemeMiktar;
            }
            foreach (var odeme in kasaModel.Odemeler.Where(x => x.subeId == subeId && x.odemeTarihi >= baslangicTarihi && x.odemeTarihi <= bitisTarihi).GroupBy(x=>x.OdemeTurleri.odemeTurAd))
            {
                OdemeYontemiDokum odemeYontemi = new OdemeYontemiDokum
                {
                    yontemAd = odeme.Key,
                    yontemMiktar = odeme.Sum(x => x.odemeMiktar)
                };
                odemeYontemi.yontemYuzde = odemeYontemi.yontemMiktar * 100 / toplamOdeme;


                odemeYontemiDokumuList.Add(odemeYontemi);
            }
            #endregion

            yaziciController.DokumYazdir(personelPerformansList, odemeYontemiDokumuList, baslangicTarihi, bitisTarihi);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult FisYazdir(Ekstrasiz[] ekstrasizlar, List<KampanyaliCheckbox> kampanyalilarinCheckboxDegerleriDizi, List<EkstraliCheckbox> ekstralilarinCheckboxDegerleriDizi)
        {
            List<FisItem> yazdirilacakFis = new List<FisItem>();
            #region EKSTRASIZLAR
            if (ekstrasizlar != null)
            {
                foreach (var a in ekstrasizlar)
                {
                    foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.urunOzellikId == a.urunOzellikId && x.Siparisler.oturumId == a.oturumId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId).Count() == 0).Take(a.siparisAdedi).GroupBy(x => x.urunOzellikId).ToList())
                    {
                        yazdirilacakFis.Add(new FisItem
                        {
                            urunAdedi = a.siparisAdedi,
                            ekstraToplami=0,
                            urunAd = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == a.urunOzellikId).FirstOrDefault().Urunler.urunAd + " - " + kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == a.urunOzellikId).FirstOrDefault().urunOzellikAd,
                            urunOzellikFiyatToplam = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == a.urunOzellikId).FirstOrDefault().urunOzellikFiyat + kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == a.urunOzellikId).FirstOrDefault().Urunler.urunFiyat
                        });
                    }
                }
            }
            #endregion EKSTRASIZLAR
            #region EKSTRALILAR
            if (ekstralilarinCheckboxDegerleriDizi != null)
            {
                foreach (var b in ekstralilarinCheckboxDegerleriDizi.ToList())
                {
                    var siparisUrunu = kasaModel.SiparisUrunleri.Where(x => x.siparisUrunlerId == b.siparisUrunlerId).FirstOrDefault();
                    double ekstraToplami = 0;
                    string urunAd = siparisUrunu.Urunler.urunAd + " - " + siparisUrunu.UrunOzellikler.urunOzellikAd;
                    double ozellikToplam = siparisUrunu.UrunOzellikler.urunOzellikFiyat;
                    double urunOzellikFiyatToplam = siparisUrunu.Urunler.urunFiyat + ozellikToplam;
                    foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId))
                    {
                        ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                    }
                    
                    int urunAdUzunlugu = urunAd.Length;
                    if (urunAdUzunlugu > 24)
                    {
                        urunAd.Remove(24);
                    }
                    urunAdUzunlugu = urunAd.Length;
                    if (ekstraToplami > 0 || ozellikToplam > 0)
                    {
                        for (int i = 1; i <= (24 - urunAdUzunlugu); i++)
                        {
                            urunAd += " ";
                        }
                        urunAd += " (Ek.)";
                    }
                    yazdirilacakFis.Add(new FisItem
                    {
                        urunAdedi = 1,
                        ekstraToplami= ekstraToplami,
                        urunAd =urunAd,
                        urunOzellikFiyatToplam = siparisUrunu.UrunOzellikler.urunOzellikFiyat + siparisUrunu.Urunler.urunFiyat
                    });
                }
            }
            #endregion EKSTRALILAR
            #region KAMPANYALILAR
            if (kampanyalilarinCheckboxDegerleriDizi != null)
            {
                foreach (var c in kampanyalilarinCheckboxDegerleriDizi.ToList())
                {
                    foreach (var siparisUrunu in kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == c.oturumKampanyaId ).GroupBy(x=>x.oturumKampanyaId))
                    {
                        int urunAdedi = 1;
                        string urunAd = kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Kampanyalar.kampanyaAd;
                        double ekstraToplami = 0;
                        foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x =>x.SiparisUrunleri.oturumKampanyaId == siparisUrunu.Key))
                        {
                            ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                        }
                        double ozellikToplam = kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
                        int urunAdUzunlugu = urunAd.Length;
                        if (urunAdUzunlugu > 24)
                        {
                            urunAd = urunAd.Remove(24);
                        }
                        urunAdUzunlugu = urunAd.Length;
                        if (ekstraToplami > 0 || ozellikToplam > 0)
                        {
                            for (int i = 1; i <= (24 - urunAdUzunlugu); i++)
                            {
                                urunAd += " ";
                            }
                            urunAd += " (Ek.)";
                        }
                        double urunOzellikFiyatToplam = ozellikToplam + kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Kampanyalar.kampanyaFiyat;

                        yazdirilacakFis.Add(new FisItem
                        {
                            urunAdedi = urunAdedi,
                            ekstraToplami= ekstraToplami,
                            urunAd = urunAd,
                            urunOzellikFiyatToplam = urunOzellikFiyatToplam
                        });
                    }
                }
            }
            #endregion KAMPANYALILAR
            bool yazdirmaDurumu = yaziciController.FisYazdir(yazdirilacakFis);
            return Json(yazdirmaDurumu, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OdemeAl(Odemeler odeme, int subeId)
        {
            Odemeler _odeme = new Odemeler
            {
                odemeMiktar = odeme.odemeMiktar,
                odemeTurId = odeme.odemeTurId,
                oturumId = odeme.oturumId,
                odemeTarihi = DateTime.Now,
                subeId=subeId
            };

            kasaModel.Odemeler.Add(_odeme);
            kasaModel.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OdemeSil(int odemeId, int subeId)
        {
            int oturumId = kasaModel.Odemeler.Where(x => x.odemeId == odemeId).FirstOrDefault().oturumId;
            kasaModel.Odemeler.RemoveRange(kasaModel.Odemeler.Where(d => d.odemeId == odemeId));

            //Veritabanını Kaydet
            kasaModel.SaveChanges();

            return Json(oturumId, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult EkstraliUrunIptal(EkstraliCheckbox ekstralilarinCheckboxDegerleriDizi, int subeId)
        {
            if (ekstralilarinCheckboxDegerleriDizi != null)
            {
                var sipoarisUrunu = kasaModel.SiparisUrunleri.Where(x => x.siparisUrunlerId == ekstralilarinCheckboxDegerleriDizi.siparisUrunlerId).FirstOrDefault();
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (sipoarisUrunu.siparisUrunlerId != null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == sipoarisUrunu.siparisUrunlerId).ToList())
                    {
                        //Sipariş Ürününün Ekstralarını Siler
                        kasaModel.SiparisUrunleriEkstralari.RemoveRange(kasaModel.SiparisUrunleriEkstralari.Where(d => d.siparisUrunlerId == siparisUrunEkstrasi.siparisUrunlerId));
                    }
                    //Sipariş Ürününü Siler
                    kasaModel.SiparisUrunleri.RemoveRange(kasaModel.SiparisUrunleri.Where(d => d.siparisUrunlerId == ekstralilarinCheckboxDegerleriDizi.siparisUrunlerId));

                    kasaModel.SaveChanges();
                }
                
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult KampanyaIptal(KampanyaliCheckbox kampanyalilarinCheckboxDegerleriDizi, int subeId)
        {
            if (kampanyalilarinCheckboxDegerleriDizi != null)
            {
                foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == kampanyalilarinCheckboxDegerleriDizi.oturumKampanyaId))
                {
                    foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId))
                    {
                        //Sipariş Ürününün Ekstralarını Siler
                        kasaModel.SiparisUrunleriEkstralari.RemoveRange(kasaModel.SiparisUrunleriEkstralari.Where(d => d.siparisUrunlerId == siparisUrunEkstrasi.siparisUrunlerId)); 
                    }
                    //Sipariş Ürününü Siler
                    kasaModel.SiparisUrunleri.RemoveRange(kasaModel.SiparisUrunleri.Where(d => d.oturumKampanyaId == kampanyalilarinCheckboxDegerleriDizi.oturumKampanyaId));
                }
                //Oturum Kampanyalarını Siler
                kasaModel.OturumKampanyalari.RemoveRange(kasaModel.OturumKampanyalari.Where(d => d.oturumKampanyaId == kampanyalilarinCheckboxDegerleriDizi.oturumKampanyaId));
                kasaModel.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult EkstrasizUrunIptal(Ekstrasiz ekstrasizlar, int subeId)
        {
            if (ekstrasizlar != null)
            {
                    foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.urunOzellikId == ekstrasizlar.urunOzellikId && x.odendi == false && x.Siparisler.oturumId == ekstrasizlar.oturumId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId && x.odendi == false).Count() == 0).Take(ekstrasizlar.siparisAdedi).ToList())
                    {
                        //Gönderilen Sipariş adedi kadar siparişÜrününü siler
                        foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId))
                        {
                            //Sipariş Ürününün Ekstralarını Siler
                            kasaModel.SiparisUrunleriEkstralari.RemoveRange(kasaModel.SiparisUrunleriEkstralari.Where(d => d.siparisUrunlerId == siparisUrunEkstrasi.siparisUrunlerId));
                        }
                        //Sipariş Ürününü Siler
                        kasaModel.SiparisUrunleri.RemoveRange(kasaModel.SiparisUrunleri.Where(d => d.siparisUrunlerId == siparisUrunu.siparisUrunlerId));
                }
                kasaModel.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult GarsonCagiranVarMi(string oturumId, int subeId)
        {
            return Json(kasaModel.Oturumlar.Where(x => x.garsonIstendi == true).ToList().Select(x => x.oturumId), JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult OturumuKapat(int oturumId, int subeId)
        {
            if (kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi==false).Count() > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            foreach(var oturum in kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).ToList())
            {
                oturum.oturumKapamaTarihi = DateTime.Now;
                oturum.odendi = true;
                foreach(var masa in kasaModel.Masalar.Where(x => x.masaId == oturum.masaId))
                {
                    masa.durum = false;
                }
            }/*
            kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).ToList().FirstOrDefault().oturumKapamaTarihi = DateTime.Now;
            kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).ToList().FirstOrDefault().odendi = true;
            kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).ToList().FirstOrDefault().Masalar.durum = false;*/
            kasaModel.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult GarsonGonder(string oturumId, int subeId)
        {
            int Id = Convert.ToInt32(oturumId);

            kasaModel.Oturumlar.Where(x => x.oturumId == Id).FirstOrDefault().garsonIstendi = false;
            kasaModel.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult HesapIsteyenVarMi(string oturumId, int subeId)
        {
            return Json(kasaModel.Oturumlar.Where(x => x.hesapIstendi == true).ToList().Select(x => x.oturumId), JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult AdisyonFisiYazdir(int oturumId, int subeId)
        {
            //kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).ToList().FirstOrDefault().hesapIstendi = false;
            //kasaModel.SaveChanges();
            List<HesapItem> hesapItemList = new List<HesapItem>();
            //EKSTRASIZLAR - KAMPANYASIZ
            foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.SiparisUrunleriEkstralari.Count() == 0 && x.oturumKampanyaId == null).GroupBy(x => x.urunOzellikId))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = kasaModel.SiparisUrunleri.Where(x => x.urunOzellikId == siparisUrunu.Key && x.Siparisler.oturumId == oturumId).FirstOrDefault().Siparisler.Personeller.personelAd,
                    urunAdedi = siparisUrunu.Count(),
                    urunAd = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunAd + " - " + kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikAd,
                    ekstraToplami = 0,
                    urunOzellikFiyatToplam = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunFiyat + kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikFiyat
                };
                hesapItemList.Add(hesapItem_);
            }
            //EKSTRALILAR - KAMPANYASIZ
            foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.SiparisUrunleriEkstralari.Count() > 0 && x.oturumKampanyaId == null))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = siparisUrunu.Siparisler.Personeller.personelAd,
                    urunAdedi = 1,
                    urunAd = siparisUrunu.Urunler.urunAd + " - " + siparisUrunu.UrunOzellikler.urunOzellikAd
                };
                foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId))
                {
                    hesapItem_.ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                }
                double ozellikToplam = siparisUrunu.UrunOzellikler.urunOzellikFiyat;
                int urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (urunAdUzunlugu > 24)
                {
                    hesapItem_.urunAd.Remove(24);
                }
                urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (hesapItem_.ekstraToplami > 0 || ozellikToplam > 0)
                {
                    for (int i = 1; i <= (24 - urunAdUzunlugu); i++)
                    {
                        hesapItem_.urunAd += " ";
                    }
                    hesapItem_.urunAd += " (Ek.)";
                }
                hesapItem_.urunOzellikFiyatToplam = siparisUrunu.Urunler.urunFiyat + ozellikToplam;
                hesapItemList.Add(hesapItem_);
            }
            //KAMPANYALILAR
            foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.oturumKampanyaId != null).GroupBy(x => x.oturumKampanyaId))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Siparisler.Personeller.personelAd,
                    urunAdedi = 1,
                    urunAd = kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Kampanyalar.kampanyaAd,
                    ekstraToplami = 0
                };
                foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.oturumId == oturumId && x.SiparisUrunleri.oturumKampanyaId == siparisUrunu.Key))
                {
                    hesapItem_.ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                }
                double ozellikToplam = kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
                int urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (urunAdUzunlugu > 24)
                {
                    hesapItem_.urunAd = hesapItem_.urunAd.Remove(24);
                }
                urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (hesapItem_.ekstraToplami > 0 || ozellikToplam > 0)
                {
                    for (int i = 1; i <= (24 - urunAdUzunlugu); i++)
                    {
                        hesapItem_.urunAd += " ";
                    }
                    hesapItem_.urunAd += " (Ek.)";
                }
                hesapItem_.urunOzellikFiyatToplam = kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat) + kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().OturumKampanyalari.Kampanyalar.kampanyaFiyat;
                hesapItemList.Add(hesapItem_);
            }
            List<String> personelListesi = new List<String>();
            foreach (var personel in hesapItemList.GroupBy(x => x.personelAd).ToList())
            {
                personelListesi.Add(personel.Key);
            }
            string masaAd = kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).FirstOrDefault().Masalar.masaAd;
            //Veritabanını Kaydet

            bool yazdirmaDurumu = yaziciController.HesapYazdir(hesapItemList, personelListesi, masaAd);
            return Json(yazdirmaDurumu, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult HesapGonder(int oturumId, int subeId)
        {
            //oturumId = Convert.ToInt32(oturumId);
            kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).ToList().FirstOrDefault().hesapIstendi = false;
            kasaModel.SaveChanges();
            List<HesapItem> hesapItemList = new List<HesapItem>();
            //EKSTRASIZLAR - KAMPANYASIZ
            foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi == false && x.SiparisUrunleriEkstralari.Count() == 0 && x.oturumKampanyaId == null).GroupBy(x => x.urunOzellikId))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = kasaModel.SiparisUrunleri.Where(x => x.urunOzellikId == siparisUrunu.Key && x.Siparisler.oturumId == oturumId).FirstOrDefault().Siparisler.Personeller.personelAd,
                    urunAdedi = siparisUrunu.Count(),
                    urunAd = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunAd + " - " + kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikAd,
                    ekstraToplami = 0,
                    urunOzellikFiyatToplam = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunFiyat + kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikFiyat
                };
                hesapItemList.Add(hesapItem_);
            }
            //EKSTRALILAR - KAMPANYASIZ
            foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi == false && x.SiparisUrunleriEkstralari.Count() > 0 && x.oturumKampanyaId == null))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = siparisUrunu.Siparisler.Personeller.personelAd,
                    urunAdedi = 1,
                    urunAd = siparisUrunu.Urunler.urunAd + " - " + siparisUrunu.UrunOzellikler.urunOzellikAd
                };
                foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId==siparisUrunu.siparisUrunlerId))
                {
                    hesapItem_.ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                }
                double ozellikToplam = siparisUrunu.UrunOzellikler.urunOzellikFiyat;
                int urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (urunAdUzunlugu > 24)
                {
                    hesapItem_.urunAd.Remove(24);
                }
                urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (hesapItem_.ekstraToplami > 0 || ozellikToplam > 0)
                {
                    for (int i = 1; i <= (24 - urunAdUzunlugu); i++){
                        hesapItem_.urunAd += " ";
                    }
                    hesapItem_.urunAd += " (Ek.)";
                }
                hesapItem_.urunOzellikFiyatToplam = siparisUrunu.Urunler.urunFiyat + ozellikToplam;
                hesapItemList.Add(hesapItem_);
            }
            //KAMPANYALILAR
            foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi == false && x.oturumKampanyaId != null).GroupBy(x => x.oturumKampanyaId))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Siparisler.Personeller.personelAd,
                    urunAdedi = 1,
                    urunAd = kasaModel.OturumKampanyalari.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Kampanyalar.kampanyaAd,
                    ekstraToplami = 0
                };
                foreach (var siparisUrunEkstrasi in kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.oturumId == oturumId && x.odendi == false && x.SiparisUrunleri.oturumKampanyaId == siparisUrunu.Key))
                {
                    hesapItem_.ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                }
                double ozellikToplam = kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
                int urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (urunAdUzunlugu > 24)
                {
                    hesapItem_.urunAd= hesapItem_.urunAd.Remove(24);
                }
                urunAdUzunlugu = hesapItem_.urunAd.Length;
                if (hesapItem_.ekstraToplami > 0 || ozellikToplam > 0)
                {
                    for (int i = 1; i <= (24 - urunAdUzunlugu); i++)
                    {
                        hesapItem_.urunAd += " ";
                    }
                    hesapItem_.urunAd += " (Ek.)";
                }
                hesapItem_.urunOzellikFiyatToplam = kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat) + kasaModel.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().OturumKampanyalari.Kampanyalar.kampanyaFiyat;
                hesapItemList.Add(hesapItem_);
            }
            List<String> personelListesi = new List<String>();
            foreach (var personel in hesapItemList.GroupBy(x => x.personelAd).ToList())
            {
                personelListesi.Add(personel.Key);
            }
            string masaAd = kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).FirstOrDefault().Masalar.masaAd;
            

            bool yazdirmaDurumu = yaziciController.HesapYazdir(hesapItemList,personelListesi,masaAd);
            return Json(yazdirmaDurumu, JsonRequestBehavior.AllowGet);
        }

        [KasaAsyncFunctionRoleControl]
        [HttpPost]
        public JsonResult GunlukCiroGetir(int subeId)
        {
            DateTime sabahTarihSaat = DateTime.Now.Date.AddHours(3);//KASADA GÖZÜKEN O GÜN SABAH SAAT 6'DAN
            DateTime geceTarihSaat = DateTime.Now.Date.AddDays(1).AddHours(3);//KASADA GÖZÜKEN O GÜN GECE 24'TEN SONRAKİ 3 SAATE KADAR
            if (DateTime.Now.TimeOfDay <= new TimeSpan(3, 0, 0) && DateTime.Now.TimeOfDay >= new TimeSpan(0, 0, 0))
            {
                sabahTarihSaat = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));
                geceTarihSaat = DateTime.Now;
            }
            List<Ciro> ciroList = new List<Ciro>();
            try
            {
                foreach (var odemeTuru in kasaModel.OdemeTurleri.Where(x => x.subeId == subeId))
                {
                    ciroList.Add(new Ciro {
                        odemeTurAd = odemeTuru.odemeTurAd,
                        miktar = odemeTuru.Odemeler.Where(x => x.odemeTarihi >= sabahTarihSaat && x.odemeTarihi <= geceTarihSaat).Sum(x => x.odemeMiktar),
                        paraBirimiAd = odemeTuru.ParaBirimleri.paraBirimiAd
                    });
                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
            
            
            return Json(ciroList, JsonRequestBehavior.AllowGet);
        }

        #endregion 
        //FONKSİYONLAR

        public bool TamamiOdendiMiAcaba(int oturumId, int subeId)
        {
            bool siparisUrunleriOdendi = true;
            bool siparisUrunEkstralariOdendi = true;
            if (kasaModel.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi == false).Count() > 0)
            {
                siparisUrunleriOdendi = false;
            }
            if (kasaModel.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.oturumId == oturumId && x.odendi == false).Count() > 0)
            {
                siparisUrunEkstralariOdendi = false;
            }
            if(siparisUrunleriOdendi && siparisUrunEkstralariOdendi)
            {
                AdisyonFisiYazdir(oturumId,subeId);
                try
                {
                    foreach (var oturum in kasaModel.Oturumlar.Where(x => x.oturumId == oturumId).ToList())
                    {
                        oturum.odendi = true;

                        foreach(var masa in kasaModel.Masalar.Where(x=>x.masaId == oturum.masaId).ToList())
                        {
                            masa.durum = false;
                        }
                        oturum.Masalar.durum = false;
                        oturum.oturumKapamaTarihi = DateTime.Now;
                    }
                    kasaModel.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                
                
            }
            return (siparisUrunleriOdendi && siparisUrunEkstralariOdendi);
        }

        //CLASSLAR
        #region // CLASSLAR

        public class Ciro
        {
            public string odemeTurAd { get; set; }
            public double miktar { get; set; }
            public string paraBirimiAd { get; set; }

        }
        public class HesapItem
        {
            public string urunAd { get; set; }
            public int urunAdedi { get; set; }
            public double urunOzellikFiyatToplam { get; set; }
            public double ekstraToplami { get; set; }
            public string personelAd { get; set; }
            /*public hesapItem()
            {
                personeller = new List<string>();
            }*/
        }
        public class FisItem
        {
            public string urunAd { get; set; }
            public int urunAdedi { get; set; }
            public double urunOzellikFiyatToplam { get; set; }
            public double ekstraToplami { get; set; }
        }
        public class EkstraliCheckbox
        {
            public int siparisUrunlerId { get; set; }

        }
        public class KampanyaliCheckbox
        {
            public int oturumKampanyaId { get; set; }

        }
        public class Ekstrasiz
        {
            public int urunOzellikId { get; set; }
            public int siparisAdedi { get; set; }
            public int oturumId { get; set; }
        }
        public class PersonelPerformans
        {
            public string personelAd { get; set; }
            public int personelId { get; set; }
            public double personelMiktar { get; set; }
            public double personelYuzde { get; set; }
        }
        public class OdemeYontemiDokum
        {
            public string yontemAd { get; set; }
            public double yontemYuzde { get; set; }
            public double yontemMiktar { get; set; }
        }

        #endregion
    }
}