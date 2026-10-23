using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace iRestaurant.Infrastructure
{
    /// <summary>Minimal client for the Papara merchant payments API.</summary>
    public static class PaparaClient
    {
        public class Payment
        {
            public double amount { get; set; }
            public string referenceId { get; set; }
            public string orderDescription { get; set; }
            public string notificationUrl { get; set; }
            public string redirectUrl { get; set; }
        }

        public class PaymentData
        {
            public string merchantId { get; set; }
            public string userId { get; set; }
            /// <summary>0 = Papara balance, 1 = saved bank/credit card, 2 = mobile payment.</summary>
            public int paymentMethod { get; set; }
            public string paymentMethodDescription { get; set; }
            public string referenceId { get; set; }
            public string orderDescription { get; set; }
            /// <summary>0 = pending, 1 = paid, 2 = refunded by the merchant.</summary>
            public int status { get; set; }
            public string statusDescription { get; set; }
            public double amount { get; set; }
            public string currency { get; set; }
            public string notificationUrl { get; set; }
            public bool notificationDone { get; set; }
            public string redirectUrl { get; set; }
            public string merchantSecretKey { get; set; }
            public string paymentUrl { get; set; }
            public string returningRedirectUrl { get; set; }
            public string id { get; set; }
            public string createdAt { get; set; }
        }

        public class PaymentError
        {
            public string message { get; set; }
            public int code { get; set; }
        }

        public class PaymentResponse
        {
            public PaymentData data { get; set; }
            public bool succeeded { get; set; }
            public PaymentError error { get; set; }
        }

        public const int StatusPaid = 1;

        public static PaymentResponse CreatePayment(Payment payment)
        {
            return Send("POST", "/payments", new JavaScriptSerializer().Serialize(payment));
        }

        public static PaymentResponse GetPayment(string paymentId)
        {
            return Send("GET", "/payments?id=" + Uri.EscapeDataString(paymentId ?? string.Empty), null);
        }

        public static bool RefundPayment(string paymentId)
        {
            PaymentResponse response = Send("PUT", "/payments?id=" + Uri.EscapeDataString(paymentId ?? string.Empty), null);
            return response.succeeded;
        }

        private static PaymentResponse Send(string method, string path, string jsonBody)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(AppSettings.PaparaApiBaseUrl.TrimEnd('/') + path);
                request.Method = method;
                request.ContentType = "application/json";
                request.Headers.Add("ApiKey", AppSettings.PaparaApiKey);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.Timeout = 30000;

                if (jsonBody != null)
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                        writer.Write(jsonBody);
                }
                else if (method != "GET")
                {
                    request.ContentLength = 0;
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = new JavaScriptSerializer().Deserialize<PaymentResponse>(reader.ReadToEnd()) ?? new PaymentResponse();
                    if (method == "PUT")
                        result.succeeded = result.succeeded || response.StatusCode == HttpStatusCode.OK;
                    return result;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Papara {0} {1} failed: {2}", method, path, e);
                return new PaymentResponse
                {
                    succeeded = false,
                    data = new PaymentData(),
                    error = new PaymentError { code = 0, message = "The payment provider could not be reached." }
                };
            }
        }
    }
}
