using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using iRestaurant.Models;
using static iRestaurant.Controllers.KasaController;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Resources;

namespace iRestaurant.Controllers
{
    public class YazdiriciController: Controller
    {
        iRestaurantDataBaseClass yazdiriciModel = new iRestaurantDataBaseClass();
        
        public class EkstrasizYazdirilacak
        {
            public string katAd { get; set; }
            public string personelAd { get; set; }
            public string masaAd { get; set; }
            public string urunAd { get; set; }
            public string urunOzellikAd { get; set; }
            public int adet { get; set; }
            public DateTime siparisTarihi { get; set; }
            public string departmanIp { get; set; }
            public string siparisNotu { get; set; }
        }
        public class EkstraliYazdirilacak
        {
            public string katAd { get; set; }
            public string personelAd { get; set; }
            public string masaAd { get; set; }
            public string urunAd { get; set; }
            public string urunOzellikAd { get; set; }
            public List<String> ekstralar { get; set; }
            public DateTime siparisTarihi { get; set; }
            public string departmanIp { get; set; }
            public string siparisNotu { get; set; }
            public EkstraliYazdirilacak()
            {
                ekstralar = new List<string>();
            }
        }
        public class PersoneleYazdirilacak
        {
            public List<EkstrasizYazdirilacak> ekstrasizYazdirilacak;
            public List<EkstraliYazdirilacak> ekstraliYazdirilacak;
            public PersoneleSiparisDetaylar personeleSiparisDetaylar;
            public PersoneleYazdirilacak()
            {
                ekstrasizYazdirilacak = new List<EkstrasizYazdirilacak>();
                ekstraliYazdirilacak = new List<EkstraliYazdirilacak>();
                personeleSiparisDetaylar = new PersoneleSiparisDetaylar();
            }
            public class PersoneleSiparisDetaylar
            {
                public DateTime siparisTarihi { get; set; }
                public string katAd { get; set; }
                public string masaAd { get; set; }
                public string personelAd { get; set; }
                public string siparisNotu { get; set; }
            }
        }

        public static string personelIp = "" /*"192.168.1.3"*/;
        public static string kasaYaziciIp = ""/*"192.168.1.4"*/;
        public static string sirketAd = ""/*"192.168.1.4"*/;
        public static bool personelYazicisiOtomatikMi = false/*"192.168.1.4"*/;
        public void Initialize(int subeId)
        {
            Subeler sube = yazdiriciModel.Subeler.Find(subeId);

            sirketAd = sube.Sirketler.sirketAd;
            personelIp = sube.personelYaziciIp;
            kasaYaziciIp = sube.kasaYaziciIp;
            personelYazicisiOtomatikMi = sube.personelYazicisiOtomatikMi;
        }

        public bool DokumYazdir(List<PersonelPerformans> personelPerformansList, List<OdemeYontemiDokum> odemeYontemiDokumuList, DateTime baslangicTarihi, DateTime bitisTarihi)
        {
            bool durum = true;
            StringBuilder sb = new StringBuilder();
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text

            //1.SATIR
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append("KASA\n");
            //1.SATIR

            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("-----" + baslangicTarihi.ToString("dd/MM/yyyy HH:mm") +"  --  "+ bitisTarihi.ToString("dd/MM/yyyy HH:mm")+"-----\n");
            sb.Append("================================================\n");
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("--------Tarih & Saat Aralığı Ciro Dökümü--------\n");
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("================================================\n");
            //ORTA ÇİZGİ ORTALA

            sb.Append("              PERSONEL PERFORMANSI              \n");
            sb.Append("|PERSONEL|           |YÜZDE|    |PER. HASILAT|  \n");
            //4.SATIR
            char[] personelSatirlarinDizisi = new char[48];
            char[] odemeSatirlarinDizisi = new char[48];
            sb.Append(ESC + "a" + (char)0); // Align left

            foreach (var personelPerformans in personelPerformansList)
            {
                personelPerformans.personelAd = " " + personelPerformans.personelAd;
                string personelMiktar= personelPerformans.personelMiktar.ToString("F2");
                double personelYuzde = Math.Round(personelPerformans.personelYuzde, 2);
                for (int i = 0; i < personelPerformans.personelAd.ToCharArray().Length; i++)
                {
                    personelSatirlarinDizisi[i] = personelPerformans.personelAd.ToCharArray()[i];
                }
                for (int i = personelPerformans.personelAd.ToCharArray().Length; i < 20; i++)
                {
                    personelSatirlarinDizisi[i] = ' ';
                }
                personelSatirlarinDizisi[20] = ' ';
                personelSatirlarinDizisi[21] = '%';
                for (int i = 0; i < personelYuzde.ToString("F2").ToCharArray().Length; i++)
                {
                    personelSatirlarinDizisi[i + 22] = personelYuzde.ToString("F2").ToCharArray()[i];
                }
                for (int i = 26; i <= 44; i++)
                {
                    personelSatirlarinDizisi[i] = ' ';
                }
                for (int i = personelMiktar.ToString().ToCharArray().Length; i >0 ; i--)
                {
                    personelSatirlarinDizisi[35 + i + (9 - personelMiktar.ToString().ToCharArray().Length)] = personelMiktar.ToString().ToCharArray()[i-1];
                }
                personelSatirlarinDizisi[45] = ' ';
                personelSatirlarinDizisi[46] = 'T';
                personelSatirlarinDizisi[47] = 'L';
                string str = new string(personelSatirlarinDizisi);
                sb.Append(str + "\n");
            }
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("TOPLAM: " + personelPerformansList.Sum(x => x.personelMiktar).ToString("F2") + " TL");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)2); // Align right
            
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("================================================\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            //ORTA ÇİZGİ ORTALA

            sb.Append("                ÖDEME YÖNTEMLERİ                \n");
            sb.Append("|ODEME TÜRÜ|         |YÜZDE|      |MÜŞ. ÖDEME|  \n");

            foreach (var odemeYontemi in odemeYontemiDokumuList)
            {
                odemeYontemi.yontemAd = " " + odemeYontemi.yontemAd;
                string odemeYontemiMiktar = odemeYontemi.yontemMiktar.ToString("F2");
                char[] odemeYonMiktar5Karakter = odemeYontemiMiktar.ToString().ToCharArray();
                
                double odemeYontemiYuzde = Math.Round(odemeYontemi.yontemYuzde, 2);
                for (int i = 0; i < odemeYontemi.yontemAd.ToCharArray().Length; i++)
                {
                    odemeSatirlarinDizisi[i] = odemeYontemi.yontemAd.ToCharArray()[i];
                }
                for (int i = odemeYontemi.yontemAd.ToCharArray().Length; i < 20; i++)
                {
                    odemeSatirlarinDizisi[i] = ' ';
                }
                odemeSatirlarinDizisi[20] = ' ';
                odemeSatirlarinDizisi[21] = '%';
                for (int i = 0; i < odemeYontemiYuzde.ToString().ToCharArray().Length; i++)
                {
                    odemeSatirlarinDizisi[i + 22] = odemeYontemiYuzde.ToString().ToCharArray()[i];
                }

                for (int i = 26; i <= 44; i++)
                {
                    odemeSatirlarinDizisi[i] = ' ';
                }
                for (int i = odemeYontemiMiktar.ToString().ToCharArray().Length; i > 0; i--)
                {
                    odemeSatirlarinDizisi[35 + i + (9 - odemeYontemiMiktar.ToString().ToCharArray().Length)] = odemeYontemiMiktar.ToString().ToCharArray()[i - 1];
                }

                odemeSatirlarinDizisi[45] = ' ';
                odemeSatirlarinDizisi[46] = 'T';
                odemeSatirlarinDizisi[47] = 'L';
                string str = new string(odemeSatirlarinDizisi);
                sb.Append(str + "\n");

            }
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("TOPLAM: " + odemeYontemiDokumuList.Sum(x => x.yontemMiktar).ToString("F2") + " TL");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)2); // Align right
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            // Feed and cut paper
            sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                TcpPrint(kasaYaziciIp, b);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                durum = false;
            }
            return durum;
        }
        public bool FisYazdir(List<FisItem> fisItemList)
        {
            bool durum=true;
            StringBuilder sb = new StringBuilder();
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text
            //1.SATIR
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append("\n");
            //1.SATIR
            //2.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(DateTime.Now.ToShortDateString()+ "\n");
            //2.SATIR

            //3.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(DateTime.Now.ToShortTimeString() + "\n");
            //3.SATIR

            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            sb.Append("|Ürün Adı|                   |Adet| |Ara Toplam|\n");
            //4.SATIR
            char[] satirlarinDizisi = new char[48];
            char[] urunAdArray = new char[30];
            char[] ekstraArray = new char[48];
            char[] urunAdetArray = new char[3];
            char[] urunFiyatArray = new char[10];

            sb.Append(ESC + "a" + (char)0); // Align left

            #region satiryazdirma
            foreach (var fisItemi in fisItemList)
            {
                
                for (int i=0; i< fisItemi.urunAd.ToCharArray().Length; i++)
                {
                    satirlarinDizisi[i] = fisItemi.urunAd.ToCharArray()[i];
                }
                for (int i = fisItemi.urunAd.ToCharArray().Length; i < 30; i++)
                {
                    satirlarinDizisi[i] = ' ';
                }
                satirlarinDizisi[30] = ' ';
                satirlarinDizisi[31] = 'x';
                for (int i = 0; i < fisItemi.urunAdedi.ToString().ToCharArray().Length; i++)
                {
                    satirlarinDizisi[i+32] = fisItemi.urunAdedi.ToString().ToCharArray()[i];
                }
                for (int i = 32+fisItemi.urunAdedi.ToString().ToCharArray().Length; i < 35; i++)
                {
                    satirlarinDizisi[i] = ' ';
                }

                for (int i = 35; i <= 44; i++)
                {
                    satirlarinDizisi[i] = ' ';
                }
                for (int i = 0; i < (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray().Length; i++)
                {
                    satirlarinDizisi[36+i+ (9-(fisItemi.urunAdedi* (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray().Length)] = (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray()[i];
                }

                satirlarinDizisi[45] = ' ';
                satirlarinDizisi[46] = 'T';
                satirlarinDizisi[47] = 'L';
                string str = new string(satirlarinDizisi);
                sb.Append(str+ "\n");
                
            }
            #endregion satiryazdirma

            string str2 = new string(ekstraArray);
            sb.Append(str2+"\n");

            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            //ORTA ÇİZGİ ORTALA

            sb.Append(ESC + "a" + (char)1); // Align right
            sb.Append("Toplam: "+ (fisItemList.Sum(x=> (x.urunOzellikFiyatToplam + x.ekstraToplami) * x.urunAdedi).ToString("F2")) + " TL"  );
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)1); // Align right


            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            // Feed and cut paper
            sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                TcpPrint(kasaYaziciIp, b);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                durum = false;
            }
            return durum;
        }
        public bool HesapYazdir(List<HesapItem> fisItemList, List<string> personeller,string masaAd)
        {
            string abc = DateTime.Now.ToString("dd.MM.yyyy");
            bool durum = true;
            StringBuilder sb = new StringBuilder();
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append(ESC + "a" + (char)0);
            
            #region Masa ve Tarih Satırı
            char[] masaVeTarihSatiri = new char[48];
            for (int i = 0; i < 48; i++)
            {
                masaVeTarihSatiri[i] = ' ';
            }
            for(int i = 0; i < 5; i++)
            {
                masaVeTarihSatiri[i] = "MASA:"[i];
            }
            for (int i=0; i < masaAd.Length;i++)
            {
                masaVeTarihSatiri[i+5] = masaAd[i];
            }
            masaVeTarihSatiri[37] = ' ';
            for (int i = 0; i < 10; i++)
            {
                masaVeTarihSatiri[i + 38] = abc[i];
            }
            
            string masaVeTarihSatiriSTR = new string(masaVeTarihSatiri);
            sb.Append(masaVeTarihSatiriSTR + "\n");
            #endregion Masa ve Tarih Satırı

            #region Personeller ve Saat Satırı
            char[] personelVeSaatSatiri = new char[48];

            for (int i = 0; i < 48; i++)
            {
                personelVeSaatSatiri[i] = ' ';
            }
            for (int i=0; i< "PERSONEL: ".Length; i++)
            {
                personelVeSaatSatiri[i] = "PERSONEL: "[i];
            }
            string personelString = "";
            foreach (var p in personeller)
            {
                personelString += p;
                if (p != personeller.Last())
                {
                    personelString += "-";
                }
            }
            for (int i = 0; i < personelString.Length; i++)
            {
                personelVeSaatSatiri[i+9] = personelString[i];
            }
            
            for (int i = 0; i < 5; i++)
            {
                personelVeSaatSatiri[i+43] = DateTime.Now.ToShortTimeString()[i];
            }
            string personelVeSaatSatiriSTR = new string(personelVeSaatSatiri);
            sb.Append(personelVeSaatSatiriSTR + "\n");
            #endregion Personeller ve Saat Satırı

            sb.Append(ESC + "a" + (char)0);
            
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            sb.Append("|Ürün Adı|                   |Adet| |Ara Toplam|\n");
            //4.SATIR
            char[] satirlarinDizisi = new char[48];
            char[] urunAdArray = new char[30];
            char[] ekstraArray = new char[48];
            char[] urunAdetArray = new char[3];
            char[] urunFiyatArray = new char[10];

            sb.Append(ESC + "a" + (char)0); // Align left
            #region satirlariyazdir
            foreach (var fisItemi in fisItemList)
            {

                for (int i = 0; i < fisItemi.urunAd.ToCharArray().Length; i++)
                {
                    satirlarinDizisi[i] = fisItemi.urunAd.ToCharArray()[i];
                }
                for (int i = fisItemi.urunAd.ToCharArray().Length; i < 30; i++)
                {
                    satirlarinDizisi[i] = ' ';
                }
                satirlarinDizisi[30] = ' ';
                satirlarinDizisi[31] = 'x';
                for (int i = 0; i < fisItemi.urunAdedi.ToString().ToCharArray().Length; i++)
                {
                    satirlarinDizisi[i + 32] = fisItemi.urunAdedi.ToString().ToCharArray()[i];
                }
                for (int i = 32 + fisItemi.urunAdedi.ToString().ToCharArray().Length; i < 35; i++)
                {
                    satirlarinDizisi[i] = ' ';
                }

                for (int i = 35; i <= 44; i++)
                {
                    satirlarinDizisi[i] = ' ';
                }
                for (int i = 0; i < (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam+fisItemi.ekstraToplami) ).ToString("F2").ToCharArray().Length; i++)
                {
                    satirlarinDizisi[36 + i + (9 - (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray().Length)] = (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray()[i];
                }

                satirlarinDizisi[45] = ' ';
                satirlarinDizisi[46] = 'T';
                satirlarinDizisi[47] = 'L';
                string str = new string(satirlarinDizisi);
                sb.Append(str + "\n");

            }
            #endregion satirlariyazdir

            string str2 = new string(ekstraArray);
            sb.Append(str2 + "\n");

            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            //ORTA ÇİZGİ ORTALA

            sb.Append(ESC + "a" + (char)1); // Align right
            sb.Append("Toplam: " + (fisItemList.Sum(x => (x.urunOzellikFiyatToplam + x.ekstraToplami) * x.urunAdedi).ToString("F2")) + " TL");
            sb.Append("\n");
            sb.Append(ESC + "a" + (char)1); // Align right


            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            // Feed and cut paper
            sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                TcpPrint(kasaYaziciIp, b);
                /*try
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrinterSettings.PrinterName = "KASA";
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            
                    pd.Print();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }*/
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                durum = false;
            }
            return durum;
        }

        [HttpPost]
        public JsonResult YazicilariAktifEt(int siparislerId)
        {
            #region 
            PersoneleYazdirilacak personeleYazdirilacakModel = new PersoneleYazdirilacak();
            foreach (var siparis in yazdiriciModel.Siparisler.Where(x=>x.siparislerId==siparislerId).ToList())
            {
                
                #region //EKSTRASIZLAR
                foreach (var siparisUrunu in yazdiriciModel.SiparisUrunleri.Where(x => /*x.odendi==false && */x.siparislerId == siparis.siparislerId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId).Count() == 0 /*&& x.onaylandi == false*/).GroupBy(x => x.urunOzellikId).ToList())
                {
                    EkstrasizYazdirilacak ekstrasizYazdirilacakModel = new EkstrasizYazdirilacak
                    {
                        katAd = siparis.Oturumlar.Masalar.Katlar.katAd,
                        masaAd = siparis.Oturumlar.Masalar.masaAd,
                        siparisTarihi = siparis.siparisTarihi,
                        personelAd = siparis.Personeller.personelAd,
                        adet = siparisUrunu.Count(),
                        urunOzellikAd = yazdiriciModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikAd,
                        urunAd = yazdiriciModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunAd,
                        departmanIp = yazdiriciModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.UrunKategoriler.Departmanlar.departmanIp
                    };
                    /*foreach (var a in kasaModel.SiparisUrunleri.Where(x => x.urunOzellikId == siparisUrunu.Key && x.siparislerId == siparis.siparislerId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId).Count() == 0).ToList())
                    {
                        a.onaylandi = true;
                    }*/
                    string departmanAd = yazdiriciModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.UrunKategoriler.Departmanlar.departmanAd;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                    bool ekstrasizReportYazdirilamadi = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                    try
                    {
                        EkstrasizReportYazdir(ekstrasizYazdirilacakModel, departmanAd);
                    }
                    catch (Exception error)
                    {
                        if (error.Message != null) ekstrasizReportYazdirilamadi = true;
                    }
                    //if (!ekstrasizReportYazdirilamadi) kasaModel.SaveChanges();
                    personeleYazdirilacakModel.ekstrasizYazdirilacak.Add(ekstrasizYazdirilacakModel);
                }
                #endregion
                #region //EKSTRALILAR
                foreach (var siparisUrunu3 in yazdiriciModel.SiparisUrunleri.Where(x => /*x.odendi == false &&*/ x.siparislerId == siparis.siparislerId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId).Count() > 0 /*&& x.onaylandi == false*/).ToList())
                {
                    EkstraliYazdirilacak ekstraliYazdirilacakModel = new EkstraliYazdirilacak
                    {
                        katAd = siparisUrunu3.Siparisler.Oturumlar.Masalar.Katlar.katAd,
                        masaAd = siparisUrunu3.Siparisler.Oturumlar.Masalar.masaAd,
                        siparisTarihi = siparisUrunu3.Siparisler.siparisTarihi,
                        personelAd = siparisUrunu3.Siparisler.Personeller.personelAd
                    };
                    ekstraliYazdirilacakModel.ekstralar.Clear();
                    foreach (var a in yazdiriciModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu3.siparisUrunlerId))
                    {
                        ekstraliYazdirilacakModel.ekstralar.Add(a.UrunEkstralar.urunEkstraAd);
                    }
                    ekstraliYazdirilacakModel.urunOzellikAd = siparisUrunu3.UrunOzellikler.urunOzellikAd;
                    ekstraliYazdirilacakModel.urunAd = siparisUrunu3.Urunler.urunAd;
                    ekstraliYazdirilacakModel.departmanIp = siparisUrunu3.Urunler.UrunKategoriler.Departmanlar.departmanIp;
                    //siparisUrunu3.onaylandi = true;
                    string departmanAd = siparisUrunu3.Urunler.UrunKategoriler.Departmanlar.departmanAd;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                    bool ekstraliReportYazdirilamadi = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                    try
                    {
                        EkstraliReportYazdir(ekstraliYazdirilacakModel, departmanAd);
                    }
                    catch (Exception error)
                    {
                        if (error.Message != null) ekstraliReportYazdirilamadi = true; 
                    }
                    //if(!ekstraliReportYazdirilamadi) kasaModel.SaveChanges();
                    personeleYazdirilacakModel.ekstraliYazdirilacak.Add(ekstraliYazdirilacakModel);
                }
                #endregion
                //kasaModel.SaveChanges();
                personeleYazdirilacakModel.personeleSiparisDetaylar.katAd = siparis.Oturumlar.Masalar.Katlar.katAd;
                personeleYazdirilacakModel.personeleSiparisDetaylar.masaAd = siparis.Oturumlar.Masalar.masaAd;
                personeleYazdirilacakModel.personeleSiparisDetaylar.personelAd = siparis.Personeller.personelAd;
                personeleYazdirilacakModel.personeleSiparisDetaylar.siparisNotu = siparis.note;
                personeleYazdirilacakModel.personeleSiparisDetaylar.siparisTarihi = siparis.siparisTarihi;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                bool personelReportYazdirilamadi = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

                if (personelYazicisiOtomatikMi)
                { 
                    try
                    {
                        PersoneleReportYazdir(personeleYazdirilacakModel.ekstraliYazdirilacak, personeleYazdirilacakModel.ekstrasizYazdirilacak, personeleYazdirilacakModel.personeleSiparisDetaylar);
                    }
                    catch (Exception error)
                    {
                        if (error.Message != null) personelReportYazdirilamadi = true;
                    }
                }
                //if(!personelReportYazdirilamadi) kasaModel.SaveChanges();

                personeleYazdirilacakModel.ekstrasizYazdirilacak.Clear();
                personeleYazdirilacakModel.ekstraliYazdirilacak.Clear();
                yazdiriciModel.Dispose();
            }
            #endregion
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public static void EkstrasizReportYazdir(EkstrasizYazdirilacak ekstrasizYazdirilacakModel, string departmanAd)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantDataBaseClass yazdiriciModel = new iRestaurantDataBaseClass();
            
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text
            #region SAAT TARİH
            //1.SATIR
            sb.Append(sirketAd + "\n");
            sb.Append(departmanAd + "\n");
            //1.SATIR
            //2.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstrasizYazdirilacakModel.siparisTarihi.ToString("dd.MM.yyyy")+ "\n");
            //2.SATIR
            //3.SATIR
            
            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstrasizYazdirilacakModel.siparisTarihi.ToString("hh:mm")+ "\n");
            //3.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            #endregion SAAT TARİH
            #region KAT MASA PERSONEL
            //4.SATIR
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Kat:" + ekstrasizYazdirilacakModel.katAd + " | ");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("Masa:" + ekstrasizYazdirilacakModel.masaAd);
            sb.Append(ESC + "a" + (char)2); // Align right
            //4.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("\n");
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            //5.SATIR
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Personel:" + ekstrasizYazdirilacakModel.personelAd + "\n");
            //5.SATIR
            #endregion KAT MASA PERSONEL
            #region ÜRÜN BİLGİSİ SİPNOT
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            //6.SATIR
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Ürün Adı:" + ekstrasizYazdirilacakModel.urunAd + " " + ekstrasizYazdirilacakModel.urunOzellikAd + "\n");
            sb.Append(ESC + "a" + (char)0); // Align left
                                            //6.SATIR

            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append(ekstrasizYazdirilacakModel.adet + "Adet" + "\n");
            sb.Append(ESC + "a" + (char)2); // Align right

            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            //ORTA ÇİZGİ ORTALA

            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Sipariş Notu:\n");
            sb.Append(ESC + "a" + (char)0); // Align left

            sb.Append(ekstrasizYazdirilacakModel.siparisNotu);
            #endregion ÜRÜN BİLGİSİ SİPNOT
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(57));
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(40) + Convert.ToChar(65) + Convert.ToChar(4) + Convert.ToChar(0) + Convert.ToChar(48) + Convert.ToChar(49) + Convert.ToChar(3) + Convert.ToChar(10));
            //sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                foreach (var departman in yazdiriciModel.Departmanlar.Where(x => x.departmanIp == ekstrasizYazdirilacakModel.departmanIp))
                {
                    if (departman.aktifMi == true)
                    {
                        TcpPrint(ekstrasizYazdirilacakModel.departmanIp, b);
                        
                        try
                        {
                            ProcessInfo processInfo = new ProcessInfo();
                            ProcessStartInfo i = new ProcessStartInfo("C:\\iis\\iis4446\\Yazici\\Yazici.exe", departmanAd);
                            Process p = new Process
                            {
                                StartInfo = i
                            };
                            p.Start();
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        

                        /*try
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = departman.departmanAd;
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }*/

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }
        public static void EkstraliReportYazdir(EkstraliYazdirilacak ekstraliYazdirilacakModel, string departmanAd)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantDataBaseClass yazdiriciModel = new iRestaurantDataBaseClass();
            #region SAAT DAKİKA
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            sb.Append("\n");
            // Add header text

            //1.SATIR
            sb.Append(sirketAd + "\n");
            sb.Append(departmanAd + "\n");
            //1.SATIR
            //2.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstraliYazdirilacakModel.siparisTarihi.ToString("dd.MM.yyyy") + "\n");
            //2.SATIR
            //3.SATIR
            
            
            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstraliYazdirilacakModel.siparisTarihi.ToString("hh:mm")+"\n");
            //sb.Append(saat + ":" + dakika + "\n");

            //3.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            #endregion SAAT TARİH
            #region KAT MASA PERSONEL
            //4.SATIR
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Kat:" + ekstraliYazdirilacakModel.katAd + " | ");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("Masa:" + ekstraliYazdirilacakModel.masaAd);
            sb.Append(ESC + "a" + (char)2); // Align right
            //4.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("\n");
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            //5.SATIR
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Personel:" + ekstraliYazdirilacakModel.personelAd + "\n");
            //5.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            #endregion KAT MASA PERSONEL
            //6.SATIR
            #region ÜRÜN ADI EKSTRALAR SİPNOT
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Ürün Adı:" + ekstraliYazdirilacakModel.urunAd + " " + ekstraliYazdirilacakModel.urunOzellikAd + "\n");
            //6.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Ekstralar:\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            foreach (var aaa in ekstraliYazdirilacakModel.ekstralar.ToList())
            {
                sb.Append(aaa + "\n");
            }
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("------------------------------------------------\n");
            sb.Append(ESC + "a" + (char)1); // Align center
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Sipariş Notu:\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(ekstraliYazdirilacakModel.siparisNotu);
            #endregion ÜRÜN ADI EKSTRALAR SİPNOT
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);

            // Feed and cut paper
            //sb.Append(ESC + Convert.ToChar("(")+Convert.ToChar("A") + Convert.ToChar() + Convert.ToChar() + Convert.ToChar() + Convert.ToChar())
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(57));
            //sb.Append(Convert.ToChar(27) + Convert.ToChar(40) + Convert.ToChar(65) + Convert.ToChar(4)+ Convert.ToChar(0) + Convert.ToChar(48) + Convert.ToChar(49) + Convert.ToChar(3) + Convert.ToChar(10));
            //sb.Append(GS + "V\x41\0");
            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                foreach (var departman in yazdiriciModel.Departmanlar.Where(x => x.departmanIp == ekstraliYazdirilacakModel.departmanIp).ToList())
                {
                    if (departman.aktifMi == true)
                    {
                        TcpPrint(ekstraliYazdirilacakModel.departmanIp, b);
                        try
                        {
                            ProcessInfo processInfo = new ProcessInfo();
                            ProcessStartInfo i = new ProcessStartInfo("C:\\iis\\iis4446\\Yazici\\Yazici.exe" /*"C:\\Programlar\\iRestaurant\\iRestaurant\\Yazici\\Yazici.exe"*/, departmanAd);
                            Process p = new Process
                            {
                                StartInfo = i
                            };
                            p.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        /*
                        try{
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = departman.departmanAd;
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        */
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void PersoneleReportYazdir(List<EkstraliYazdirilacak> ekstraliYazdirilacakModelList, List<EkstrasizYazdirilacak> ekstrasizYazdirilacakModelList, PersoneleYazdirilacak.PersoneleSiparisDetaylar personeleSiparisDetaylar)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantDataBaseClass yazdiriciModel = new iRestaurantDataBaseClass();
            // Initialize printer
            #region SAAT TARİH
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            //1.SATIR
            sb.Append(sirketAd+"\n");
            sb.Append("PERSONEL\n");
            //1.SATIR
            //2.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(personeleSiparisDetaylar.siparisTarihi.ToString("dd.MM.yyyy") + "\n");
            //2.SATIR
            
            //3.SATIR
            sb.Append(ESC + "a" + (char)2);
            
            sb.Append(personeleSiparisDetaylar.siparisTarihi.ToString("hh.mm") + "\n");
            //3.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            //sb.Append("________________________________________________\n");
            //ORTA ÇİZGİ ORTALA
            #endregion SAAT TARİH
            #region KAT MASA PERSONEL
            //4.SATIR
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Kat:" + personeleSiparisDetaylar.katAd + " | ");
            sb.Append(ESC + "a" + (char)0); // Align left

            sb.Append(ESC + "a" + (char)2); // Align right
            sb.Append("Masa:" + personeleSiparisDetaylar.masaAd);
            sb.Append(ESC + "a" + (char)2); // Align right
            //4.SATIR
            //ORTA ÇİZGİ ORTALA
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("\n");

            //ORTA ÇİZGİ ORTALA
            //5.SATIR
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Personel:" + personeleSiparisDetaylar.personelAd + "\n");
            sb.Append(ESC + "a" + (char)0); // Align left
                                            //5.SATIR
            #endregion KAT MASA PERSONEL
            #region ÜRÜNADI BİLGİSİ
            sb.Append(ESC + "a" + (char)1); // Align center
            sb.Append("________________________________________________\n");
            if (ekstrasizYazdirilacakModelList.Count() > 0)
            {
                //ORTA ÇİZGİ ORTALA

                sb.Append("|Ürün Adı|                      |Adet|          \n");
                //ORTA ÇİZGİ ORTALA
                char[] satirlarinDizisi = new char[48];
                char[] urunAdArray = new char[30];
                char[] ekstraArray = new char[48];
                char[] urunAdetArray = new char[3];
                char[] urunFiyatArray = new char[10];

                sb.Append(ESC + "a" + (char)0); // Align left

                foreach (var ekstrasizYazdirilacakModel in ekstrasizYazdirilacakModelList)
                {

                    for (int i = 0; i < (ekstrasizYazdirilacakModel.urunAd + ' ' + ekstrasizYazdirilacakModel.urunOzellikAd).ToCharArray().Length; i++)
                    {
                        satirlarinDizisi[i] = (ekstrasizYazdirilacakModel.urunAd + ' ' + ekstrasizYazdirilacakModel.urunOzellikAd).ToCharArray()[i];
                    }
                    for (int i = (ekstrasizYazdirilacakModel.urunAd + ' ' + ekstrasizYazdirilacakModel.urunOzellikAd).ToCharArray().Length; i < 33; i++)
                    {
                        satirlarinDizisi[i] = ' ';
                    }
                    satirlarinDizisi[33] = ' ';
                    satirlarinDizisi[34] = 'x';
                    for (int i = 0; i < ekstrasizYazdirilacakModel.adet.ToString().ToCharArray().Length; i++)
                    {
                        satirlarinDizisi[i + 35] = ekstrasizYazdirilacakModel.adet.ToString().ToCharArray()[i];
                    }
                    for (int i = 35 + ekstrasizYazdirilacakModel.adet.ToString().ToCharArray().Length; i < 38; i++)
                    {
                        satirlarinDizisi[i] = ' ';
                    }

                    for (int i = 38; i <= 47; i++)
                    {
                        satirlarinDizisi[i] = ' ';
                    }

                    string str = new string(satirlarinDizisi);
                    sb.Append(str + "\n");

                }
                sb.Append(ESC + "a" + (char)1); // Align center
                sb.Append("================================================\n");
                sb.Append(ESC + "a" + (char)1); // Align center
            }
            if (ekstraliYazdirilacakModelList.Count() > 0)
            {
                foreach (var ekstraliYazdirilacakModel in ekstraliYazdirilacakModelList)
                {
                    //ORTA ÇİZGİ ORTALA
                    if (ekstraliYazdirilacakModelList.Count() > 1)
                    {
                        sb.Append(ESC + "a" + (char)1); // Align center
                        sb.Append("-   -   -   -   -   -   -   -   -   -   -   -   \n");
                        sb.Append(ESC + "a" + (char)1); // Align center
                    }
                    //ORTA ÇİZGİ ORTALA
                    //6.SATIR
                    sb.Append(ESC + "a" + (char)0); // Align left
                    sb.Append("Ürün Adı: " + ekstraliYazdirilacakModel.urunAd + " " + ekstraliYazdirilacakModel.urunOzellikAd + "\n");
                    //6.SATIR

                    sb.Append(ESC + "a" + (char)0); // Align left
                    sb.Append("Ekstralar: ");
                    sb.Append(ESC + "a" + (char)0); // Align left
                    foreach (var aaa in ekstraliYazdirilacakModel.ekstralar.ToList())
                    {
                        sb.Append(aaa + " ");
                    }
                    sb.Append("\n");
                }
                sb.Append("================================================\n");
            }
            #endregion ÜRÜNADI BİLGİSİ
            #region DİPNOT
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append("Sipariş Notu:\n");
            sb.Append(ESC + "a" + (char)0); // Align left
            sb.Append(" " + personeleSiparisDetaylar.siparisNotu + "\n");
            // Revert to align left and normal weight
            sb.Append(ESC + "a" + (char)0);
            sb.Append(ESC + "E" + (char)0);
            #endregion DİPNOT
            
            // Feed and cut paper
            sb.Append(GS + "V\x41\0");

            byte[] a = Encoding.Default.GetBytes(sb.ToString());
            byte[] b = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);

            try
            {
                foreach (var departman in yazdiriciModel.Departmanlar.Where(x => x.departmanIp == personelIp).ToList())
                {
                    if (departman.aktifMi == true)
                    {
                        TcpPrint(personelIp, b);/*
                        try
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = departman.departmanAd;
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }*/
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }
        
        const char ESC = '\x1b';
        const char FS = '\x1c';
        const char GS = '\x1d';
        const char NUL = '\x00';
        static void TcpPrint(string host, byte[] data)
        {
            Socket s = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            s.Connect(host, 9100);
            s.Send(data);
            s.Disconnect(false);
        }
    }
}