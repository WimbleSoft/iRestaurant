using System;
using System.Configuration;

namespace iRestaurant.Infrastructure
{
    /// <summary>
    /// Strongly typed access to the &lt;appSettings&gt; section of Web.config.
    /// Secrets (SMTP password, payment provider keys) must never be committed: keep the values
    /// empty in source control and provide them per environment (config transform, IIS
    /// configuration editor, or environment-specific secret store).
    /// </summary>
    public static class AppSettings
    {
        public static string SmtpHost => Get("Smtp:Host", "smtp.gmail.com");
        public static int SmtpPort => GetInt("Smtp:Port", 587);
        public static bool SmtpEnableSsl => GetBool("Smtp:EnableSsl", true);
        public static string SmtpUserName => Get("Smtp:UserName");
        public static string SmtpPassword => Get("Smtp:Password");
        public static string MailFromAddress => Get("Mail:FromAddress", SmtpUserName);
        public static string MailFromName => Get("Mail:FromName", "iRestaurant");
        public static string ContactInboxAddress => Get("Mail:ContactInbox");

        public static string PaparaApiBaseUrl => Get("Papara:ApiBaseUrl", "https://merchantapi-test-master.papara.com");
        public static string PaparaApiKey => Get("Papara:ApiKey");
        public static string PaparaMerchantSecretKey => Get("Papara:MerchantSecretKey");

        /// <summary>
        /// Absolute base URL used in e-mails and payment callbacks (e.g. https://irestaurant.example.com).
        /// When empty, the current request's scheme and authority are used.
        /// </summary>
        public static string PublicBaseUrl => Get("App:PublicBaseUrl");

        /// <summary>Optional external helper that is launched after a kitchen ticket is printed.</summary>
        public static string PrinterHelperPath => Get("Printer:HelperExecutablePath");

        /// <summary>Header printed on cashier receipts and reports.</summary>
        public static string ReceiptHeader => Get("Printer:ReceiptHeader", "iRestaurant");

        private static string Get(string key, string fallback = "")
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }

        private static int GetInt(string key, int fallback)
        {
            int value;
            return int.TryParse(Get(key), out value) ? value : fallback;
        }

        private static bool GetBool(string key, bool fallback)
        {
            bool value;
            return bool.TryParse(Get(key), out value) ? value : fallback;
        }

        /// <summary>
        /// Base URL for links sent by e-mail and for payment callbacks. The Host header is
        /// attacker-controlled, so it is only trusted for local development requests; on a real
        /// server App:PublicBaseUrl must be configured, otherwise null is returned and no link is sent
        /// (prevents password-reset links pointing to an attacker's site).
        /// </summary>
        public static string ResolvePublicBaseUrl(System.Web.HttpRequestBase request)
        {
            string configured = PublicBaseUrl;
            if (!string.IsNullOrEmpty(configured))
                return configured.TrimEnd('/');
            return request.IsLocal ? request.Url.GetLeftPart(UriPartial.Authority) : null;
        }
    }
}
