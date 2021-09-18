using MetroFramework.Forms;
using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Linq;

namespace iRestaurantPosPrinterSoftware
{
    public partial class Anasayfa : MetroForm
    {
        public ChromiumWebBrowser browser;
        public string sirketEmail;
        public string personelEmail;
        public string personelParola;
        public short dil;
        Subeler sube;

        string ayarlarpath = "kullanici.ayarlar";
        private void InitializeChromium(string sirketEmail, string personelEmail, string personelParola)
        {
            CefSettings settings = new CefSettings();
            if(!Cef.IsInitialized)
            Cef.Initialize(settings);
            

            //subeYonetimBrowser = new ChromiumWebBrowser("http://localhost:62422/SubeYonetim/YoneticiGiris?returnUrl=Anasayfa&sirketEmail=" + sirketEmail + "&personelEmail=" + personelEmail + "&personelParola=" + personelParola + "&webView=true");
            browser = new ChromiumWebBrowser("http://localhost:62422/Kasa/YoneticiGiris?returnUrl=Kasa&sirketEmail=" + sirketEmail + "&personelEmail=" + personelEmail + "&personelParola=" + personelParola + "&webView=true");
            browser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled; 
            browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            //this.Controls.Add(subeYonetimBrowser);
            htmlPanel.Controls.Add(browser);
            //subeYonetimBrowser.Dock = DockStyle.Fill;
            browser.Dock = DockStyle.Fill;

            
        }

        public Anasayfa(string sirketEmail, string personelEmail, string personelParola, short dil)
        {
            this.sirketEmail = sirketEmail;
            this.personelEmail = personelEmail;
            this.personelParola = personelParola;
            this.dil = dil;
            InitializeComponent();
            InitializeChromium(sirketEmail, personelEmail, personelParola);
            browser.RegisterJsObject("BrowserMethods", new BrowserMethods(browser, this));
            try
            {
                iRestaurantEntities iRestaurant = new iRestaurantEntities();
                Personeller personel = iRestaurant.Personeller.Where(x => x.personelEmail == personelEmail && x.personelParola == personelParola).First();

                sube = iRestaurant.Subeler.Where(x => x.subeId == personel.subeId).First();
            }
            catch
            {

            }
        }

        private void oturumuKapat_Click(object sender, EventArgs e)
        {
            if(File.Exists(ayarlarpath))
            {
                File.Delete(ayarlarpath);
            }
            GirisPanel girisPanel = new GirisPanel(sirketEmail,personelEmail,personelParola, dil);
            //GirisPanel girisPanel = (GirisPanel)Application.OpenForms["GirisPanel"];
            girisPanel.Show();
            Hide();

        }

        private void Anasayfa_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
            
            Application.Exit();
        }

        private void kasaPaneliButon_Click(object sender, EventArgs e)
        {
            browser.Visible = true;
            ayarlarPanel.Visible = false;
            browser.Load("http://localhost:62422/Kasa/YoneticiGiris?returnUrl=Kasa&sirketEmail=" + sirketEmail + "&personelEmail=" + personelEmail + "&personelParola=" + personelParola + "&webView=true");
            //new ChromiumWebBrowser("http://localhost:62422/Kasa/YoneticiGiris?returnUrl=Kasa&sirketEmail=" + sirketEmail + "&personelEmail=" + personelEmail + "&personelParola=" + personelParola + "&webView=true");
            browser.ShowDevTools();
        }

        private void yonetimPaneliButon_Click_1(object sender, EventArgs e)
        {
            browser.Visible = true;
            ayarlarPanel.Visible = false;
            browser.Load("http://localhost:62422/SubeYonetim/YoneticiGiris?returnUrl=Anasayfa&sirketEmail=" + sirketEmail + "&personelEmail=" + personelEmail + "&personelParola=" + personelParola + "&webView=true");
           
            //new ChromiumWebBrowser("http://localhost:62422/SubeYonetim/YoneticiGiris?returnUrl=Anasayfa&sirketEmail=" + sirketEmail + "&personelEmail=" + personelEmail + "&personelParola=" + personelParola + "&webView=true");

        }

        private void ayarlarButon_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                browser.Hide();
                ayarlarPanel.Show();
                iRestaurantEntities iRestaurant = new iRestaurantEntities();
                Subeler sube = iRestaurant.Subeler.Where(x => x.subeId == this.sube.subeId).First();
                if (sube.duyuruAlaniAktifMi)
                {
                    duyuruAlaniAktifMi.CheckState = CheckState.Checked;
                }
                else
                {
                    duyuruAlaniAktifMi.CheckState = CheckState.Unchecked;
                }
                if (sube.personelYazicisiOtomatikMi)
                {
                    personelYazicisiOtomatikMi.CheckState = CheckState.Checked;
                }
                else
                {
                    personelYazicisiOtomatikMi.CheckState = CheckState.Unchecked;
                }
                kasaYaziciAd.Text = sube.kasaYaziciAd;
                kasaYaziciIp.Text = sube.kasaYaziciIp;
                personelYaziciAd.Text = sube.personelYaziciAd;
                personelYaziciIp.Text = sube.personelYaziciIp;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ayarlar paneli gösterilirken bir hata oluştu. Sistemden otomatik olarak çıkış yapılacak.");
                Console.WriteLine(ex.Message);
            }
            
        }

        private void kaydetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                iRestaurantEntities iRestaurant = new iRestaurantEntities();
                var kaydedilecekSube = iRestaurant.Subeler.Where(x => x.subeId == sube.subeId).First();
                kaydedilecekSube.duyuruAlaniAktifMi = duyuruAlaniAktifMi.Checked;
                kaydedilecekSube.personelYazicisiOtomatikMi = personelYazicisiOtomatikMi.Checked;
                kaydedilecekSube.personelYaziciIp = personelYaziciIp.Text;
                kaydedilecekSube.personelYaziciAd = personelYaziciAd.Text;
                kaydedilecekSube.kasaYaziciAd = kasaYaziciAd.Text;
                kaydedilecekSube.kasaYaziciIp = kasaYaziciIp.Text;
                iRestaurant.SaveChanges();
                MessageBox.Show("Kaydetme İşlemi Başarılı");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
            }
            
        }
    }
}
