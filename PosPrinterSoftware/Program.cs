using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace iRestaurantPosPrinterSoftware
{
    static class GirisBilgi
    {
        public static string sirketEmail { get; set; }
        public static string personelEmail { get; set; }
        public static string personelParola { get; set; }
        public static string dil { get; set; }
    }
    static class Program
    {
        //Database.Connection.ConnectionString.Replace("XXXXX", "12345");
        static string ayarlarYolu = "kullanici.ayarlar";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //GirisPanel girisPanel = new GirisPanel(null, null, null, null);
            //Application.Run(new GirisPanel(null, null, null, null));
            if (File.Exists(ayarlarYolu))
            {
                var lines = File.ReadAllLines(ayarlarYolu);
                string _sirketEmail = lines[0];
                string _personelEmail = lines[1];
                string _personelParola = lines[2];
                short _dil = Convert.ToInt16(lines[3].ToString());
                string girisDurumu = GirisYaptir(_sirketEmail, _personelEmail, _personelParola, true, _dil);
                if (girisDurumu.Contains("true_") || girisDurumu.Contains("false_"))
                {
                    //Anasayfa anasayfa = new Anasayfa(_sirketEmail, _personelEmail, _personelParola, Convert.ToInt16(_dil));
                    //Application.Run(new Anasayfa(_sirketEmail, _personelEmail, _personelParola, _dil));
                    
                    Thread anasayfaThread = new Thread(()=>OpenAnasayfa(_sirketEmail, _personelEmail, _personelParola, _dil));
                    Thread printerControlThread = new Thread(() => OpenPrinterControl(Convert.ToInt32(girisDurumu.Split('_')[1])));
                    anasayfaThread.Start();
                    printerControlThread.IsBackground = true;
;                   printerControlThread.Start();
                }
                else
                {
                    if (File.Exists(ayarlarYolu))
                    {
                        File.Delete(ayarlarYolu);
                    }
                    Application.Run(new GirisPanel(null, null, null, null));
                }
            }
            else
            {
                Application.Run(new GirisPanel(null, null, null, null));
            }
        }
        private static void OpenAnasayfa(string _sirketEmail, string _personelEmail, string _personelParola, short _dil)
        {
            Application.Run(new Anasayfa(_sirketEmail, _personelEmail, _personelParola, Convert.ToInt16(_dil)));
        }
        private static void OpenPrinterControl(int subeId)
        {
            new PrinterControl(subeId);
        }
        private static string GirisYaptir(string fonkSirketEmail, string fonkPersonelEmail, string fonkPersonelParola, bool fonkBeniHatirla, object fonkDil)
        {
            try
            {
                var webAddr = "http://localhost:62422/SubeYonetim/JsonGirisKontrol";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{" +
                        "'sirketEmail':'" + fonkSirketEmail + "'," +
                        "'personelEmail':'" + fonkPersonelEmail + "'," +
                        "'personelParola':'" + fonkPersonelParola + "'" +
                        "}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd().ToString();
                    //return result;
                    if (result.Contains("true_"))
                    {//Giriş bilgileri doğru.
                        return result.Replace(@"\", "").Replace("\"", "");
                    }
                    else
                    {//Hata bulundu, hatayı geri gönder giriş yapma.
                        MessageBox.Show(result);
                        return result.Replace(@"\", "").Replace("\"", "");
                    }

                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
    }
}
