using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace iRestaurantPosPrinterSoftware
{
    class PrinterControl 
    {
        private static string ayarlarYolu = "kullanici.ayarlar";
        iRestaurantEntities iRestaurant = new iRestaurantEntities();
        public PrinterControl(int subeId)
        {
            MessageBox.Show("Yazdırma programı çalışmaya başladı.");
            while (true)
            {
                Thread.Sleep(5000);
                iRestaurantEntities iRestaurantYazici = new iRestaurantEntities();
                personelYaziciIp = "";
                kasaYaziciIp = "";
                foreach (var siparis in iRestaurantYazici.YazdirmaListesi.Where(x => x.subeId == subeId))
                {
                    try
                    {
                        YazicilariAktifEt(siparis.siparislerId);
                        iRestaurantYazici.YazdirmaListesi.RemoveRange(iRestaurantYazici.YazdirmaListesi.Where(x => x.siparislerId == siparis.siparislerId));
                        
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                iRestaurantYazici.SaveChanges();
                iRestaurantYazici.Dispose();
                Thread.Sleep(5000);
            }
        }

        #region // SINIFLAR
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

        public static string personelYaziciIp = "192.168.1.3";
        public static string kasaYaziciIp = "192.168.1.4";

        #endregion


        

        public void YazicilariAktifEt(int siparislerId)
        {
            #region 
            PersoneleYazdirilacak personeleYazdirilacakModel = new PersoneleYazdirilacak();
            iRestaurantEntities kasaModel = new iRestaurantEntities();
            foreach (var siparis in kasaModel.Siparisler.Where(x => x.siparislerId == siparislerId).ToList())
            {

                #region //EKSTRASIZLAR
                foreach (var siparisUrunu in kasaModel.SiparisUrunleri.Where(x => /*x.odendi==false && */x.siparislerId == siparis.siparislerId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId).Count() == 0 /*&& x.onaylandi == false*/).GroupBy(x => x.urunOzellikId).ToList())
                {
                    EkstrasizYazdirilacak ekstrasizYazdirilacakModel = new EkstrasizYazdirilacak
                    {
                        katAd = siparis.Oturumlar.Masalar.Katlar.katAd,
                        masaAd = siparis.Oturumlar.Masalar.masaAd,
                        siparisTarihi = siparis.siparisTarihi,
                        personelAd = siparis.Personeller.personelAd,
                        adet = siparisUrunu.Count(),
                        urunOzellikAd = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikAd,
                        urunAd = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunAd,
                        departmanIp = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.UrunKategoriler.Departmanlar.departmanIp
                    };
                    /*foreach (var a in kasaModel.SiparisUrunleri.Where(x => x.urunOzellikId == siparisUrunu.Key && x.siparislerId == siparis.siparislerId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId).Count() == 0).ToList())
                    {
                        a.onaylandi = true;
                    }*/
                    string departmanAd = kasaModel.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.UrunKategoriler.Departmanlar.departmanAd;
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
                foreach (var siparisUrunu3 in kasaModel.SiparisUrunleri.Where(x => /*x.odendi == false &&*/ x.siparislerId == siparis.siparislerId && x.SiparisUrunleriEkstralari.Where(y => y.siparisUrunlerId == x.siparisUrunlerId).Count() > 0 /*&& x.onaylandi == false*/).ToList())
                {
                    EkstraliYazdirilacak ekstraliYazdirilacakModel = new EkstraliYazdirilacak
                    {
                        katAd = siparisUrunu3.Siparisler.Oturumlar.Masalar.Katlar.katAd,
                        masaAd = siparisUrunu3.Siparisler.Oturumlar.Masalar.masaAd,
                        siparisTarihi = siparisUrunu3.Siparisler.siparisTarihi,
                        personelAd = siparisUrunu3.Siparisler.Personeller.personelAd
                    };
                    ekstraliYazdirilacakModel.ekstralar.Clear();
                    foreach (var a in kasaModel.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu3.siparisUrunlerId))
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
                try
                {
                    PersoneleReportYazdir(personeleYazdirilacakModel.ekstraliYazdirilacak, personeleYazdirilacakModel.ekstrasizYazdirilacak, personeleYazdirilacakModel.personeleSiparisDetaylar);
                }
                catch (Exception error)
                {
                    if (error.Message != null) personelReportYazdirilamadi = true;
                }
                //if(!personelReportYazdirilamadi) kasaModel.SaveChanges();

                personeleYazdirilacakModel.ekstrasizYazdirilacak.Clear();
                personeleYazdirilacakModel.ekstraliYazdirilacak.Clear();
                kasaModel.Dispose();
                Thread.Sleep(1000);
            }
            #endregion
            //return Json(true, JsonRequestBehavior.AllowGet);
        }

        public static void EkstrasizReportYazdir(EkstrasizYazdirilacak ekstrasizYazdirilacakModel, string departmanAd)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantEntities yazdiriciModel = new iRestaurantEntities();

            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            // Add header text
            #region SAAT TARİH
            //1.SATIR
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append(departmanAd + "\n");
            //1.SATIR
            //2.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstrasizYazdirilacakModel.siparisTarihi.ToString("dd.MM.yyyy") + "\n");
            //2.SATIR
            //3.SATIR

            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstrasizYazdirilacakModel.siparisTarihi.ToString("hh:mm") + "\n");
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
            #region ÜRÜN BİLGİSİ DİPNOT
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
            byte[] yazilacakByte = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                foreach (var departman in yazdiriciModel.Departmanlar.Where(x => x.departmanIp == ekstrasizYazdirilacakModel.departmanIp))
                {
                    if (departman.aktifMi == true)
                    {
                        try
                        {
                            TcpPrint(ekstrasizYazdirilacakModel.departmanIp, yazilacakByte);
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = departman.departmanAd;
                            pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
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
            iRestaurantEntities yazdiriciModel = new iRestaurantEntities();
            #region SAAT DAKİKA
            // Initialize printer
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            sb.Append("\n");
            // Add header text

            //1.SATIR
            sb.Append("Kodes Cafe & Bistro\n");
            sb.Append(departmanAd + "\n");
            //1.SATIR
            //2.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstraliYazdirilacakModel.siparisTarihi.ToString("dd.MM.yyyy") + "\n");
            //2.SATIR
            //3.SATIR


            sb.Append(ESC + "a" + (char)2);
            sb.Append(ekstraliYazdirilacakModel.siparisTarihi.ToString("hh:mm") + "\n");
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
            #region ÜRÜN ADI EKSTRALAR SİPNOT
            //6.SATIR
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
            byte[] yazilacakByte = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);
            try
            {
                foreach (var departman in yazdiriciModel.Departmanlar.Where(x => x.departmanIp == ekstraliYazdirilacakModel.departmanIp).ToList())
                {
                    if (departman.aktifMi == true)
                    {
                        try
                        {
                            TcpPrint(ekstraliYazdirilacakModel.departmanIp, yazilacakByte);
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = departman.departmanAd;
                            pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
                            pd.Print();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void PersoneleReportYazdir(List<EkstraliYazdirilacak> ekstraliYazdirilacakModelList, List<EkstrasizYazdirilacak> ekstrasizYazdirilacakModelList, PersoneleYazdirilacak.PersoneleSiparisDetaylar personeleSiparisDetaylar)
        {
            StringBuilder sb = new StringBuilder();
            iRestaurantEntities yazdiriciModel = new iRestaurantEntities();
            // Initialize printer
            #region SAAT TARİH
            sb.Append(ESC + "@");
            sb.Append(ESC + "t" + (char)79);
            // Align center
            sb.Append(ESC + "a" + (char)1);
            //1.SATIR
            sb.Append("Kodes Cafe & Bistro\n");
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
            byte[] yazilacakByte = Encoding.Convert(Encoding.Default, Encoding.GetEncoding(857), a);

            try
            {
                foreach (var departman in yazdiriciModel.Departmanlar.Where(x => x.departmanIp == personelYaziciIp).ToList())
                {
                    if (departman.aktifMi == true)
                    {
                        //TcpPrint(personelYaziciIp, yazilacakByte);
                        /*
                        try
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = departman.departmanAd;
                            pd.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        public static void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            
        }

        public const char ESC = '\x1b';
        public const char FS = '\x1c';
        public const char GS = '\x1d';
        public const char NUL = '\x00';
        public static void TcpPrint(string host, byte[] data)
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
