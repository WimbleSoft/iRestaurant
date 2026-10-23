using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace iRestaurant.Infrastructure
{
    /// <summary>
    /// Sends HTML e-mails through the SMTP account configured in Web.config.
    /// This used to be a public controller action, which turned the site into an open mail
    /// relay (anyone could send arbitrary mail from the company's Gmail account) and the
    /// account password was hard-coded in the source.
    /// Callers must HTML-encode any user-supplied text placed in <paramref name="htmlBody"/>.
    /// </summary>
    public static class EmailService
    {
        public static async Task<bool> SendAsync(string toAddress, string toName, string subject, string htmlBody)
        {
            if (string.IsNullOrEmpty(AppSettings.SmtpUserName) || string.IsNullOrEmpty(AppSettings.SmtpPassword))
            {
                Trace.TraceWarning("SMTP is not configured; e-mail to {0} was not sent.", toAddress);
                return false;
            }

            try
            {
                using (var message = new MailMessage())
                using (var smtp = new SmtpClient(AppSettings.SmtpHost, AppSettings.SmtpPort))
                {
                    message.From = new MailAddress(AppSettings.MailFromAddress, AppSettings.MailFromName);
                    message.To.Add(new MailAddress(toAddress, toName));
                    message.Subject = subject;
                    message.Body = htmlBody;
                    message.IsBodyHtml = true;

                    smtp.EnableSsl = AppSettings.SmtpEnableSsl;
                    smtp.Credentials = new NetworkCredential(AppSettings.SmtpUserName, AppSettings.SmtpPassword);
                    await smtp.SendMailAsync(message);
                }
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError("Sending e-mail failed: {0}", e);
                return false;
            }
        }
    }
}
