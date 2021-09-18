using CefSharp.WinForms;
using iRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static iRestaurantPosPrinterSoftware.PrinterControl;

namespace iRestaurantPosPrinterSoftware
{
    class BrowserMethods
    {
        iRestaurantEntities iRestaurant = new iRestaurantEntities();
        private static ChromiumWebBrowser _instanceBrowser = null;
        
        private static Anasayfa _instanceMainForm = null;

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
            sb.Append("-----" + baslangicTarihi.ToString("dd/MM/yyyy HH:mm") + "  --  " + bitisTarihi.ToString("dd/MM/yyyy HH:mm") + "-----\n");
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
                string personelMiktar = personelPerformans.personelMiktar.ToString("F2");
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
                for (int i = personelMiktar.ToString().ToCharArray().Length; i > 0; i--)
                {
                    personelSatirlarinDizisi[35 + i + (9 - personelMiktar.ToString().ToCharArray().Length)] = personelMiktar.ToString().ToCharArray()[i - 1];
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
            string yeni = Convert.ToString(b);
            try
            {
                //TcpPrint(kasaYaziciIp, b);
                System.Windows.Forms.MessageBox.Show("Kasadan döküm yazdırıldı");
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
            sb.Append("\n");
            //1.SATIR
            //2.SATIR
            sb.Append(ESC + "a" + (char)2);
            sb.Append(DateTime.Now.ToShortDateString() + "\n");
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
                for (int i = 0; i < (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray().Length; i++)
                {
                    satirlarinDizisi[36 + i + (9 - (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray().Length)] = (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray()[i];
                }

                satirlarinDizisi[45] = ' ';
                satirlarinDizisi[46] = 'T';
                satirlarinDizisi[47] = 'L';
                string str = new string(satirlarinDizisi);
                sb.Append(str + "\n");

            }
            #endregion satiryazdirma

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
                //TcpPrint(kasaYaziciIp, b);
                System.Windows.Forms.MessageBox.Show("Kasadan fiş yazdırıldı");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                durum = false;
            }
            return durum;
        }
        public bool HesapYazdir(List<HesapItem> fisItemList, List<string> personeller, string masaAd)
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
            for (int i = 0; i < 5; i++)
            {
                masaVeTarihSatiri[i] = "MASA:"[i];
            }
            for (int i = 0; i < masaAd.Length; i++)
            {
                masaVeTarihSatiri[i + 5] = masaAd[i];
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
            for (int i = 0; i < "PERSONEL: ".Length; i++)
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
                personelVeSaatSatiri[i + 9] = personelString[i];
            }

            for (int i = 0; i < 5; i++)
            {
                personelVeSaatSatiri[i + 43] = DateTime.Now.ToShortTimeString()[i];
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
                for (int i = 0; i < (fisItemi.urunAdedi * (fisItemi.urunOzellikFiyatToplam + fisItemi.ekstraToplami)).ToString("F2").ToCharArray().Length; i++)
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
                //TcpPrint(kasaYaziciIp, b);
                System.Windows.Forms.MessageBox.Show("Kasadan hesap yazdırıldı");
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
        public BrowserMethods(ChromiumWebBrowser browser, Anasayfa anasayfaForm)
        {
            _instanceBrowser = browser;
            _instanceMainForm = anasayfaForm;
        }

        public void adisyonFisiYazdir(string oturumId2, string subeId2)
        {
            int oturumId = Convert.ToInt32(oturumId2);
            int subeId = Convert.ToInt32(subeId2);

            List<HesapItem> hesapItemList = new List<HesapItem>();
            //EKSTRASIZLAR - KAMPANYASIZ
            foreach (var siparisUrunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.SiparisUrunleriEkstralari.Count() == 0 && x.oturumKampanyaId == null).GroupBy(x => x.urunOzellikId))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = iRestaurant.SiparisUrunleri.Where(x => x.urunOzellikId == siparisUrunu.Key && x.Siparisler.oturumId == oturumId).FirstOrDefault().Siparisler.Personeller.personelAd,
                    urunAdedi = siparisUrunu.Count(),
                    urunAd = iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunAd + " - " + iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikAd,
                    ekstraToplami = 0,
                    urunOzellikFiyatToplam = iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunFiyat + iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikFiyat
                };
                hesapItemList.Add(hesapItem_);
            }
            //EKSTRALILAR - KAMPANYASIZ
            foreach (var siparisUrunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.SiparisUrunleriEkstralari.Count() > 0 && x.oturumKampanyaId == null))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = siparisUrunu.Siparisler.Personeller.personelAd,
                    urunAdedi = 1,
                    urunAd = siparisUrunu.Urunler.urunAd + " - " + siparisUrunu.UrunOzellikler.urunOzellikAd
                };
                foreach (var siparisUrunEkstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId))
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
            foreach (var siparisUrunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.oturumKampanyaId != null).GroupBy(x => x.oturumKampanyaId))
            {
                HesapItem hesapItem_ = new HesapItem
                {
                    personelAd = iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Siparisler.Personeller.personelAd,
                    urunAdedi = 1,
                    urunAd = iRestaurant.OturumKampanyalari.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Kampanyalar.kampanyaAd,
                    ekstraToplami = 0
                };
                foreach (var siparisUrunEkstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.oturumId == oturumId && x.SiparisUrunleri.oturumKampanyaId == siparisUrunu.Key))
                {
                    hesapItem_.ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                }
                double ozellikToplam = iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
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
                hesapItem_.urunOzellikFiyatToplam = iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat) + iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().OturumKampanyalari.Kampanyalar.kampanyaFiyat;
                hesapItemList.Add(hesapItem_);
            }
            List<String> personelListesi = new List<String>();
            foreach (var personel in hesapItemList.GroupBy(x => x.personelAd).ToList())
            {
                personelListesi.Add(personel.Key);
            }
            string masaAd = iRestaurant.Oturumlar.Where(x => x.oturumId == oturumId).FirstOrDefault().Masalar.masaAd;
            //Veritabanını Kaydet

            bool yazdirmaDurumu = HesapYazdir(hesapItemList, personelListesi, masaAd);
        }

        public void hesapGonder(string oturumId2, string subeId2)
        {
            //oturumId = Convert.ToInt32(oturumId);
            try
            {
                int oturumId = Convert.ToInt32(oturumId2);
                int subeId = Convert.ToInt32(subeId2);
                List<HesapItem> hesapItemList = new List<HesapItem>();
                //EKSTRASIZLAR - KAMPANYASIZ
                foreach (var siparisUrunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi == false && x.SiparisUrunleriEkstralari.Count() == 0 && x.oturumKampanyaId == null).GroupBy(x => x.urunOzellikId))
                {
                    HesapItem hesapItem_ = new HesapItem
                    {
                        personelAd = iRestaurant.SiparisUrunleri.Where(x => x.urunOzellikId == siparisUrunu.Key && x.Siparisler.oturumId == oturumId).FirstOrDefault().Siparisler.Personeller.personelAd,
                        urunAdedi = siparisUrunu.Count(),
                        urunAd = iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunAd + " - " + iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikAd,
                        ekstraToplami = 0,
                        urunOzellikFiyatToplam = iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().Urunler.urunFiyat + iRestaurant.UrunOzellikler.Where(x => x.urunOzellikId == siparisUrunu.Key).FirstOrDefault().urunOzellikFiyat
                    };
                    hesapItemList.Add(hesapItem_);
                }
                //EKSTRALILAR - KAMPANYASIZ
                foreach (var siparisUrunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi == false && x.SiparisUrunleriEkstralari.Count() > 0 && x.oturumKampanyaId == null))
                {
                    HesapItem hesapItem_ = new HesapItem
                    {
                        personelAd = siparisUrunu.Siparisler.Personeller.personelAd,
                        urunAdedi = 1,
                        urunAd = siparisUrunu.Urunler.urunAd + " - " + siparisUrunu.UrunOzellikler.urunOzellikAd
                    };
                    foreach (var siparisUrunEkstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisUrunu.siparisUrunlerId))
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
                foreach (var siparisUrunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.odendi == false && x.oturumKampanyaId != null).GroupBy(x => x.oturumKampanyaId))
                {
                    HesapItem hesapItem_ = new HesapItem
                    {
                        personelAd = iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.oturumId == oturumId && x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Siparisler.Personeller.personelAd,
                        urunAdedi = 1,
                        urunAd = iRestaurant.OturumKampanyalari.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().Kampanyalar.kampanyaAd,
                        ekstraToplami = 0
                    };
                    foreach (var siparisUrunEkstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.oturumId == oturumId && x.odendi == false && x.SiparisUrunleri.oturumKampanyaId == siparisUrunu.Key))
                    {
                        hesapItem_.ekstraToplami += siparisUrunEkstrasi.UrunEkstralar.urunEkstraFiyat;
                    }
                    double ozellikToplam = iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
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
                    hesapItem_.urunOzellikFiyatToplam = iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).Sum(x => x.UrunOzellikler.urunOzellikFiyat) + iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == siparisUrunu.Key).FirstOrDefault().OturumKampanyalari.Kampanyalar.kampanyaFiyat;
                    hesapItemList.Add(hesapItem_);
                }
                List<String> personelListesi = new List<String>();
                foreach (var personel in hesapItemList.GroupBy(x => x.personelAd).ToList())
                {
                    personelListesi.Add(personel.Key);
                }
                string masaAd = iRestaurant.Oturumlar.Where(x => x.oturumId == oturumId).FirstOrDefault().Masalar.masaAd;


                bool yazdirmaDurumu = HesapYazdir(hesapItemList, personelListesi, masaAd);
                if (yazdirmaDurumu)
                {
                    iRestaurant.Oturumlar.Where(x => x.oturumId == oturumId).ToList().FirstOrDefault().hesapIstendi = false;
                    iRestaurant.SaveChanges();
                }
            }
            catch
            {

            }
        }

        public void dokumAl(string baslangicTarihi2, string bitisTarihi2, string subeId2)
        {
            int subeId = Convert.ToInt32(subeId2);
            DateTime baslangicTarihi = Convert.ToDateTime(baslangicTarihi2);
            DateTime bitisTarihi = Convert.ToDateTime(bitisTarihi2);
            List<PersonelPerformans> personelPerformansList = new List<PersonelPerformans>();
            List<OdemeYontemiDokum> odemeYontemiDokumuList = new List<OdemeYontemiDokum>();
            bitisTarihi = bitisTarihi.AddSeconds(59).AddMilliseconds(999);
            #region // PERSONELPERFORMANS
            foreach (var personel in iRestaurant.Personeller.ToList())
            {
                PersonelPerformans personelPerformans = new PersonelPerformans();
                double personelToplamMiktar = 0;
                double toplamMiktar = 0;

                //PERSONELE AİT TOPLAM 

                //KAMPANYASIZ SİPARİŞ ÜRÜNLERİNİN FİYATLARI, ÜRÜNLERİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var siparisurunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    personelToplamMiktar += siparisurunu.UrunOzellikler.urunOzellikFiyat; //Personele ait Kampanyasız girilen bütün özellikler toplamı
                    personelToplamMiktar += siparisurunu.Urunler.urunFiyat; //Personele ait Kampanyasız girilen ürünlerin fiyatını toplar
                    foreach (var siparisurunekstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisurunu.siparisUrunlerId).ToList())
                    {
                        personelToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Personele ait Kampanyasız girilen bütün ekstraların toplamı
                    }

                }

                //\\//\\

                //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var urunGrubu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    // Aşağıdaki satır Personele ait Kampanyalı girilen ürünlerin aynı kampanyaya ait olanlarını gruplayıp, o gruba ait ürünlerin ürün özelliklerin toplar
                    personelToplamMiktar += iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId == urunGrubu.Key && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).Sum(x => x.UrunOzellikler.urunOzellikFiyat);

                    foreach (var siparisurunekstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.Siparisler.personelId == personel.personelId && x.SiparisUrunleri.oturumKampanyaId == urunGrubu.Key).ToList())
                    {
                        personelToplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Personele ait Kampanyalı girilen bütün ekstraların toplamı
                    }

                }
                foreach (var urunGrubu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    personelToplamMiktar += iRestaurant.OturumKampanyalari.Where(x => x.oturumKampanyaId == urunGrubu.Key).FirstOrDefault().Kampanyalar.kampanyaFiyat; //Aynı Kampanyaya ait ürünleri gruplar ve kampanyalı fiyatını ekler
                }
                /*
                foreach(var siparisurunu in iRestaurant.SiparisUrunleri.Where(x => x.Siparisler.personelId == personel.personelId && x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    personelToplamMiktar += siparisurunu.Urunler.urunFiyat; //Kampanyasız girilen ürünlerin fiyatını toplar
                }
                */
                //PERSONELLER TOPLAMI

                //KAMPANYASIZ SİPARİŞ ÜRÜNLERİNİN FİYATLARI, ÜRÜNLERİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var siparisurunu in iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    toplamMiktar += siparisurunu.UrunOzellikler.urunOzellikFiyat; //Tüm Personele ait bütün özellikler toplamı
                    toplamMiktar += siparisurunu.Urunler.urunFiyat; // Tüm Personele ait bütün ürün fiyatlar toplamı
                    foreach (var siparisurunekstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.siparisUrunlerId == siparisurunu.siparisUrunlerId).ToList())
                    {
                        toplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Tüm Personele ait bütün ekstraların toplamı
                    }

                }

                //\\//\\

                //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN ÖZELLİKLERİ VE EKSTRALARI TOPLAMI
                foreach (var urunGrubu in iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    //Aşağıdaki satır Tüm Personele ait Kampanyalı girilen ürünlerin aynı kampanyaya ait olanlarını gruplayıp, o gruba ait ürünlerin ürün özelliklerin toplar
                    toplamMiktar += iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == urunGrubu.Key && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).Sum(x => x.UrunOzellikler.urunOzellikFiyat);
                    foreach (var siparisurunekstrasi in iRestaurant.SiparisUrunleriEkstralari.Where(x => x.SiparisUrunleri.oturumKampanyaId == urunGrubu.Key).ToList())
                    {
                        toplamMiktar += siparisurunekstrasi.UrunEkstralar.urunEkstraFiyat;  //Tüm Personele ait bütün ekstraların toplamı
                    }
                }
                //KAMPANYALI SİPARİŞ ÜRÜNLERİNİN KAMPANYA FİYATLARI TOPLAMI
                foreach (var urunGrubu in iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId != null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).GroupBy(x => x.oturumKampanyaId).ToList())
                {
                    toplamMiktar += iRestaurant.OturumKampanyalari.Where(x => x.oturumKampanyaId == urunGrubu.Key).FirstOrDefault().Kampanyalar.kampanyaFiyat; //Aynı Kampanyaya ait ürünleri gruplar ve kampanyalı fiyatını ekler
                }
                /*
                foreach (var siparisurunu in iRestaurant.SiparisUrunleri.Where(x => x.oturumKampanyaId == null && x.odenmeTarihi >= baslangicTarihi && x.odenmeTarihi <= bitisTarihi).ToList())
                {
                    toplamMiktar += siparisurunu.Urunler.urunFiyat; //Kampanyasız girilen ürünlerin fiyatını toplar
                }
                */
                personelPerformans.personelId = personel.personelId;
                personelPerformans.personelAd = personel.personelAd;
                personelPerformans.personelMiktar = personelToplamMiktar;
                personelPerformans.personelYuzde = personelToplamMiktar * 100 / toplamMiktar;
                personelPerformansList.Add(personelPerformans);
            }
            #endregion
            #region // ODEMEYONTEMIDOKUMU
            double toplamOdeme = 0;
            foreach (var odeme in iRestaurant.Odemeler.Where(x => x.odemeTarihi >= baslangicTarihi && x.odemeTarihi <= bitisTarihi))
            {
                toplamOdeme += odeme.odemeMiktar;
            }
            foreach (var odeme in iRestaurant.Odemeler.Where(x => x.odemeTarihi >= baslangicTarihi && x.odemeTarihi <= bitisTarihi).GroupBy(x => x.OdemeTurleri.odemeTurAd))
            {
                OdemeYontemiDokum odemeYontemi = new OdemeYontemiDokum();
                /*
                string yontemAdi = "";
                switch (odeme.Key)
                {
                    case 1:
                        yontemAdi = "Nakit";
                        break;
                    case 2:
                        yontemAdi = "Kredi Kartı";
                        break;
                    case 3:
                        yontemAdi = "İkram";
                        break;
                    case 4:
                        yontemAdi = "Ticket";
                        break;
                    case 5:
                        yontemAdi = "Multinet";
                        break;
                    case 6:
                        yontemAdi = "Veresiye";
                        break;
                }
                */
                odemeYontemi.yontemAd = /*yontemAdi*/odeme.Key;
                odemeYontemi.yontemMiktar = odeme.Sum(x => x.odemeMiktar);
                odemeYontemi.yontemYuzde = Convert.ToDouble(odemeYontemi.yontemMiktar * 100 / toplamOdeme);


                odemeYontemiDokumuList.Add(odemeYontemi);
            }
            #endregion
            bool yazdirmadurumu = DokumYazdir(personelPerformansList, odemeYontemiDokumuList, baslangicTarihi, bitisTarihi);
            
        }
    }
}
