using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace iRestaurantPosPrinterSoftware
{
    /// <summary>Result of signing in against the iRestaurant web server.</summary>
    public class LoginResult
    {
        public bool Success { get; set; }
        public int BranchId { get; set; }
        /// <summary>Single-use token that opens the web panels in the embedded browser.</summary>
        public string Token { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Talks to the iRestaurant web server. The password is sent as typed (use an HTTPS ServerUrl
    /// in production) and is never put in a URL; the server answers with a one-time token instead.
    /// </summary>
    public static class ServerApi
    {
        [DataContract]
        private class LoginRequest
        {
            [DataMember] public string companyEmail { get; set; }
            [DataMember] public string employeeEmail { get; set; }
            [DataMember] public string employeePassword { get; set; }
        }

        public static string BaseUrl
        {
            get
            {
                string configured = ConfigurationManager.AppSettings["ServerUrl"];
                return (string.IsNullOrWhiteSpace(configured) ? "http://localhost:62422" : configured.Trim()).TrimEnd('/');
            }
        }

        public static LoginResult Login(string companyEmail, string employeeEmail, string password)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(BaseUrl + "/BranchAdmin/JsonCheckLogin");
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Timeout = 15000;

                var serializer = new DataContractJsonSerializer(typeof(LoginRequest));
                using (Stream body = request.GetRequestStream())
                    serializer.WriteObject(body, new LoginRequest { companyEmail = companyEmail, employeeEmail = employeeEmail, employeePassword = password });

                string response;
                using (var http = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(http.GetResponseStream()))
                    response = reader.ReadToEnd().Trim().Trim('"');

                // Success looks like "true_<branchId>_<token>"; anything else is a message for the user.
                string[] parts = response.Split('_');
                int branchId;
                if (parts.Length == 3 && parts[0] == "true" && int.TryParse(parts[1], out branchId))
                    return new LoginResult { Success = true, BranchId = branchId, Token = parts[2] };

                return new LoginResult { Success = false, Message = response };
            }
            catch (Exception e)
            {
                return new LoginResult { Success = false, Message = "The server could not be reached: " + e.Message };
            }
        }

        /// <summary>URL that signs the embedded browser into a panel ("Cashier" or "BranchAdmin").</summary>
        public static string PanelUrl(string panel, string token)
        {
            return BaseUrl + "/" + panel + "/Login?webView=true&loginToken=" + Uri.EscapeDataString(token ?? string.Empty);
        }
    }

    /// <summary>
    /// "Remember me" storage for the POS app. Credentials are encrypted with Windows DPAPI for the
    /// current Windows user, instead of the previous plain-text file holding the password hash.
    /// </summary>
    public static class CredentialStore
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "iRestaurant", "credentials.dat");
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("iRestaurant.PosPrinterSoftware");

        public static void Save(string companyEmail, string employeeEmail, string password, int language)
        {
            string plain = string.Join("\n", companyEmail, employeeEmail, password, language.ToString());
            byte[] encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(plain), Entropy, DataProtectionScope.CurrentUser);
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllBytes(FilePath, encrypted);
        }

        public static bool TryLoad(out string companyEmail, out string employeeEmail, out string password, out short language)
        {
            companyEmail = employeeEmail = password = null;
            language = 0;
            try
            {
                if (!File.Exists(FilePath))
                    return false;
                byte[] plain = ProtectedData.Unprotect(File.ReadAllBytes(FilePath), Entropy, DataProtectionScope.CurrentUser);
                string[] lines = Encoding.UTF8.GetString(plain).Split('\n');
                if (lines.Length != 4 || !short.TryParse(lines[3], out language))
                    return false;
                companyEmail = lines[0];
                employeeEmail = lines[1];
                password = lines[2];
                return true;
            }
            catch (CryptographicException)
            {
                Delete();
                return false;
            }
        }

        public static void Delete()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);

            // Remove the legacy plain-text file written by older versions.
            foreach (string legacy in new[] { "kullanici.ayarlar", "kullanici.settings" })
            {
                if (File.Exists(legacy))
                    File.Delete(legacy);
            }
        }
    }
}
