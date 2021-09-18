using MetroFramework.Forms;
using Rework;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace iRestaurantPosPrinterSoftware
{
    public partial class GirisPanel : MetroForm
    {
        string ayarlarYolu = "kullanici.ayarlar";
        public GirisPanel(string sirketEmail, string personelEmail, string personelParola, short? _dilValue)
        {
            InitializeComponent();
            dil.SelectedIndex = 0;
            if(sirketEmail != null && personelEmail != null && personelParola != null && _dilValue != null)
            {
                this.sirketEmail.Text=sirketEmail;
                this.personelEmail.Text=personelEmail;
                this.personelParola.Text="";
                this.beniHatirla.Checked = false;
                this.dil.SelectedIndex = _dilValue.Value;   
            }
            
        }

        private string girisYaptir(string fonkSirketEmail, string fonkPersonelEmail, string fonkPersonelParola, bool fonkBeniHatirla, object fonkDil)
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
                        if (fonkBeniHatirla == true)
                        {//Eğer beni hatırla tıklandıysa Giriş bilgilerini dosyaya kaydet.
                            FileStream fs = new FileStream(ayarlarYolu, FileMode.Create, FileAccess.Write);
                            if (fs.CanWrite)
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                stringBuilder.AppendLine(fonkSirketEmail);
                                stringBuilder.AppendLine(fonkPersonelEmail);
                                stringBuilder.AppendLine(fonkPersonelParola);
                                stringBuilder.AppendLine(dil.SelectedIndex.ToString());
                                byte[] buffer = Encoding.Default.GetBytes(stringBuilder.ToString());
                                fs.Write(buffer, 0, buffer.Length);
                            }

                            fs.Flush();
                            fs.Close();
                            return result;
                        }
                        else
                        {
                            if (File.Exists(ayarlarYolu))
                            {
                                File.Delete(ayarlarYolu);
                            }
                            return result;
                        }


                    }
                    else
                    {//Hata bulundu, hatayı geri gönder giriş yapma.
                        hataMesaji.Text = result;
                        return result;
                    }

                }
            }
            catch(Exception exception)
            {
                return exception.Message;
            }
            
        }

        private void girisYapButton_Click(object sender, EventArgs e)
        {
            //sirketEmail.Text;
            //personelEmail.Text;
            //personelParola.Text;
            //beniHatirla.Checked;
            //dil.SelectedValue;
            try
            {
                string girisYaniti = girisYaptir(sirketEmail.Text, personelEmail.Text, personelParola.Text.ToSHA(Crypto.SHA_Type.SHA256), beniHatirla.Checked, dil.SelectedIndex);
                if (girisYaniti.Contains("true_") || girisYaniti.Contains("false"))
                {
                    Anasayfa anasayfa = new Anasayfa(sirketEmail.Text, personelEmail.Text, personelParola.Text.ToSHA(Crypto.SHA_Type.SHA256), (short)dil.SelectedIndex);
                    
                    anasayfa.Show();
                    Hide();
                }
                else
                {
                    hataMesaji.Text = girisYaniti;
                }
            }
            catch (Exception exception)
            {
                hataMesaji.Text = exception.Message;
            }
            
            
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bütün kurallar burada yazıyor.");
        }

        private void kosullarKabul_CheckedChanged(object sender, EventArgs e)
        {
            if (kosullarKabul.Checked)
            {
                this.girisYapButton.Enabled = true;
            }
            else
            {
                this.girisYapButton.Enabled = false;
            }
        }

        private void GirisPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CefSharp.Cef.IsInitialized)
                CefSharp.Cef.Shutdown();
            Application.Exit();
        }
    }
}
