using iRestaurant.Attributes;
using iRestaurant.Models;
using Rework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace iRestaurant.Controllers
{
    public class HomeController : Controller
    {
        #region // CLASSLAR
        public async Task<bool> Email(EmailFormModel model)
        {
            try
            {
                var body = "<p>{0} ({1})</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(model.toEmail));  // replace with valid value 
                message.From = new MailAddress("fmsforum@gmail.com");  // replace with valid value
                message.Subject = model.subject;
                message.Body = string.Format(body, model.fromName, model.fromEmail, model.message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "fmsforum@gmail.com",  // replace with valid value
                        Password = "eyzsdhvzinekxdfx"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 465;
                    smtp.Host = "smtp.gmail.com"; // replace with valid value
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
        }
        public static string paparaApiKey = "AJHSDFGQUWYGDOAHSDBASDBKUYQWGDKA";
        public static string merchantSecretKey = "SDFDFGHFGHERTSDFSDFSEDRFWESFD";
        iRestaurantDataBaseClass iRestaurantModel = new iRestaurantDataBaseClass();
        public class LoginModel
        {
            public string sirketEmail { get; set; }
            public string sirketParola { get; set; }
            public string hatirla { get; set; }
            public string returnUrl { get; set; }
        }
        public class RecoverModel
        {
            public string sirketEmail { get; set; }
            public string sirketParola { get; set; }
            public string sirketParolaTekrar { get; set; }
            public string aktiflestirmeKodu { get; set; }
        }
        public class SepettekiHizmet
        {
            public int hizmetId { get; set; }
            public string hizmetAd { get; set; }
            public double hizmetAylikUcret { get; set; }
            public int paraBirimiId { get; set; }
            public short hizmetTuru { get; set; }
            public int adet { get; set; }
            public bool yillikMi { get; set; }
            public int? hizmetKampanyaId { get; set; }
        }
        public class PaparaRedirect
        {
            public string paymentId { get; set; }
            public string referenceId { get; set; }
            public int status { get; set; }
            public double amount { get; set; }
        }
        public class Payment
        {
            public double amount { get; set; }
            public string referenceId { get; set; }
            public string orderDescription { get; set; }
            public string notificationUrl { get; set; }
            public string redirectUrl { get; set; }
        }
        public class PaymentData
        {
            public string merchantId { get; set; }
            public string userId { get; set; }
            public int paymentMethod { get; set; }//0-Kullanıcı işlemi var olan Papara bakiyesi ile tamamladı //1-Kullanıcı işlemi hesabına daha önce tanımladığı banka/kredi kartı ile tamamladı //2-Kullanıcı işlemi mobil ödeme ile tamamladı 
            public string paymentMethodDescription { get; set; }
            public string referenceId { get; set; }
            public string orderDescription { get; set; }
            public int status { get; set; }//0-Beklemede, ödeme henüz yapılmadı //1-Ödeme yapıldı, işlem tamamlandı //2-İşlem üye işyeri tarafından iade edildi. 
            public string statusDescription { get; set; }
            public double amount { get; set; }
            public string currency { get; set; }
            public string notificationUrl { get; set; }
            public bool notificationDone { get; set; }
            public string redirectUrl { get; set; }
            public string merchantSecretKey { get; set; }
            public string paymentUrl { get; set; }
            public string returningRedirectUrl { get; set; }
            public string id { get; set; }
            public string createdAt { get; set; }
        }
        public class PaymentError
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        public class PaymentGet
        {
            public PaymentData data { get; set; }
            public bool succeeded { get; set; }
            public PaymentError error { get; set; }
        }
        public static PaymentGet CreatePaymentObject(Payment payment)
        {
            PaymentGet returningPayment = new PaymentGet();
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://merchantapi-test-master.papara.com/payments");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("ApiKey", paparaApiKey);
                JavaScriptSerializer js = new JavaScriptSerializer();
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {


                    string json = js.Serialize(payment);

                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    //Now you have your response.
                    //or false depending on information in the response
                    returningPayment = js.Deserialize<PaymentGet>(responseText);
                    return returningPayment;
                }
            }
            catch(Exception e)
            {
                returningPayment.data=new PaymentData{
                    amount=0,
                    createdAt="",
                    currency="",
                    id="",
                    merchantId="",
                    merchantSecretKey="",
                    notificationDone=false,
                    notificationUrl="",
                    orderDescription="",
                    paymentMethod=0,
                    paymentMethodDescription="",
                    paymentUrl="",
                    redirectUrl="",
                    referenceId="",
                    returningRedirectUrl="",
                    status=0,
                    statusDescription="",
                    userId=""
                };
                returningPayment.succeeded = false;
                returningPayment.error = new PaymentError
                { message = e.Message,
                    code = 401
                };
                return returningPayment;
            }
        }
        public static PaymentGet GetPaymentObject(string id)
        {
            PaymentGet paymentGet = new PaymentGet();
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://merchantapi-test-master.papara.com/payments?id=" + id);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("ApiKey", paparaApiKey);
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                JavaScriptSerializer js = new JavaScriptSerializer();
                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var responseText = reader.ReadToEnd();
                    paymentGet = js.Deserialize<PaymentGet>(responseText);
                    return paymentGet;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                paymentGet.data = new PaymentData{id = id};
                paymentGet.error = new PaymentError{code=0,message=e.Message};
                paymentGet.succeeded = false;
                return paymentGet;
            }
            
        }
        public static bool RefundPayment(string id)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://merchantapi-test-master.papara.com/payments?id=" + id);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("ApiKey", paparaApiKey);
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                JavaScriptSerializer js = new JavaScriptSerializer();
                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var responseText = reader.ReadToEnd();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion

        #region // SAYFA ÇAĞRIMLARI

        public ActionResult About()
        {



            return View();
        }
        public ActionResult Career()
        {



            return View();
        }
        public ActionResult CareerDetail(int careerId)
        {



            return View();
        }
        public ActionResult Cart()
        {
            List<SepettekiHizmet> sepetList = new List<SepettekiHizmet>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (Session["sepet"] != null)
                sepetList = (List<SepettekiHizmet>)Session["sepet"];

            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                HizmetKampanyaHizmetleri = iRestaurantModel.HizmetKampanyaHizmetleri.ToList(),
                HizmetKampanyalari = iRestaurantModel.HizmetKampanyalari.ToList(),
                Hizmetler = iRestaurantModel.Hizmetler.ToList(),
                HizmetLisanslari = iRestaurantModel.HizmetLisanslari.ToList(),
                ParaBirimleri = iRestaurantModel.ParaBirimleri.ToList(),
                SepettekiHizmetler = sepetList
            };
            return View(vm);
        }
        [HomeSirketRoleControl]
        public ActionResult Checkout()
        {
            try
            {
                if (Session["sepet"] == null)
                {
                    return RedirectToAction("Cart", "Home");
                }
                else
                {
                    List<SepettekiHizmet> sepettekiHizmetler = (List<SepettekiHizmet>)Session["sepet"];
                    if (sepettekiHizmetler.Count == 0)
                    {
                        return RedirectToAction("Cart", "Home");
                    }
                    
                }
                int sirketId = (int)Session["sirketId"];
                ViewModelDemoVM vm = new ViewModelDemoVM
                {
                    HizmetKampanyaHizmetleri = iRestaurantModel.HizmetKampanyaHizmetleri.ToList(),
                    Hizmetler = iRestaurantModel.Hizmetler.ToList(),
                    HizmetKampanyalari = iRestaurantModel.HizmetKampanyalari.ToList(),
                    HizmetLisanslari = iRestaurantModel.HizmetLisanslari.ToList(),
                    ParaBirimleri = iRestaurantModel.ParaBirimleri.ToList(),
                    SepettekiHizmetler = (List<SepettekiHizmet>)Session["sepet"],
                    Ulkeler = iRestaurantModel.Ulkeler.ToList(),
                    Ilceler = iRestaurantModel.Ilceler.ToList(),
                    Iller = iRestaurantModel.Iller.ToList(),
                    Sirketler = iRestaurantModel.Sirketler.Where(x=>x.sirketId== sirketId).ToList()
                };

                return View(vm);
            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
                ViewBag.message = e.Message;
                return View("Error404");
            }
        }
        public ActionResult CheckoutFinished()
        {
            try
            {
                if (Session["sepet"] == null)
                {
                    return RedirectToAction("Cart", "Home");
                }
                else
                {
                    List<SepettekiHizmet> sepettekiHizmetler = (List<SepettekiHizmet>)Session["sepet"];
                    if (sepettekiHizmetler.Count == 0)
                    {
                        return RedirectToAction("Cart", "Home");
                    }

                }
                int sirketId = (int)Session["sirketId"];
                ViewModelDemoVM vm = new ViewModelDemoVM
                {
                    HizmetKampanyaHizmetleri = iRestaurantModel.HizmetKampanyaHizmetleri.ToList(),
                    Hizmetler = iRestaurantModel.Hizmetler.ToList(),
                    HizmetKampanyalari = iRestaurantModel.HizmetKampanyalari.ToList(),
                    HizmetLisanslari = iRestaurantModel.HizmetLisanslari.ToList(),
                    ParaBirimleri = iRestaurantModel.ParaBirimleri.ToList(),
                    SepettekiHizmetler = (List<SepettekiHizmet>)Session["sepet"],
                    Ulkeler = iRestaurantModel.Ulkeler.ToList(),
                    Ilceler = iRestaurantModel.Ilceler.ToList(),
                    Iller = iRestaurantModel.Iller.ToList(),
                    Sirketler = iRestaurantModel.Sirketler.Where(x => x.sirketId == sirketId).ToList()
                };

                return View(vm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = e.Message;
                return View("Error404");
            }
        }
        [HomeSirketRoleControl]
        public ActionResult Buy(int ilceId, string sirketAdres)
        {
            try
            {
                if (Session["sepet"] == null)
                {
                    return RedirectToAction("Cart", "Home");
                }
                else
                {
                    Payment payment = new Payment();
                    List<SepettekiHizmet> sepettekiHizmetler = (List<SepettekiHizmet>)Session["sepet"];
                    Faturalar yeniFatura = new Faturalar();
                    try
                    {//FATURA OLUŞTURULDU
                        
                        if (sepettekiHizmetler.Count == 0)
                        {
                            return RedirectToAction("Cart", "Home");
                        }
                        int sirketId = (int)Session["sirketId"];
                        double aratop = (sepettekiHizmetler.Where(x => x.yillikMi == true && x.hizmetKampanyaId == null).Sum(x => x.hizmetAylikUcret * x.adet) * 12) +
                                         sepettekiHizmetler.Where(x => x.yillikMi == false && x.hizmetKampanyaId == null).Sum(x => x.hizmetAylikUcret * x.adet);

                        foreach (var hizmetKampanya in sepettekiHizmetler.Where(x => x.hizmetKampanyaId != null).GroupBy(x => x.hizmetKampanyaId))
                        {
                            aratop += iRestaurantModel.HizmetKampanyalari.Where(x => x.hizmetKampanyaId == hizmetKampanya.Key.Value).First().hizmetKampanyaFiyat * sepettekiHizmetler.Where(x => x.hizmetKampanyaId == hizmetKampanya.Key.Value).First().adet;
                        }
                        if(iRestaurantModel.Faturalar.Where(x=>x.odendiMi==false && x.sirketId == sirketId).Count() == 0)
                        {
                            yeniFatura = iRestaurantModel.Faturalar.Add(new Faturalar
                            {
                                faturaTarih = DateTime.Now,
                                odendiMi = false,
                                sirketId = sirketId
                            });
                            
                        }
                        else
                        {
                            yeniFatura = iRestaurantModel.Faturalar.Where(x => x.odendiMi == false && x.sirketId == sirketId).First();
                        }

                        iRestaurantModel.SaveChanges();
                        payment = new Payment()
                        {
                            amount = (aratop * 1.18),
                            notificationUrl = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/Home/BuyNotification",
                            orderDescription = "Satın alınan şeylerin listesi",
                            redirectUrl = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/Home/BuyRedirect",
                            referenceId = "faturaId_" + yeniFatura.faturaId.ToString()
                        };
                    }
                    catch(Exception e)
                    {//FATURA OLUŞTURULAMADI
                        ViewBag.message = e.Message;
                        return View("Error404");
                    }//FATURA OLUŞTURULDU
                    try
                    {//FATURA OLUŞTURULDUĞU VARSAYILDIĞINDA ÖDEME OLUŞTURULABİLECEK Mİ
                        PaymentGet paymentResponse = CreatePaymentObject(payment);
                        if (paymentResponse.succeeded == true)
                        {//FATURA OLUŞTURULDU VE ÖDEME İSTEĞİ OLUŞTURULDU, SEPETTEKİ HİZMETLER FATURAYA AİT FATURAHİZMETLERİNE AKTARILABİLİR VE ÖDEMEYE YÖNLENDİRİLEBİLİR
                            foreach (var sepettekiHizmet in sepettekiHizmetler)
                            {
                                FaturadakiHizmetler faturadakiHizmet = new FaturadakiHizmetler
                                {
                                    faturaId = yeniFatura.faturaId,
                                    hizmetId = sepettekiHizmet.hizmetId,
                                    adet = sepettekiHizmet.adet,
                                    yillikMi = sepettekiHizmet.yillikMi,
                                    hizmetKampanyaId = sepettekiHizmet.hizmetKampanyaId,
                                    paraBirimiId = sepettekiHizmet.paraBirimiId

                                };
                                if(iRestaurantModel.FaturadakiHizmetler.Where(x=>
                                        x.yillikMi==faturadakiHizmet.yillikMi && 
                                        x.faturaId==faturadakiHizmet.faturaId && 
                                        x.adet == faturadakiHizmet.adet && 
                                        x.hizmetId == faturadakiHizmet.hizmetId && 
                                        x.paraBirimiId == faturadakiHizmet.paraBirimiId && 
                                        x.hizmetKampanyaId.Value == faturadakiHizmet.hizmetKampanyaId.Value
                                    ).Count() == 0)
                                { 
                                    iRestaurantModel.FaturadakiHizmetler.Add(faturadakiHizmet);
                                }
                            }
                            iRestaurantModel.SaveChanges();
                            return Redirect(paymentResponse.data.paymentUrl);
                        }
                        else
                        {//FATURA OLUŞTURULDU ANCAK ÖDEME İSTEĞİ OLUŞTURULAMADI
                            iRestaurantModel.Faturalar.Remove(yeniFatura);
                            iRestaurantModel.SaveChanges();
                            ViewBag.message ="Papara'da fatura oluştururken bir hata oluştu | " + paymentResponse.error.message;
                            return View("Error404");
                        }
                    }
                    catch (Exception e)
                    {//FATURA OLUŞTURULDUĞU VARSAYILDI ANCA ÖDEME OLUŞTURULAMADI
                        ViewBag.message = e.Message;
                        //return RedirectToAction("Lisanslar","SirketYonetim",new PaymentError {code=0, message=e.Message});
                        return View("Error404");
                    }

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = e.Message;
                return View("Error404");
            }
        }
        [HttpGet]
        public ActionResult BuyRedirect(string paymentId, int status, double amount)
        {
            List<SepettekiHizmet> sepetList = new List<SepettekiHizmet>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            
            PaymentGet payment = GetPaymentObject(paymentId);
            
                if (payment.succeeded == true)
                {
                    int faturaId = 16;//Convert.ToInt32(payment.data.referenceId.Split('_')[1]);

                    double aratop = 0;
                    try
                    {
                        foreach (var faturadakiHizmet in iRestaurantModel.FaturadakiHizmetler.Where(x => x.yillikMi == true && x.hizmetKampanyaId == null))
                        {
                            aratop += Math.Round(faturadakiHizmet.Hizmetler.hizmetAylikUcret, 2) * faturadakiHizmet.adet * 12;
                        }
                        foreach (var faturadakiHizmet in iRestaurantModel.FaturadakiHizmetler.Where(x => x.yillikMi == false && x.hizmetKampanyaId == null))
                        {
                            aratop += Math.Round(faturadakiHizmet.Hizmetler.hizmetAylikUcret, 2) * faturadakiHizmet.adet;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        aratop += 0;
                    }
                    try
                    {
                        foreach (var hizmetKampanyaId in iRestaurantModel.FaturadakiHizmetler.Where(x => x.yillikMi == true && x.faturaId == faturaId && x.hizmetKampanyaId != null).GroupBy(x => x.hizmetKampanyaId))
                        {
                            HizmetKampanyalari hizmetKampanya = iRestaurantModel.HizmetKampanyalari.Where(x => x.hizmetKampanyaId == hizmetKampanyaId.Key.Value).First();
                            FaturadakiHizmetler faturadakiHizmet = iRestaurantModel.FaturadakiHizmetler.Where(x => x.faturaId == faturaId && x.hizmetKampanyaId == hizmetKampanyaId.Key.Value).First();
                            if (hizmetKampanya != null && faturadakiHizmet != null)
                                aratop += hizmetKampanya.hizmetKampanyaFiyat * faturadakiHizmet.adet;
                        }
                        foreach (var hizmetKampanyaId in iRestaurantModel.FaturadakiHizmetler.Where(x => x.yillikMi == false && x.faturaId == faturaId && x.hizmetKampanyaId != null).GroupBy(x => x.hizmetKampanyaId))
                        {
                            HizmetKampanyalari hizmetKampanya = iRestaurantModel.HizmetKampanyalari.Where(x => x.hizmetKampanyaId == hizmetKampanyaId.Key.Value).First();
                            FaturadakiHizmetler faturadakiHizmet = iRestaurantModel.FaturadakiHizmetler.Where(x => x.faturaId == faturaId && x.hizmetKampanyaId == hizmetKampanyaId.Key.Value).First();
                            if (hizmetKampanya != null && faturadakiHizmet != null)
                                aratop += hizmetKampanya.hizmetKampanyaFiyat * faturadakiHizmet.adet;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        aratop += 0;
                    }

                    if (payment.data.amount == Math.Round(aratop * 1.18, 2))//Sitemiz üzerinde çıkan tutarla eş ödeme yapılmıştır. Fatura kontrolüne geçebiliriz.
                    {
                        Faturalar fatura = iRestaurantModel.Faturalar.Where(x => x.faturaId == faturaId).First();
                        if (fatura == null)//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunmamakta
                        {
                        }//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunmamakta
                        else//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunuyor.
                        {
                            if (fatura.odendiMi == true)//Gelen ödeme bildiriminde bahsi geçen fatura zaten ödenmiştir. İçinde 
                            {
                                /*
                                foreach(var faturadakiHizmet in iRestaurantModel.FaturadakiHizmetler.Where(x => x.faturaId == faturaId).ToList())
                                {
                                    DateTime bitisTarihi = new DateTime();
                                    if (faturadakiHizmet.yillikMi)
                                    {
                                        bitisTarihi = DateTime.Now.AddYears(1);
                                    }
                                    else
                                    {
                                        bitisTarihi = DateTime.Now.AddMonths(1);
                                    }
                                    fatura.HizmetLisanslari.Add(new HizmetLisanslari
                                    {
                                        faturaId = fatura.faturaId,
                                        hizmetId = faturadakiHizmet.hizmetId,
                                        subeId = null,
                                        hizmetLisansBaslangicTarihi = DateTime.Now,
                                        hizmetLisansBitisTarihi = bitisTarihi
                                    });
                                }
                                    
                                */
                                    
                                iRestaurantModel.SaveChanges();
                                Session["sepet"] = null;
                                ViewBag.message = "Faturanız ödenmiştir. Yönetim panelinden satın almış olduğunuz lisanslarla ilgili işlem yapabilirsiniz.";
                            }//Gelen ödeme bildiriminde bahsi geçen fatura zaten ödenmiştir
                            else//Gelen ödeme bildiriminde bahsi geçen fatura henüz ödenmemiştir. Veritabanında ödendi olarak gösterilmeli ve lisanslar tayin edilmeli
                            {
                                fatura.odendiMi = true;
                                if (fatura.FaturadakiHizmetler.Count > 0)//Eğer faturaya hizmetler tayin edilmemişse
                                {
                                    foreach (var faturadakiHizmet in fatura.FaturadakiHizmetler)
                                    {
                                        DateTime bitisTarihi = new DateTime();
                                        if (faturadakiHizmet.yillikMi)
                                        {
                                            bitisTarihi = DateTime.Now.AddYears(1);
                                        }
                                        else
                                        {
                                            bitisTarihi = DateTime.Now.AddMonths(1);
                                        }
                                        fatura.HizmetLisanslari.Add(new HizmetLisanslari
                                        {
                                            faturaId = fatura.faturaId,
                                            hizmetId = faturadakiHizmet.hizmetId,
                                            subeId = null,
                                            hizmetLisansBaslangicTarihi = DateTime.Now,
                                            hizmetLisansBitisTarihi = bitisTarihi,
                                        });
                                    }
                                    iRestaurantModel.FaturaOdemeleri.Add(new FaturaOdemeleri //Eğer faturanın ödeme miktarı ödemelere eklenmemişse
                                    {
                                        faturaId = faturaId,
                                        faturaOdemeMiktari = Math.Round((aratop * 1.18), 2),
                                        faturaOdemeTarihi = DateTime.Now,
                                        paraBirimiId = fatura.FaturadakiHizmetler.First().paraBirimiId

                                    });
                                }

                                    
                                iRestaurantModel.SaveChanges();
                                Session["sepet"] = null;
                                ViewBag.message = "Faturanız ödenmiştir. Yönetim panelinden satın almış olduğunuz lisanslarla ilgili işlem yapabilirsiniz.";
                            }//Gelen ödeme bildiriminde bahsi geçen fatura henüz ödenmemiştir. Veritabanında ödendi olarak gösterilmeli ve lisanslar tayin edilmeli
                        }//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunuyor.
                    }//Sitemiz üzerinde çıkan tutarla eş ödeme yapılmıştır. Fatura kontrolüne geçebiliriz.
                    else if (payment.data.amount > aratop * 1.18)//Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek. Ve satın almış olduğunuz lisanslar iptal edilecek.
                    {
                        ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek";
                        bool refundStatus = RefundPayment(payment.data.id);
                        if (refundStatus == true)//EĞER GERİ ÖDEME BAŞARILIYSA
                        {
                            iRestaurantModel.HizmetLisanslari.RemoveRange(iRestaurantModel.HizmetLisanslari.Where(x => x.faturaId == faturaId));
                            iRestaurantModel.FaturadakiHizmetler.RemoveRange(iRestaurantModel.FaturadakiHizmetler.Where(x => x.faturaId == faturaId));
                            iRestaurantModel.FaturaOdemeleri.RemoveRange(iRestaurantModel.FaturaOdemeleri.Where(x => x.faturaId == faturaId));
                            ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Sepetiniz iptal edildi. Ödenmiş olan ücret PAPARA'ya geri iade edildi.";
                            Session["sepet"] = null;
                        }//EĞER GERİ ÖDEME BAŞARILIYSA
                        else//EĞER GERİ ÖDEME BAŞARI-SIZSA
                        {
                            ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Sepetiniz iptal edildi. Ve bir hatadan ötürü ödenmiş olan ücret PAPARA'ya geri iade edilemedi. Lütfen bizimle irtibata geçin.";
                            Session["sepet"] = null;
                        }//EĞER GERİ ÖDEME BAŞARI-SIZSA
                    }//Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek. Ve satın almış olduğunuz lisanslar iptal edilecek.
                    else//Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek. Ve satın almış olduğunuz lisanslar iptal edilecek.
                    {
                        ViewBag.message = "Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Ücret geri iade edilecek.";
                        bool refundStatus = RefundPayment(payment.data.id);
                        if (refundStatus == true)//EĞER GERİ ÖDEME BAŞARILIYSA
                        {
                            ViewBag.message = "Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Sepetiniz iptal edildi. Ve ödenmiş olan ücret PAPARA'ya geri iade edildi. Yönetim panelinizden ödenmemiş faturalarınıza göz atıp";
                            Session["sepet"] = null;
                        }//EĞER GERİ ÖDEME BAŞARILIYSA
                        else//EĞER GERİ ÖDEME BAŞARI-SIZSA
                        {
                            ViewBag.message = "Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Sepetiniz iptal edildi. Ve bir hatadan ötürü ödenmiş olan ücret PAPARA'ya geri iade edilemedi. Lütfen bizimle irtibata geçin.";
                            Session["sepet"] = null;
                        }//EĞER GERİ ÖDEME BAŞARI-SIZSA
                    }//Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek. Ve satın almış olduğunuz lisanslar iptal edilecek.

                }
                else//ÖDEME BAŞARISIZ
                {

                    int faturaId = Convert.ToInt32(payment.data.referenceId.Split('_')[1]);
                    try
                    {
                        iRestaurantModel.HizmetLisanslari.RemoveRange(iRestaurantModel.HizmetLisanslari.Where(x => x.faturaId == faturaId));
                        iRestaurantModel.FaturadakiHizmetler.RemoveRange(iRestaurantModel.FaturadakiHizmetler.Where(x => x.faturaId == faturaId));
                        iRestaurantModel.FaturaOdemeleri.RemoveRange(iRestaurantModel.FaturaOdemeleri.Where(x => x.faturaId == faturaId));
                        iRestaurantModel.Faturalar.RemoveRange(iRestaurantModel.Faturalar.Where(x => x.faturaId == faturaId));
                        iRestaurantModel.SaveChanges();
                        ViewBag.message = "Ödeme başarısız. Faturanız ve ona bağlı olan tüm hizmetler veritabanımızdan silindi. Lütfen sepetinizi yeniden oluşturup ödeme yapınız.";
                    }
                    catch
                    {

                    }

                }//ÖDEME BAŞARISIZ
            
            
            return View("CheckoutFinished");
            
        }
        [HttpPost]
        public HttpStatusCode BuyNotification(PaymentData paymentData)//PAPARA, ÖDENMEMİŞ / TAMAMLANMAMIŞ ÖDEMELER İÇİN BİLDİRİMDE BULUNMAZ
        {
            List<SepettekiHizmet> sepettekiHizmetler = new List<SepettekiHizmet>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (Session["sepet"] != null)
                sepettekiHizmetler = (List<SepettekiHizmet>)Session["sepet"];
            int faturaId = Convert.ToInt32(paymentData.referenceId.Split('_')[1]);
            

            double aratop = 0;
            try
            {
                aratop = (Math.Round(iRestaurantModel.FaturadakiHizmetler.Where(x => x.yillikMi == true && x.hizmetKampanyaId == null).Sum(x => x.Hizmetler.hizmetAylikUcret * x.adet),2) * 12) +
                                     iRestaurantModel.FaturadakiHizmetler.Where(x => x.yillikMi == false && x.hizmetKampanyaId == null).Sum(x => x.Hizmetler.hizmetAylikUcret * x.adet);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                aratop += 0;
            }
            try
            {
                foreach (var hizmetKampanya in iRestaurantModel.FaturadakiHizmetler.Where(x => x.hizmetKampanyaId != null).GroupBy(x => x.hizmetKampanyaId))
                {
                    aratop += Math.Round(iRestaurantModel.HizmetKampanyalari.Where(x => x.hizmetKampanyaId == hizmetKampanya.Key.Value).First().hizmetKampanyaFiyat,2) * iRestaurantModel.FaturadakiHizmetler.Where(x => x.faturaId == faturaId && x.hizmetKampanyaId == hizmetKampanya.Key.Value).First().adet;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                aratop += 0;
            }


            if (paymentData.amount == Math.Round(aratop*1.18,2))//Sitemiz üzerinde çıkan tutarla eş ödeme yapılmıştır. Fatura kontrolüne geçebiliriz.
            {
                Faturalar fatura = iRestaurantModel.Faturalar.Where(x => x.faturaId == faturaId).First();
                if (fatura == null)//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunmamakta
                {
                    ViewBag.message = "";
                    bool refundStatus = RefundPayment(paymentData.id);
                    if (refundStatus == true)//EĞER GERİ ÖDEME BAŞARILIYSA
                    {
                        ViewBag.message = "Ödeme yaptığınız fatura sistemimizde bulunmamaktadır. Sepetiniz iptal edildi. Ödenmiş olduğunuz ücret PAPARA'ya geri iade edildi. Ücretin size iade süresi PAPARA'nın günlük durumuna göre değişebilir.";
                        Session["sepet"] = null;
                    }//EĞER GERİ ÖDEME BAŞARILIYSA
                    else//EĞER GERİ ÖDEME BAŞARI-SIZSA
                    {
                        ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Sepetiniz iptal edildi. Ve sistemde bulunan bir hatadan ötürü ödenmiş olan ücret PAPARA'ya geri iade edilemedi. Lütfen bizimle irtibata geçin.";
                        Session["sepet"] = null;
                    }//EĞER GERİ ÖDEME BAŞARI-SIZSA
                    return HttpStatusCode.OK;
                }//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunmamakta
                else//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunuyor.
                {
                    if (fatura.odendiMi == true)//Gelen ödeme bildiriminde bahsi geçen fatura zaten ödenmiştir
                    {
                        ViewBag.message = "Fatura zaten ödenmiştir. Son yaptığınız ödeme geri ödenecektir.";
                        bool refundStatus = RefundPayment(paymentData.id);
                        if (refundStatus == true)//EĞER GERİ ÖDEME BAŞARILIYSA
                        {
                            ViewBag.message = "Fatura zaten ödenmiştir. Son yaptığınız ödeme geri ödenecektir. Sepetiniz iptal edildi. Ödenmiş olan ücret PAPARA'ya geri iade edildi.";
                            Session["sepet"] = null;
                        }//EĞER GERİ ÖDEME BAŞARILIYSA
                        else//EĞER GERİ ÖDEME BAŞARI-SIZSA
                        {
                        
                            ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Sepetiniz iptal edildi. Ve bir hatadan ötürü ödenmiş olan ücret PAPARA'ya geri iade edilemedi. Lütfen bizimle irtibata geçin.";
                            Session["sepet"] = null;
                        }//EĞER GERİ ÖDEME BAŞARI-SIZSA
                        return HttpStatusCode.OK;
                    }//Gelen ödeme bildiriminde bahsi geçen fatura zaten ödenmiştir
                    else//Gelen ödeme bildiriminde bahsi geçen fatura henüz ödenmemiştir. Veritabanında ödendi olarak gösterilmeli ve lisanslar tayin edilmeli
                    {
                        fatura.odendiMi = true;
                        foreach(var faturadakiHizmet in fatura.FaturadakiHizmetler)
                        {
                            DateTime bitisTarihi = new DateTime();
                            if (faturadakiHizmet.yillikMi)
                            {
                                bitisTarihi = DateTime.Now.AddYears(1);
                            }
                            else
                            {
                                bitisTarihi = DateTime.Now.AddMonths(1);
                            }
                            fatura.HizmetLisanslari.Add(new HizmetLisanslari
                            {
                                faturaId=fatura.faturaId,
                                hizmetId=faturadakiHizmet.hizmetId,
                                subeId=null,
                                hizmetLisansBaslangicTarihi=DateTime.Now,
                                hizmetLisansBitisTarihi=bitisTarihi,
                            });
                        }
                        iRestaurantModel.FaturaOdemeleri.Add(new FaturaOdemeleri
                        {
                            faturaId = faturaId,
                            faturaOdemeMiktari = Math.Round((aratop * 1.18), 2),
                            faturaOdemeTarihi=DateTime.Now,
                            paraBirimiId=fatura.FaturadakiHizmetler.First().paraBirimiId
                            
                        });
                        iRestaurantModel.SaveChanges();
                        return HttpStatusCode.OK;
                    }//Gelen ödeme bildiriminde bahsi geçen fatura henüz ödenmemiştir. Veritabanında ödendi olarak gösterilmeli ve lisanslar tayin edilmeli
                }//Gelen ödeme bildiriminde bahsi geçen faturaId'si veritabanımızda bulunuyor.
            }//Sitemiz üzerinde çıkan tutarla eş ödeme yapılmıştır. Fatura kontrolüne geçebiliriz.
            else if(paymentData.amount> aratop * 1.18)//Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek
            {
                ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek";
                bool refundStatus = RefundPayment(paymentData.id);
                if(refundStatus == true)//EĞER GERİ ÖDEME BAŞARILIYSA
                {
                    ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Sepetiniz iptal edildi. Ödenmiş olan ücret PAPARA'ya geri iade edildi.";
                    Session["sepet"] = null;
                }//EĞER GERİ ÖDEME BAŞARILIYSA
                else//EĞER GERİ ÖDEME BAŞARI-SIZSA
                {
                    ViewBag.message = "Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Sepetiniz iptal edildi. Ve bir hatadan ötürü ödenmiş olan ücret PAPARA'ya geri iade edilemedi. Lütfen bizimle irtibata geçin.";
                    Session["sepet"] = null;
                }//EĞER GERİ ÖDEME BAŞARI-SIZSA
                return HttpStatusCode.OK;
            }//Sitemiz üzerinde çıkan tutardan fazla ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek
            else//Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek.
            {
                ViewBag.message = "Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Ücret geri iade edilecek.";
                bool refundStatus = RefundPayment(paymentData.id);
                if (refundStatus == true)//EĞER GERİ ÖDEME BAŞARILIYSA
                {
                    ViewBag.message = "Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Sepetiniz iptal edildi. Ve ödenmiş olan ücret PAPARA'ya geri iade edildi. Yönetim panelinizden ödenmemiş faturalarınıza göz atıp";
                    TempData["errorMessaga"] = ViewBag.message;
                    Session["sepet"] = null;
                }//EĞER GERİ ÖDEME BAŞARILIYSA
                else//EĞER GERİ ÖDEME BAŞARI-SIZSA
                {
                    ViewBag.message = "Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Sepetiniz iptal edildi. Ve bir hatadan ötürü ödenmiş olan ücret PAPARA'ya geri iade edilemedi. Lütfen bizimle irtibata geçin.";
                    Session["sepet"] = null;
                }//EĞER GERİ ÖDEME BAŞARI-SIZSA
                return HttpStatusCode.OK;
            }//Sitemiz üzerinde çıkan tutardan az ödeme yapılmıştır. Ödenmiş olan ücret geri iade edilecek.

        }
        public async Task<ActionResult> Contact(string name, string email, string message)
        {
            if(name != null && email !=null && message != null)
            {
                #region //EMAİL DİZAYN ET

                string subject = "iRestaurant'a mesaj gönderildi.";
                string mailMesaji =
                    "Merhaba, " + "iRestaurant Yönetimi" +
                    "<br><br>" +
                    "Gönderen Adı: " + name +
                    "<br>" +
                    "Gönderen Email :" + email +
                    "<br><br>"+
                    message +
                    "<br><br>" +
                    "İyi günler dileriz, iRestaurant İletişim Birimi."
                ;
                EmailFormModel emailForm = new EmailFormModel
                {
                    fromEmail = "fmsforum@gmail.com",
                    fromName = "iRestaurant",
                    message = mailMesaji,
                    subject = subject,
                    toEmail = "silentwimble@hotmail.com",
                    toName = "iRestaurant İletişim Birimi"
                };
                #endregion
                bool EmailGonderildiMi = await Email(emailForm);
                if (!EmailGonderildiMi)
                {
                    ViewBag.message = "Mesajınız gönderilirken bir sorun oluştu. Lütfen ekibimiz bu sorunu giderirken biraz bekleyin, daha sonra tekrar deneyin.";

                }
                else
                {
                    ViewBag.message = "Mesajınız iRestaurant İletişim Birimi'ne iletildi. En kısa sürede size mailiniz üzerinden geri dönüş yapılacaktır.";
                }
                return View("Index");
            }
            else
            {
                ViewBag.message = "Lütfen tüm alanları doldurduğunuzdan emin olun.";
                return View("Index");
            }

            

            
        }
        public ActionResult Error404()
        {



            return View();
        }
        public ActionResult Faq()
        {



            return View();
        }
        public ActionResult Features()
        {

            return View();
        }
        public ActionResult Index()
        {



            return View();
        }
        public ActionResult Login(LoginModel login)
        {
            SirketCookieControl sirketCookie = new SirketCookieControl();
            string donusSayfasi = "Index";
            if (login.returnUrl != null)
                if (login.returnUrl.Length > 0)
                {
                    donusSayfasi = login.returnUrl;
                }
            if (login.sirketEmail !=null && login.sirketParola!=null)
            {
                string sirketParola = login.sirketParola.ToSHA(Crypto.SHA_Type.SHA256);
                Sirketler sirket = iRestaurantModel.Sirketler.Where(x => x.sirketEmail == login.sirketEmail && x.sirketParola == sirketParola).FirstOrDefault();
                if (sirket != null)
                {
                    if (sirket.aktiflestirildiMi == true)
                    {
                        sirketCookie.CookieSil();
                        Sirketler sirketBilgisi = new Sirketler
                        {
                            sirketAd = sirket.sirketAd,
                            sirketEmail = sirket.sirketEmail,
                            sirketParola = sirket.sirketParola,
                        };
                        if(login.hatirla == "on")
                        sirketCookie.CookieKaydet(sirketBilgisi);
                        Session["aktiflestirildiMi"] = sirket.aktiflestirildiMi;
                        Session["sirketId"] = sirket.sirketId;
                        Session["sirketAd"] = sirket.sirketAd;
                        Session["sirketEmail"] = sirket.sirketEmail;
                        Session["sirketTelefon"] = sirket.sirketTelefon;
                        Session["login"] = true;
                        Session["GirilenYer"] = "Home";

                        return RedirectToAction(donusSayfasi, "Home");
                        
                    }
                    else
                    {
                        if (sirketCookie.CookieGetir() != null)
                        {
                            sirketCookie.CookieSil();
                        }
                        Session.RemoveAll();
                        ViewBag.message = "Hesabınız aktifleştirilmeyi bekliyor.";
                    }
                }
                else
                {
                    ViewBag.message = "Email / şifre bilgileriniz hatalıdır. Lütfen yeniden giriş yapınız.";
                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            SirketCookieControl sirketCookie = new SirketCookieControl();
            sirketCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult PaymentCompleted()
        {
            List<SepettekiHizmet> sepetList = new List<SepettekiHizmet>();
            if (Session["sepet"] != null)
            {
                sepetList = (List<SepettekiHizmet>)Session["sepet"];
                if (sepetList.Count == 0)
                {
                    ViewBag.message = "Ödeme alımı sırasında sepetinizin boş olduğu tespit edildi. Lütfen sepetini kontrol edip yeniden deneyin.";
                    return RedirectToAction("Checkout", "Home");
                }
                else
                {// Ödeme almaya başla

                    double aratop = (sepetList.Where(x => x.yillikMi == true && x.hizmetKampanyaId == null).Sum(x => x.hizmetAylikUcret * x.adet) * 12) +
                                      sepetList.Where(x => x.yillikMi == false && x.hizmetKampanyaId == null).Sum(x => x.hizmetAylikUcret * x.adet);

                    double aratop2 = 0;
                    foreach (var hizmetKampanya in sepetList.Where(x => x.hizmetKampanyaId != null).GroupBy(x => x.hizmetKampanyaId))
                    {
                        aratop2 += iRestaurantModel.HizmetKampanyalari.Where(x => x.hizmetKampanyaId == hizmetKampanya.Key.Value).First().hizmetKampanyaFiyat * sepetList.Where(x => x.hizmetKampanyaId == hizmetKampanya.Key.Value).First().adet;
                    }



                }
            }
            else
            {
                ViewBag.message = "Ödeme alımı sırasında sepetinizin boş olduğu tespit edildi. Lütfen sepetini kontrol edip yeniden deneyin.";
                return RedirectToAction("Checkout", "Home");
            }



            return View();
        }
        public ActionResult Pricing()
        {

            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                HizmetKampanyaHizmetleri = iRestaurantModel.HizmetKampanyaHizmetleri.ToList(),
                Hizmetler = iRestaurantModel.Hizmetler.ToList(),
                HizmetKampanyalari = iRestaurantModel.HizmetKampanyalari.ToList(),
                HizmetLisanslari=iRestaurantModel.HizmetLisanslari.ToList(),
                ParaBirimleri = iRestaurantModel.ParaBirimleri.ToList()
            };


            return View(vm);
        }
        public async Task<ActionResult> Recover(RecoverModel recoverModel)
        {
            if (recoverModel.sirketEmail !=null && recoverModel.aktiflestirmeKodu == null && recoverModel.sirketParola == null && recoverModel.sirketParolaTekrar == null)
            {//SADECE EMAİL GİRİLDİYSE ŞİFRE DEĞİŞTİRME İSTEĞİ GELSİN
                if (iRestaurantModel.Sirketler.Where(x => x.sirketEmail == recoverModel.sirketEmail).Count() == 1)
                {
                    Sirketler sirket = iRestaurantModel.Sirketler.Where(x => x.sirketEmail == recoverModel.sirketEmail).First();
                    sirket.aktiflestirmeKodu = (sirket.sirketId.ToString() + new Random().Next(999999,99999999).ToString()).ToSHA(Crypto.SHA_Type.SHA1);
                    iRestaurantModel.SaveChanges();
                    #region //EMAİL DİZAYN ET
                
                    string subject = "Hesabınızda şifre değişikliği talebi belirlendi.";
                    string mailMesaji =
                        "Merhaba, " + sirket.sirketAd +
                        "<br>" +
                            "Üyesi bulunduğunuz Sirus Yazılım - iRestaurant yazılımımızın şirket hesabına erişemediğinizi bildirdiniz." +
                        "<br>" +
                            "Aşağıdaki linke tıklayarak şifrenizi yenileyebilirsiniz;" +
                        "<br>" +
                            "<a style='font-color:blue;' href='http://localhost:62422/Home/Recover?sirketEmail=" +sirket.sirketEmail+ "&aktiflestirmeKodu="+ sirket.aktiflestirmeKodu + "'>Buraya tıklayarak şifrenizi değiştirebilirsiniz.</a>"+
                        "<br>" +
                            "Eğer bu işlemi siz başlatmadıysanız şu an için yapılacak bir şey yok. Eski şifreniz ile iRestaurant'ı kullanmaya devam edebilirsiniz." + 
                        "<br>" +
                        "İyi günler dileriz, iRestaurant Yönetimi."
                    ;
                    EmailFormModel emailForm = new EmailFormModel
                    {
                        fromEmail = "silentwimble@hotmail.com",
                        fromName = "Stockers",
                        message = mailMesaji,
                        subject = subject,
                        toEmail = sirket.sirketEmail,
                        toName = sirket.sirketAd
                    };
                    #endregion
                    bool EmailGonderildiMi = await Email(emailForm);
                    if (!EmailGonderildiMi) {
                        ViewBag.message = "Size email gönderilirken bir sorun oluştu. Lütfen ekibimiz bu sorunu giderirken biraz bekleyin, daha sonra tekrar deneyin.";
                        return View();

                    }
                    else
                    {
                        ViewBag.sifrelerGirilecekMi = false;
                        ViewBag.message = "Lütfen mailinizin gelen kutusunu kontrol edin. Emailler bazen spam kutunuza da düşmektedir. Eğer 15dk içinde mail gelmez ise lütfen tekrar deneyin.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "Lütfen sistemde bulunan e-mail adresinizi giriniz.";
                    return View();
                }
                
            }
            else if (recoverModel.sirketEmail != null && recoverModel.aktiflestirmeKodu != null && recoverModel.sirketParola == null && recoverModel.sirketParolaTekrar == null)
            {//EMAİL ŞİFRELER GİRİLECEK
                ViewBag.sifrelerGirilecekMi = true;
                ViewBag.aktiflestirmeKodu = recoverModel.aktiflestirmeKodu;
                ViewBag.sirketEmail = recoverModel.sirketEmail;
                return View();
            }
            else if(recoverModel.sirketEmail !=null && recoverModel.aktiflestirmeKodu != null && recoverModel.sirketParola !=null && recoverModel.sirketParolaTekrar !=null)
            {//ŞİFRELER GİRİLDİ İŞLEM YAP
                
                if (recoverModel.sirketParola == recoverModel.sirketParolaTekrar)
                {
                    if (iRestaurantModel.Sirketler.Where(x => x.sirketEmail == recoverModel.sirketEmail && x.aktiflestirmeKodu == recoverModel.aktiflestirmeKodu).Count() == 1)
                    {
                        Sirketler sirket = iRestaurantModel.Sirketler.Where(x => x.sirketEmail == recoverModel.sirketEmail && x.aktiflestirmeKodu == recoverModel.aktiflestirmeKodu).First();
                        if (sirket != null)
                        {
                            sirket.sirketParola = recoverModel.sirketParola.ToSHA(Crypto.SHA_Type.SHA256);
                            iRestaurantModel.SaveChanges();
                            ViewBag.message = "Şifreniz değiştirilmiştir. Şifrenizle giriş yapabilirsiniz.";
                            return View("Login");

                        }
                        else
                        {
                            ViewBag.message = "Email ve aktifleştirme kodunuz uyuşmuyor ya da aktivasyon kodu geçerliliğini yitirmiş. Lütfen tekrar deneyiniz.";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.message = "Email ve aktifleştirme kodunuz uyuşmuyor ya da aktivasyon kodu geçerliliğini yitirmiş. Lütfen tekrar deneyiniz.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.sifrelerGirilecekMi = true;
                    ViewBag.aktiflestirmeKodu = recoverModel.aktiflestirmeKodu;
                    ViewBag.sirketEmail = recoverModel.sirketEmail;
                    ViewBag.message = "Şifreleriniz uyuşmuyor. Lütfen yeniden deneyiniz.";
                    return View();
                }
            }
            else if (recoverModel.sirketEmail != null && recoverModel.aktiflestirmeKodu != null && recoverModel.sirketParola != recoverModel.sirketParolaTekrar)
            {
                ViewBag.message = "Şifreleriniz uyuşmamaktadır. ";
                return RedirectToAction("Recover", "Home", new RecoverModel { sirketEmail = recoverModel.sirketEmail, aktiflestirmeKodu = recoverModel.aktiflestirmeKodu });
            }
            else if(recoverModel.sirketEmail == null && recoverModel.aktiflestirmeKodu == null && recoverModel.sirketParola == null && recoverModel.sirketParolaTekrar == null)
            {
                return View();
            }
            else
            {
                return View();
            }
        }
        public async Task<ActionResult> Register(Sirketler sirket, string sirketParolaTekrar, string terms)
        {
            ViewModelDemoVM vm = new ViewModelDemoVM
            {
                Ulkeler = iRestaurantModel.Ulkeler.ToList()
            };
            try
            {
                #pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if((sirket.ilceId != null || sirket.ilceId != 0) && sirket.sirketAd != null && sirket.sirketEmail != null && sirket.sirketParola != null && sirketParolaTekrar != null && sirket.sirketTelefon != null)
                #pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    if (terms == "on")
                    {
                        if (sirket.sirketParola == sirketParolaTekrar)
                        {
                            if (iRestaurantModel.Sirketler.Where(x => x.sirketEmail == sirket.sirketEmail).Count() == 0)
                            {
                                sirket.aktiflestirildiMi = false;
                                sirket.aktiflestirmeKodu = "0";
                                sirket.sirketParola = sirket.sirketParola.ToSHA(Crypto.SHA_Type.SHA256);
                                Sirketler yenisirket = iRestaurantModel.Sirketler.Add(sirket);
                                yenisirket.aktiflestirmeKodu = (yenisirket.sirketId.ToString() + new Random().Next(999999, 99999999).ToString()).ToSHA(Crypto.SHA_Type.SHA1);

                                #region //EMAİL DİZAYN ET

                                string subject = "iRestaurant'a kaydınız başarıyla tamamlandı.";
                                string mailMesaji =
                                    "Merhaba, " + sirket.sirketAd +
                                    "<br>" +
                                        "Sirus Yazılım - iRestaurant yazılımımıza bu email adresiyle kayıt olduğunuz için bu maili almaktasınız." +
                                    "<br>" +
                                        "Aşağıdaki linke tıklayarak email doğrulama işleminizi gerçekleştirmelisiniz;" +
                                    "<br>" +
                                        "<a class='font-color:green;' href='http://localhost:62422/Home/Validate?sirketEmail=" + sirket.sirketEmail + "&aktiflestirmeKodu=" + sirket.aktiflestirmeKodu + "'>Buraya tıklayarak doğrulama işlemini başlatın.</a> "+
                                    "<br>" +
                                        "Eğer bu işlemi siz başlatmadıysanız endişelenecek bir şey yok. Sistem, doğrulama yapılmayan şirketleri 30 gün içerisinde otomatik olarak kaldırmaktadır." +
                                    "<br>" +
                                    "İyi günler dileriz, iRestaurant Yönetimi."
                                ;
                                EmailFormModel emailForm = new EmailFormModel
                                {
                                    fromEmail = "fmsforum@gmail.com",
                                    fromName = "iRestaurant",
                                    message = mailMesaji,
                                    subject = subject,
                                    toEmail = sirket.sirketEmail,
                                    toName = sirket.sirketAd
                                };
                                #endregion
                                bool EmailGonderildiMi = await Email(emailForm);
                                if (!EmailGonderildiMi)
                                {
                                    ViewBag.message = "Size email gönderilirken bir sorun oluştu. Lütfen ekibimiz bu sorunu giderirken biraz bekleyin, daha sonra tekrar deneyin.";

                                }
                                else
                                {
                                    iRestaurantModel.SaveChanges();
                                    ViewBag.message = "Girdiğiniz eposta adresinize doğrulama epostası gönderildi. Giriş yapabilmek için lütfen eposta adresinize gelen doğrulama linkine tıklayın.";
                                    return RedirectToAction("Login", "Home");
                                }
                            }
                            else
                            {
                                ViewBag.message = "Girdiğiniz eposta adresi daha önce kullanılmış. Lütfen yeni bir eposta adresiyle tekrar deneyin, ya da zaten var olan hesabınıza giriş yapın.";
                                return RedirectToAction("Login", "Home");
                            }
                            

                        }
                        else
                        {
                            ViewBag.message = "Şifreleriniz uyuşmamaktadır.";
                        }
                    }
                    else
                    {
                        ViewBag.message = "Kullanım şartlarını kabul etmelisiniz.";
                    }
                }
                else
                {
                    ViewBag.message = "Lütfen tüm alanları girdiğinizden emin olun.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = "Bir hata oluştu. Lütfen kayıt olmayı daha sonra tekrar deneyiniz.";
            }
            
            return View(vm);
        }
        public ActionResult Testimonials()
        {



            return View();
        }
        public ActionResult Terms()
        {



            return View();
        }
        public ActionResult Validate(string sirketEmail, string aktiflestirmeKodu)
        {
            if(sirketEmail !=null && aktiflestirmeKodu != null)
            {
                try
                {
                    if(iRestaurantModel.Sirketler.Where(x => x.aktiflestirmeKodu == aktiflestirmeKodu && x.sirketEmail == sirketEmail).Count() > 0)
                    {
                        Sirketler sirket = iRestaurantModel.Sirketler.Where(x => x.aktiflestirmeKodu == aktiflestirmeKodu && x.sirketEmail == sirketEmail).First();
                        if (sirket != null)
                        {
                            sirket.aktiflestirildiMi = true;
                            sirket.aktiflestirmeKodu = (sirket.sirketId.ToString() + new Random().Next(999999, 99999999).ToString()).ToSHA(Crypto.SHA_Type.SHA1);
                            iRestaurantModel.SaveChanges();
                            ViewBag.message = "Doğrulama işleminiz gerçekleşti. Artık giriş yapabilirsiniz.";
                            return View("Login");
                        }
                        else
                        {
                            ViewBag.message = "Doğrulama kodu geçersiz veya hesabınız zaten doğrulanmış durumda.";
                            return View("Login");
                        }
                    }
                    else
                    {
                        ViewBag.message = "Doğrulama kodu geçersiz veya hesabınız zaten doğrulanmış durumda.";
                        return View("Login");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.message = "Doğrulama işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                    Console.WriteLine(e.Message);
                }
                
            }
            return View("Login");
        }

        #endregion

        #region // SEPET EKLE SİL GÜNCELLE

        public ActionResult AddCampaignToCart(int hizmetKampanyaId)
        {
            try
            {
                List<HizmetKampanyaHizmetleri> hizmetkampanyahizmetleri = iRestaurantModel.HizmetKampanyalari.Find(hizmetKampanyaId).HizmetKampanyaHizmetleri.ToList();
                List<SepettekiHizmet> sepeteYeniEklenenHizmetler = new List<SepettekiHizmet>();



                foreach (var hizmetKampanyaHizmeti in hizmetkampanyahizmetleri)
                {
                    SepettekiHizmet sepettekiHizmet = new SepettekiHizmet();
                    sepettekiHizmet.hizmetAd = hizmetKampanyaHizmeti.Hizmetler.hizmetAd;
                    sepettekiHizmet.adet = 1;
                    sepettekiHizmet.hizmetAylikUcret = hizmetKampanyaHizmeti.Hizmetler.hizmetAylikUcret;
                    sepettekiHizmet.hizmetId = hizmetKampanyaHizmeti.hizmetId;
                    sepettekiHizmet.hizmetTuru = hizmetKampanyaHizmeti.Hizmetler.hizmetTuru;
                    sepettekiHizmet.paraBirimiId = hizmetKampanyaHizmeti.Hizmetler.paraBirimiId;
                    sepettekiHizmet.yillikMi = hizmetKampanyaHizmeti.HizmetKampanyalari.yillikMi;
                    sepettekiHizmet.hizmetKampanyaId = hizmetKampanyaId;
                    sepeteYeniEklenenHizmetler.Add(sepettekiHizmet);
                }

                if (Session["sepet"] != null)
                {
                    List<SepettekiHizmet> sessiondakiHizmetler = (List<SepettekiHizmet>)Session["sepet"];

                    if (sessiondakiHizmetler.Where(x => x.hizmetKampanyaId.Value == hizmetKampanyaId).Count() > 0)
                    {
                        foreach (var sessiondakiHizmet in sessiondakiHizmetler.Where(x => x.hizmetKampanyaId.Value == hizmetKampanyaId))
                        {
                            sessiondakiHizmet.adet += 1;
                        }
                    }
                    else
                    {
                        foreach (var sepettekiYeniHizmet in sepeteYeniEklenenHizmetler)
                        {
                            sessiondakiHizmetler.Add(sepettekiYeniHizmet);
                        }
                    }

                    Session["sepet"] = sessiondakiHizmetler;
                    /*
                    foreach (var sepettekiYeniHizmet in sepeteYeniEklenenHizmetler)
                {
                    if (sessiondakiHizmetler.Where(x => x.hizmetId == sepettekiYeniHizmet.hizmetId && x.yillikMi == sepettekiYeniHizmet.yillikMi && sepettekiYeniHizmet.hizmetKampanyaId.Value == hizmetKampanyaId).Count() > 0)
                    {
                        foreach(var sepettekiHizmet in sessiondakiHizmetler.Where(x => x.hizmetId == sepettekiYeniHizmet.hizmetId && x.yillikMi == sepettekiYeniHizmet.yillikMi && sepettekiYeniHizmet.hizmetKampanyaId.Value == hizmetKampanyaId))
                        {
                            sepettekiHizmet.adet += 1;
                        }
                    }
                    else
                    {
                        sessiondakiHizmetler.Add(sepettekiYeniHizmet);
                    }
                }*/
                }
                else
                {
                    Session["sepet"] = sepeteYeniEklenenHizmetler;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = "Bir hata oluştu. Lütfen sepete ürün eklemeyi tekrar deneyiniz. Sorun devam ederse çerezleri temizledikten sonra tarayıcınızı yeniden başlatıp tekrar deneyiniz.";
            }

            return RedirectToAction("Cart", "Home");
        }
        public ActionResult AddItemToCart(int hizmetId, string yillik)
        {//EXPERIMENTAL
            bool yillikMi = false;
            if (yillik == "True")
                yillikMi = true;
            try
            {
                if (Session["sepet"] != null)
                {
                    List<SepettekiHizmet> sessiondakiHizmetler = (List<SepettekiHizmet>)Session["sepet"];
                    if (sessiondakiHizmetler.Where(x => x.hizmetId == hizmetId && x.yillikMi == yillikMi && x.hizmetKampanyaId == null).Count() > 0)
                    {
                        sessiondakiHizmetler.Where(x => x.hizmetId == hizmetId && x.yillikMi == yillikMi && x.hizmetKampanyaId == null).First().adet += 1;

                        Session["sepet"] = sessiondakiHizmetler;
                        ViewBag.message = "Ürünün sepetteki adeti arttırılmıştır.";
                    }
                    else
                    {
                        Hizmetler hizmet = iRestaurantModel.Hizmetler.Where(x => x.hizmetId == hizmetId).First();
                        if (hizmet != null)
                        {
                            sessiondakiHizmetler.Add(new SepettekiHizmet
                            {
                                adet = 1,
                                hizmetAd = hizmet.hizmetAd,
                                hizmetAylikUcret = hizmet.hizmetAylikUcret,
                                paraBirimiId = hizmet.paraBirimiId,
                                hizmetId = hizmet.hizmetId,
                                hizmetTuru = hizmet.hizmetTuru,
                                yillikMi = yillikMi,
                                hizmetKampanyaId = null
                            });
                            Session["sepet"] = sessiondakiHizmetler;
                        }

                        ViewBag.message = "Ürün sepete eklenmiştir.";
                    }
                }
                else
                {
                    ViewBag.message = "Sepetinizde zaten ürün bulunmamaktadır.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = "Bir hata oluştu. Lütfen sepetten ürün çıkartmayı tekrar deneyiniz. Sorun devam ederse çerezleri temizledikten sonra tarayıcınızı yeniden başlatıp tekrar deneyiniz.";
            }
            return RedirectToAction("Cart", "Home");
        }
        public ActionResult RemoveCampaignFromCart(int hizmetKampanyaId, string yillik)
        {//BİR KAMPANYANIN BİR İTEMİNİ SİLMEK, O KAMPANYANIN TÜM İTEMLERİNİ 1 AZALTMAK VE ARTIK ADEDİ 0'A DÜŞMÜŞSE, O KAMPANYANIN İTEMLERİNİ SİLMEK DEMEK
            bool yillikMi = false;
            if (yillik == "True")
                yillikMi = true;
            try
            {
                if (Session["sepet"] != null)
                {
                    List<SepettekiHizmet> sessiondakiHizmetler = (List<SepettekiHizmet>)Session["sepet"];
                    if (sessiondakiHizmetler.Where(x => x.hizmetKampanyaId == hizmetKampanyaId && x.yillikMi == yillikMi).Count() > 0)
                    {//SİLİNECEK KAMPANYA SEPETTE VAR İSE
                        foreach (var sessiondakiHizmet in sessiondakiHizmetler.Where(x => x.hizmetKampanyaId == hizmetKampanyaId && x.yillikMi == yillikMi))
                        {//ÖNCE SİLİNECEK KAMPANYANIN İÇİNDEKİ HİZMETLERİN HER BİRİNİN ADEDİNİ 1 AZALT
                            sessiondakiHizmet.adet -= 1;

                        }
                        sessiondakiHizmetler.RemoveAll(x => x.adet == 0);
                        Session["sepet"] = sessiondakiHizmetler;
                        ViewBag.message = "Kampanya sepetinizden kaldırılmıştır.";
                    }
                    else
                    {
                        ViewBag.message = "Bu kampanya sepetinizde bulunmamaktadır.";
                    }
                }
                else
                {
                    ViewBag.message = "Sepetinizde ürün bulunmamaktadır.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = "Bir hata oluştu. Lütfen sepetten ürün çıkartmayı tekrar deneyiniz. Sorun devam ederse tarayıcınızı yeniden başlatıp sepetinize yeniden ürün ekleyiniz.";
            }
            return RedirectToAction("Cart", "Home");
        }
        public ActionResult RemoveItemFromCart(int hizmetId, string yillik)
        {//EXPERIMENTAL
            bool yillikMi = false;
            if (yillik == "True")
                yillikMi = true;
            try
            {
                if (Session["sepet"] != null)
                {
                    List<SepettekiHizmet> sessiondakiHizmetler = (List<SepettekiHizmet>)Session["sepet"];
                    if (sessiondakiHizmetler.Where(x => x.hizmetId == hizmetId && x.yillikMi == yillikMi && x.hizmetKampanyaId == null).Count() > 0)
                    {
                        sessiondakiHizmetler.Where(x => x.hizmetId == hizmetId && x.yillikMi == yillikMi && x.hizmetKampanyaId == null).First().adet = 0;
                        if (sessiondakiHizmetler.Where(x => x.hizmetId == hizmetId && x.yillikMi == yillikMi && x.hizmetKampanyaId == null).First().adet == 0)
                        {
                            sessiondakiHizmetler.Remove(sessiondakiHizmetler.Where(x => x.hizmetId == hizmetId && x.yillikMi == yillikMi && x.hizmetKampanyaId == null).First());
                        }
                        Session["sepet"] = sessiondakiHizmetler;
                        ViewBag.message = "Ürün sepetinizden kaldırılmıştır.";
                    }
                    else
                    {
                        ViewBag.message = "Bu ürün sepetinizde bulunmamaktadır.";
                    }
                }
                else
                {
                    ViewBag.message = "Sepetinizde zaten ürün bulunmamaktadır.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = "Bir hata oluştu. Lütfen sepetten ürün çıkartmayı tekrar deneyiniz. Sorun devam ederse çerezleri temizledikten sonra tarayıcınızı yeniden başlatıp tekrar deneyiniz.";
            }
            return RedirectToAction("Cart", "Home");
        }
        public JsonResult ChangeCampaignQuantityFromCart(int adet, int hizmetKampanyaId)
        {
            try
            {
                if (Session["sepet"] != null)
                {
                    List<SepettekiHizmet> sessiondakiHizmetlistesi = ((List<SepettekiHizmet>)Session["sepet"]);
                    if (sessiondakiHizmetlistesi.Where(x => x.hizmetKampanyaId == hizmetKampanyaId).Count() > 0)
                    {
                        foreach (var sepettekiKampanyaliHizmet in sessiondakiHizmetlistesi.Where(x => x.hizmetKampanyaId == hizmetKampanyaId))
                        {
                            sepettekiKampanyaliHizmet.adet = adet;
                        }
                        sessiondakiHizmetlistesi.RemoveAll(x => x.adet == 0);
                        ViewBag.message = "Ürün adetleri değiştirilmiştir. Adedi 0 olan ürünler sepetten kaldırılmıştır.";
                        Session["sepet"] = sessiondakiHizmetlistesi;
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.message = "Bu kampanya sepetinizde bulunmamaktadır.";
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.message = "Sepetinizde hiç ürün bulunmamaktadır.";
                    return Json(3, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = "Bir hata oluştu. Lütfen sepetten ürün çıkartmayı tekrar deneyiniz. Sorun devam ederse çerezleri temizledikten sonra tarayıcınızı yeniden başlatıp tekrar deneyiniz.";
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ChangeItemQuantityFromCart(int hizmetId, string yillik, int adet, int? hizmetKampanyaId)
        {//EXPERIMENTAL
            bool yillikMi = false;
            if (yillik == "True")
                yillikMi = true;
            try
            {
                if (Session["sepet"] != null)
                {
                    List<SepettekiHizmet> sessiondakiHizmetlistesi = (List<SepettekiHizmet>)Session["sepet"];
                    if (hizmetKampanyaId.HasValue)//Eğer sepete bir kampanya aracılığıyla eklenmişse
                    {
                        List<SepettekiHizmet> sessiondakiKampanyaliHizmetler = sessiondakiHizmetlistesi.Where(x => x.yillikMi == yillikMi && x.hizmetKampanyaId == hizmetKampanyaId.Value).ToList();
                        if (sessiondakiKampanyaliHizmetler.Count > 0)
                        {
                            foreach (var sepettekiKampanyaliHizmet in sessiondakiKampanyaliHizmetler)
                            {
                                sepettekiKampanyaliHizmet.adet = adet;
                            }
                            ViewBag.message = "Ürün adetleri değiştirilmiştir.";
                            foreach (var sepettekiKampanyaliHizmet in sessiondakiKampanyaliHizmetler)
                            {
                                if (sepettekiKampanyaliHizmet.adet == 0)
                                {
                                    sessiondakiKampanyaliHizmetler.Remove(sepettekiKampanyaliHizmet);
                                    ViewBag.message = "Ürün sepetinizden kaldırılmıştır.";
                                }
                            }

                            Session["sepet"] = sessiondakiHizmetlistesi;

                            return Json(0, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            ViewBag.message = "Bu ürün sepetinizde bulunmamaktadır.";
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else //Eğer sepete kampanyasız eklenmişse
                    {
                        List<SepettekiHizmet> sessiondakiKampanyasizHizmetler = sessiondakiHizmetlistesi.Where(x => x.hizmetId == hizmetId && x.yillikMi == yillikMi && x.hizmetKampanyaId == null).ToList();
                        if (sessiondakiKampanyasizHizmetler.Count > 0)
                        {
                            sessiondakiKampanyasizHizmetler.First().adet = adet;
                            ViewBag.message = "Ürün adedi değiştirilmiştir.";
                            if (sessiondakiKampanyasizHizmetler.First().adet == 0)
                            {
                                sessiondakiKampanyasizHizmetler.Remove(sessiondakiKampanyasizHizmetler.First());
                                ViewBag.message = "Ürün sepetinizden kaldırılmıştır.";
                            }


                            Session["sepet"] = sessiondakiHizmetlistesi;

                            return Json(0, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            ViewBag.message = "Bu ürün sepetinizde bulunmamaktadır.";
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else
                {
                    ViewBag.message = "Sepetinizde hiç ürün bulunmamaktadır.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.message = "Bir hata oluştu. Lütfen sepetten ürün çıkartmayı tekrar deneyiniz. Sorun devam ederse çerezleri temizledikten sonra tarayıcınızı yeniden başlatıp tekrar deneyiniz.";
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}