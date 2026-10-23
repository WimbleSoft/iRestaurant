using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace iRestaurant.Security
{
    /// <summary>
    /// "Remember me" cookies that no longer contain e-mail addresses or password hashes.
    /// The cookie carries an encrypted and signed ticket (account id, expiry and a fingerprint of
    /// the current password hash) protected with the ASP.NET machine key, so changing the
    /// password invalidates every outstanding cookie.
    ///
    /// Never commit a &lt;machineKey&gt; to source control: a leaked key lets anyone forge these
    /// tickets. Leave it auto-generated, or provide it per environment for web farms.
    /// </summary>
    public static class RememberMeCookie
    {
        public enum Kind { Company, Employee }

        private const string Purpose = "iRestaurant.RememberMe.v1";

        private static string CookieName(Kind kind) => kind == Kind.Company ? "iRestaurant.RememberCompany" : "iRestaurant.RememberEmployee";

        public static void Issue(HttpContextBase context, Kind kind, int accountId, string passwordHash, TimeSpan lifetime)
        {
            DateTime expires = DateTime.UtcNow.Add(lifetime);
            string payload = string.Join("|", kind, accountId, expires.Ticks, Fingerprint(passwordHash));
            byte[] protectedBytes = MachineKey.Protect(Encoding.UTF8.GetBytes(payload), Purpose);

            context.Response.Cookies.Add(new HttpCookie(CookieName(kind), HttpServerUtility.UrlTokenEncode(protectedBytes))
            {
                Expires = expires,
                HttpOnly = true,
                Secure = context.Request.IsSecureConnection,
                Path = "/"
            });
        }

        /// <summary>Returns the account id from a valid cookie, or 0.</summary>
        public static int Read(HttpContextBase context, Kind kind, Func<int, string> currentPasswordHash)
        {
            HttpCookie cookie = context.Request.Cookies[CookieName(kind)];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return 0;

            try
            {
                byte[] raw = MachineKey.Unprotect(HttpServerUtility.UrlTokenDecode(cookie.Value), Purpose);
                string[] parts = Encoding.UTF8.GetString(raw).Split('|');
                int accountId;
                long ticks;
                if (parts.Length != 4 || parts[0] != kind.ToString()
                    || !int.TryParse(parts[1], out accountId) || !long.TryParse(parts[2], out ticks)
                    || new DateTime(ticks, DateTimeKind.Utc) < DateTime.UtcNow)
                    return 0;

                string hash = currentPasswordHash(accountId);
                return hash != null && PasswordHasher.FixedTimeEquals(Fingerprint(hash), parts[3]) ? accountId : 0;
            }
            catch (Exception)
            {
                // Tampered, truncated or issued with another machine key.
                return 0;
            }
        }

        public static void Clear(HttpContextBase context, Kind kind)
        {
            UserSession.ExpireCookie(context, CookieName(kind));
        }

        private static string Fingerprint(string passwordHash)
        {
            using (var sha = SHA256.Create())
                return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(passwordHash ?? string.Empty)), 0, 12);
        }
    }
}
