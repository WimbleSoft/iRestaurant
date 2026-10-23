using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iRestaurant.Security;
using iRestaurant.Infrastructure;

namespace iRestaurant.Controllers
{
    /// <summary>Public website: marketing pages, company sign-up / sign-in, cart and checkout.</summary>
    public class HomeController : BaseController
    {
        private const double VatMultiplier = 1.18;
        private const int MaxCartQuantity = 100;
        private const string InvoiceReferencePrefix = "invoiceId_";
        private const string LegacyInvoiceReferencePrefix = "faturaId_";

        #region Models

        public class LoginModel
        {
            public string companyEmail { get; set; }
            public string companyPassword { get; set; }
            public string remember { get; set; }
            public string returnUrl { get; set; }
        }

        public class RecoverModel
        {
            public string companyEmail { get; set; }
            public string companyPassword { get; set; }
            public string companyPasswordRepeat { get; set; }
            public string activationCode { get; set; }
        }

        /// <summary>A service (license) in the shopping cart, kept in the session.</summary>
        [Serializable]
        public class CartService
        {
            public int serviceId { get; set; }
            public string serviceName { get; set; }
            public double serviceMonthlyFee { get; set; }
            public int currencyId { get; set; }
            public short serviceType { get; set; }
            public int quantity { get; set; }
            public bool isYearly { get; set; }
            public int? serviceCampaignId { get; set; }
        }

        private List<CartService> CurrentCart
        {
            get { return Session["cart"] as List<CartService> ?? new List<CartService>(); }
            set { Session["cart"] = value; }
        }

        #endregion

        #region Static pages

        public ActionResult About() => View();
        public ActionResult Career() => View();
        public ActionResult CareerDetail(int? careerId) => View();
        public ActionResult Error404() => View();
        public ActionResult Faq() => View();
        public ActionResult Features() => View();
        public ActionResult Index() => View();
        public ActionResult Testimonials() => View();
        public ActionResult Terms() => View();

        public ActionResult Pricing()
        {
            return View(new ViewModelDemoVM
            {
                ServiceCampaignServices = db.ServiceCampaignServices.ToList(),
                Services = db.Services.ToList(),
                ServiceCampaigns = db.ServiceCampaigns.ToList(),
                Currencies = db.Currencies.ToList()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Contact(string name, string email, string message)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
            {
                ViewBag.message = "Please fill in all fields.";
                return View("Index");
            }
            if (LoginThrottle.IsBlocked("contact/" + ClientAddress, null))
            {
                ViewBag.message = "You have sent too many messages. Please try again later.";
                return View("Index");
            }
            LoginThrottle.RegisterFailure("contact/" + ClientAddress, null); // counts messages per client, separately from logins

            string body =
                "Hello iRestaurant team,<br><br>" +
                "Sender name: " + HttpUtility.HtmlEncode(name) + "<br>" +
                "Sender e-mail: " + HttpUtility.HtmlEncode(email) + "<br><br>" +
                HttpUtility.HtmlEncode(message).Replace("\n", "<br>") +
                "<br><br>iRestaurant contact form";

            bool sent = !string.IsNullOrEmpty(AppSettings.ContactInboxAddress)
                        && await EmailService.SendAsync(AppSettings.ContactInboxAddress, "iRestaurant Support", "New message from the iRestaurant website", body);

            ViewBag.message = sent
                ? "Your message has been sent to the iRestaurant team. We will get back to you by e-mail as soon as possible."
                : "Something went wrong while sending your message. Please try again later.";
            return View("Index");
        }

        #endregion

        #region Sign in / sign up

        public ActionResult Login(LoginModel login)
        {
            if (TempData["Message"] != null)
                ViewBag.message = TempData["Message"];
            if (Request.HttpMethod != "POST" || login == null || string.IsNullOrWhiteSpace(login.companyEmail) || string.IsNullOrEmpty(login.companyPassword))
                return View();

            if (LoginThrottle.IsBlocked(login.companyEmail, ClientAddress))
            {
                ViewBag.message = ThrottledLoginError;
                return View();
            }

            Companies company = FindCompanyByCredentials(login.companyEmail, login.companyPassword);
            if (company == null)
            {
                LoginThrottle.RegisterFailure(login.companyEmail, ClientAddress);
                ViewBag.message = GenericLoginError;
                return View();
            }
            LoginThrottle.RegisterSuccess(login.companyEmail);

            if (!company.isActivated)
            {
                SignOutEverywhere();
                ViewBag.message = "Your account is waiting for e-mail verification.";
                return View();
            }

            UserSession.SignInCompany(HttpContext, company, AuthArea.Home);
            if (login.remember == "on")
                RememberMeCookie.Issue(HttpContext, RememberMeCookie.Kind.Company, company.companyId, company.companyPassword, TimeSpan.FromDays(3));
            return Redirect(SafeRedirect.Resolve(login.returnUrl, "Home", "Index"));
        }

        public ActionResult Logout()
        {
            SignOutEverywhere();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new ViewModelDemoVM { Countries = db.Countries.ToList() });
        }

        [HttpPost]
        public async Task<ActionResult> Register(string companyName, string companyEmail, string companyPhone, string companyPassword,
                                                 string companyPasswordRepeat, int? districtId, string terms)
        {
            var vm = new ViewModelDemoVM { Countries = db.Countries.ToList() };

            if (string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(companyEmail) || string.IsNullOrWhiteSpace(companyPhone)
                || string.IsNullOrEmpty(companyPassword) || string.IsNullOrEmpty(companyPasswordRepeat) || !districtId.HasValue || districtId <= 0)
            {
                ViewBag.message = "Please fill in all fields.";
                return View(vm);
            }
            if (terms != "on")
            {
                ViewBag.message = "You must accept the terms of use.";
                return View(vm);
            }
            if (companyPassword != companyPasswordRepeat)
            {
                ViewBag.message = "The passwords do not match.";
                return View(vm);
            }
            if (!PasswordHasher.IsAcceptablePassword(companyPassword))
            {
                ViewBag.message = "Your password must be between " + PasswordHasher.MinimumLength + " and " + PasswordHasher.MaximumLength + " characters.";
                return View(vm);
            }
            if (db.Companies.Any(x => x.companyEmail == companyEmail))
            {
                ViewBag.message = "This e-mail address is already in use. Please sign in or use another address.";
                return View(vm);
            }

            var company = new Companies
            {
                companyName = companyName.Trim(),
                companyEmail = companyEmail.Trim(),
                companyPhone = companyPhone.Trim(),
                districtId = districtId.Value,
                companyPassword = PasswordHasher.Hash(companyPassword),
                isActivated = false,
                isDeleted = false,
                activationCode = SecureRandom.HexToken(20)
            };

            string baseUrl = AppSettings.ResolvePublicBaseUrl(Request);
            if (baseUrl == null)
            {
                ViewBag.message = "Registration is not available right now. Please try again later.";
                return View(vm);
            }
            string link = baseUrl + "/Home/Validate?companyEmail=" + Uri.EscapeDataString(company.companyEmail)
                          + "&activationCode=" + company.activationCode;
            string body =
                "Hello " + HttpUtility.HtmlEncode(company.companyName) + ",<br>" +
                "You are receiving this e-mail because this address was used to sign up for iRestaurant by Sirus Software.<br>" +
                "Please verify your e-mail address by clicking the link below:<br>" +
                "<a href=\"" + HttpUtility.HtmlAttributeEncode(link) + "\">Verify my e-mail address</a><br>" +
                "If you did not sign up, you can ignore this e-mail; unverified companies are removed automatically after 30 days.<br>" +
                "Best regards, the iRestaurant team.";

            if (!await EmailService.SendAsync(company.companyEmail, company.companyName, "Your iRestaurant registration", body))
            {
                ViewBag.message = "We could not send you an e-mail. Please try again later.";
                return View(vm);
            }

            db.Companies.Add(company);
            db.SaveChanges();
            TempData["Message"] = "A verification e-mail has been sent. Please click the link in it before signing in.";
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Validate(string companyEmail, string activationCode)
        {
            if (string.IsNullOrEmpty(companyEmail) || string.IsNullOrEmpty(activationCode))
                return View("Login");

            Companies company = db.Companies.FirstOrDefault(x => x.companyEmail == companyEmail && !x.isActivated);
            if (company == null || !PasswordHasher.FixedTimeEquals(company.activationCode, activationCode))
            {
                ViewBag.message = "The verification code is invalid or your account has already been verified.";
                return View("Login");
            }

            company.isActivated = true;
            company.activationCode = SecureRandom.HexToken(20); // single use
            db.SaveChanges();
            ViewBag.message = "Your e-mail address has been verified. You can now sign in.";
            return View("Login");
        }

        public async Task<ActionResult> Recover(RecoverModel model)
        {
            model = model ?? new RecoverModel();
            bool hasEmail = !string.IsNullOrWhiteSpace(model.companyEmail);
            bool hasCode = !string.IsNullOrEmpty(model.activationCode);
            bool hasPasswords = !string.IsNullOrEmpty(model.companyPassword) || !string.IsNullOrEmpty(model.companyPasswordRepeat);

            // Step 1: e-mail only -> send a reset link. The answer is the same whether or not the
            // address exists, so the form cannot be used to discover registered companies.
            if (hasEmail && !hasCode && Request.HttpMethod == "POST")
            {
                Companies company = db.Companies.FirstOrDefault(x => x.companyEmail == model.companyEmail && !x.isDeleted);
                string baseUrl = AppSettings.ResolvePublicBaseUrl(Request);
                if (company != null && baseUrl != null && !LoginThrottle.IsBlocked("recover/" + model.companyEmail, ClientAddress))
                {
                    LoginThrottle.RegisterFailure("recover/" + model.companyEmail, ClientAddress);
                    company.activationCode = SecureRandom.HexToken(20);
                    db.SaveChanges();

                    string link = baseUrl + "/Home/Recover?companyEmail=" + Uri.EscapeDataString(company.companyEmail)
                                  + "&activationCode=" + company.activationCode;
                    string body =
                        "Hello " + HttpUtility.HtmlEncode(company.companyName) + ",<br>" +
                        "We received a request to reset the password of your iRestaurant company account.<br>" +
                        "<a href=\"" + HttpUtility.HtmlAttributeEncode(link) + "\">Click here to choose a new password.</a><br>" +
                        "If you did not request this, you can ignore this e-mail and keep using your current password.<br>" +
                        "Best regards, the iRestaurant team.";
                    await EmailService.SendAsync(company.companyEmail, company.companyName, "Password reset request", body);
                }

                ViewBag.isPasswordEntryRequired = false;
                ViewBag.message = "If this address belongs to an account, a reset link has been sent. Please also check your spam folder.";
                return View();
            }

            // Step 2: link opened -> show the new password form.
            if (hasEmail && hasCode && !hasPasswords)
            {
                ViewBag.isPasswordEntryRequired = true;
                ViewBag.activationCode = model.activationCode;
                ViewBag.companyEmail = model.companyEmail;
                return View();
            }

            // Step 3: new password submitted.
            if (hasEmail && hasCode && hasPasswords && Request.HttpMethod == "POST")
            {
                ViewBag.isPasswordEntryRequired = true;
                ViewBag.activationCode = model.activationCode;
                ViewBag.companyEmail = model.companyEmail;

                if (model.companyPassword != model.companyPasswordRepeat)
                {
                    ViewBag.message = "The passwords do not match. Please try again.";
                    return View();
                }
                if (!PasswordHasher.IsAcceptablePassword(model.companyPassword))
                {
                    ViewBag.message = "Your password must be between " + PasswordHasher.MinimumLength + " and " + PasswordHasher.MaximumLength + " characters.";
                    return View();
                }

                Companies company = db.Companies.FirstOrDefault(x => x.companyEmail == model.companyEmail && !x.isDeleted);
                if (company == null || !PasswordHasher.FixedTimeEquals(company.activationCode, model.activationCode))
                {
                    ViewBag.message = "The reset link is invalid or has already been used. Please request a new one.";
                    return View();
                }

                company.companyPassword = PasswordHasher.Hash(model.companyPassword);
                company.activationCode = SecureRandom.HexToken(20); // the link cannot be reused
                company.isActivated = true; // the reset link proves ownership of the e-mail address
                db.SaveChanges();
                SignOutEverywhere();
                ViewBag.message = "Your password has been changed. You can now sign in.";
                return View("Login");
            }

            return View();
        }

        private Companies FindCompanyByCredentials(string email, string password)
        {
            Companies company = db.Companies.FirstOrDefault(x => x.companyEmail == email && !x.isDeleted);
            bool needsRehash;
            if (company == null || !PasswordHasher.Verify(password, company.companyPassword, out needsRehash))
                return null;

            if (needsRehash)
            {
                company.companyPassword = PasswordHasher.Hash(password);
                db.SaveChanges();
            }
            return company;
        }

        #endregion

        #region Cart

        public ActionResult Cart()
        {
            return View(new ViewModelDemoVM
            {
                ServiceCampaignServices = db.ServiceCampaignServices.ToList(),
                ServiceCampaigns = db.ServiceCampaigns.ToList(),
                Services = db.Services.ToList(),
                Currencies = db.Currencies.ToList(),
                CartServices = CurrentCart
            });
        }

        public ActionResult AddCampaignToCart(int serviceCampaignId)
        {
            ServiceCampaigns campaign = db.ServiceCampaigns.Find(serviceCampaignId);
            if (campaign == null)
                return RedirectToAction("Cart", "Home");

            List<CartService> cart = CurrentCart;
            List<CartService> existing = cart.Where(x => x.serviceCampaignId == serviceCampaignId).ToList();
            if (existing.Count > 0)
            {
                foreach (CartService item in existing)
                    item.quantity = Math.Min(item.quantity + 1, MaxCartQuantity);
            }
            else
            {
                foreach (ServiceCampaignServices campaignService in campaign.ServiceCampaignServices)
                {
                    cart.Add(new CartService
                    {
                        serviceId = campaignService.serviceId,
                        serviceName = campaignService.Services.serviceName,
                        serviceMonthlyFee = campaignService.Services.serviceMonthlyFee,
                        currencyId = campaignService.Services.currencyId,
                        serviceType = campaignService.Services.serviceType,
                        isYearly = campaign.isYearly,
                        serviceCampaignId = serviceCampaignId,
                        quantity = 1
                    });
                }
            }
            CurrentCart = cart;
            return RedirectToAction("Cart", "Home");
        }

        public ActionResult AddItemToCart(int serviceId, string yearly)
        {
            bool isYearly = yearly == "True";
            List<CartService> cart = CurrentCart;
            CartService existing = cart.FirstOrDefault(x => x.serviceId == serviceId && x.isYearly == isYearly && x.serviceCampaignId == null);
            if (existing != null)
            {
                existing.quantity = Math.Min(existing.quantity + 1, MaxCartQuantity);
            }
            else
            {
                Services service = db.Services.Find(serviceId);
                if (service != null)
                {
                    cart.Add(new CartService
                    {
                        serviceId = service.serviceId,
                        serviceName = service.serviceName,
                        serviceMonthlyFee = service.serviceMonthlyFee,
                        currencyId = service.currencyId,
                        serviceType = service.serviceType,
                        isYearly = isYearly,
                        serviceCampaignId = null,
                        quantity = 1
                    });
                }
            }
            CurrentCart = cart;
            return RedirectToAction("Cart", "Home");
        }

        /// <summary>Removing one campaign decreases the quantity of all of its services by one.</summary>
        public ActionResult RemoveCampaignFromCart(int serviceCampaignId, string yearly)
        {
            bool isYearly = yearly == "True";
            List<CartService> cart = CurrentCart;
            foreach (CartService item in cart.Where(x => x.serviceCampaignId == serviceCampaignId && x.isYearly == isYearly))
                item.quantity -= 1;
            cart.RemoveAll(x => x.quantity <= 0);
            CurrentCart = cart;
            return RedirectToAction("Cart", "Home");
        }

        public ActionResult RemoveItemFromCart(int serviceId, string yearly)
        {
            bool isYearly = yearly == "True";
            List<CartService> cart = CurrentCart;
            cart.RemoveAll(x => x.serviceId == serviceId && x.isYearly == isYearly && x.serviceCampaignId == null);
            CurrentCart = cart;
            return RedirectToAction("Cart", "Home");
        }

        public JsonResult ChangeCampaignQuantityFromCart(int quantity, int serviceCampaignId)
        {
            List<CartService> cart = CurrentCart;
            List<CartService> items = cart.Where(x => x.serviceCampaignId == serviceCampaignId).ToList();
            if (items.Count == 0)
                return JsonCode(cart.Count == 0 ? 3 : 2);

            foreach (CartService item in items)
                item.quantity = ClampQuantity(quantity);
            cart.RemoveAll(x => x.quantity <= 0);
            CurrentCart = cart;
            return JsonCode(Success);
        }

        public JsonResult ChangeItemQuantityFromCart(int serviceId, string yearly, int quantity, int? serviceCampaignId)
        {
            bool isYearly = yearly == "True";
            List<CartService> cart = CurrentCart;
            if (cart.Count == 0)
                return JsonCode(3);

            List<CartService> items = serviceCampaignId.HasValue
                ? cart.Where(x => x.isYearly == isYearly && x.serviceCampaignId == serviceCampaignId.Value).ToList()
                : cart.Where(x => x.serviceId == serviceId && x.isYearly == isYearly && x.serviceCampaignId == null).ToList();
            if (items.Count == 0)
                return JsonCode(2);

            foreach (CartService item in items)
                item.quantity = ClampQuantity(quantity);
            cart.RemoveAll(x => x.quantity <= 0);
            CurrentCart = cart;
            return JsonCode(Success);
        }

        private static int ClampQuantity(int quantity) => Math.Max(0, Math.Min(quantity, MaxCartQuantity));

        #endregion

        #region Checkout and payment

        [RequireLogin(AuthArea.Home, AuthArea.CompanyAdmin)]
        public ActionResult Checkout()
        {
            if (CurrentCart.Count == 0)
                return RedirectToAction("Cart", "Home");
            return View(BuildCheckoutViewModel());
        }

        public ActionResult CheckoutFinished()
        {
            if (CurrentCart.Count == 0)
                return RedirectToAction("Cart", "Home");
            return View(BuildCheckoutViewModel());
        }

        private ViewModelDemoVM BuildCheckoutViewModel()
        {
            int companyId = CurrentCompanyId;
            return new ViewModelDemoVM
            {
                ServiceCampaignServices = db.ServiceCampaignServices.ToList(),
                Services = db.Services.ToList(),
                ServiceCampaigns = db.ServiceCampaigns.ToList(),
                Currencies = db.Currencies.ToList(),
                CartServices = CurrentCart,
                Countries = db.Countries.ToList(),
                Districts = db.Districts.ToList(),
                Provinces = db.Provinces.ToList(),
                Companies = db.Companies.Where(x => x.companyId == companyId).ToList()
            };
        }

        /// <summary>Turns the cart into an unpaid invoice and redirects to the Papara payment page.</summary>
        [HttpPost]
        [RequireLogin(AuthArea.Home, AuthArea.CompanyAdmin)]
        public ActionResult Buy(int? districtId, string companyAddress)
        {
            List<CartService> cart = CurrentCart;
            if (cart.Count == 0)
                return RedirectToAction("Cart", "Home");

            int companyId = CurrentCompanyId;
            Invoices invoice = db.Invoices.FirstOrDefault(x => !x.isPaid && x.companyId == companyId);
            if (invoice == null)
            {
                invoice = db.Invoices.Add(new Invoices { invoiceDate = DateTime.Now, isPaid = false, companyId = companyId });
            }
            else
            {
                // An unpaid invoice is re-used; its contents are replaced by the current cart.
                db.InvoiceServices.RemoveRange(db.InvoiceServices.Where(x => x.invoiceId == invoice.invoiceId));
            }

            foreach (CartService item in cart)
            {
                db.InvoiceServices.Add(new InvoiceServices
                {
                    Invoices = invoice,
                    serviceId = item.serviceId,
                    quantity = item.quantity,
                    isYearly = item.isYearly,
                    serviceCampaignId = item.serviceCampaignId,
                    currencyId = item.currencyId
                });
            }
            db.SaveChanges();

            string baseUrl = AppSettings.ResolvePublicBaseUrl(Request);
            if (baseUrl == null)
            {
                ViewBag.message = "Payments are not configured on this server.";
                return View("Error404");
            }
            PaparaClient.PaymentResponse response = PaparaClient.CreatePayment(new PaparaClient.Payment
            {
                amount = CalculateInvoiceTotal(invoice.invoiceId),
                notificationUrl = baseUrl + "/Home/BuyNotification",
                redirectUrl = baseUrl + "/Home/BuyRedirect",
                orderDescription = "iRestaurant service licenses",
                referenceId = InvoiceReferencePrefix + invoice.invoiceId
            });

            if (!response.succeeded || response.data == null || string.IsNullOrEmpty(response.data.paymentUrl))
            {
                ViewBag.message = "The payment could not be started. Please try again later.";
                return View("Error404");
            }
            return Redirect(response.data.paymentUrl);
        }

        /// <summary>The customer's browser comes back here from Papara.</summary>
        [HttpGet]
        [RequireLogin(AuthArea.Home, AuthArea.CompanyAdmin)]
        public ActionResult BuyRedirect(string paymentId)
        {
            // Never trust the query string: ask Papara what really happened.
            PaparaClient.PaymentResponse payment = PaparaClient.GetPayment(paymentId);
            PaymentOutcome outcome = payment.succeeded && payment.data != null
                ? ProcessPayment(payment.data, CurrentCompanyId)
                : PaymentOutcome.NotPaid;

            if (outcome == PaymentOutcome.Paid || outcome == PaymentOutcome.AlreadyProcessed)
                CurrentCart = null;
            ViewBag.message = OutcomeMessage(outcome);
            return View("CheckoutFinished", BuildCheckoutViewModel());
        }

        /// <summary>
        /// Server-to-server notification from Papara. Previously this trusted the posted body, so
        /// anyone could mark an invoice as paid and get free licenses. Now the merchant secret must
        /// match and the payment is re-fetched from Papara before anything is changed.
        /// </summary>
        [HttpPost]
        [AllowWithoutAntiForgeryToken]
        public ActionResult BuyNotification(PaparaClient.PaymentData paymentData)
        {
            string secret = AppSettings.PaparaMerchantSecretKey;
            if (paymentData == null || string.IsNullOrEmpty(secret) || !PasswordHasher.FixedTimeEquals(secret, paymentData.merchantSecretKey ?? string.Empty))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            PaparaClient.PaymentResponse payment = PaparaClient.GetPayment(paymentData.id);
            if (!payment.succeeded || payment.data == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            ProcessPayment(payment.data, null);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private enum PaymentOutcome { Paid, AlreadyProcessed, NotPaid, UnknownInvoice, AmountMismatchRefunded, AmountMismatchRefundFailed }

        /// <summary>
        /// Marks the invoice paid and grants its licenses. Idempotent, because both the redirect
        /// and the notification end up here for the same payment.
        /// </summary>
        private PaymentOutcome ProcessPayment(PaparaClient.PaymentData payment, int? expectedCompanyId)
        {
            int invoiceId = ParseInvoiceId(payment.referenceId);
            Invoices invoice = invoiceId == 0 ? null : db.Invoices.Find(invoiceId);
            if (invoice == null || (expectedCompanyId.HasValue && invoice.companyId != expectedCompanyId.Value))
                return PaymentOutcome.UnknownInvoice;

            if (payment.status != PaparaClient.StatusPaid)
                return PaymentOutcome.NotPaid;

            if (invoice.isPaid)
                return PaymentOutcome.AlreadyProcessed;

            double expected = CalculateInvoiceTotal(invoice.invoiceId);
            if (Math.Abs(payment.amount - expected) > 0.009)
            {
                Trace.TraceWarning("Payment {0} for invoice {1} has amount {2}, expected {3}; refunding.", payment.id, invoice.invoiceId, payment.amount, expected);
                return PaparaClient.RefundPayment(payment.id) ? PaymentOutcome.AmountMismatchRefunded : PaymentOutcome.AmountMismatchRefundFailed;
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                // The redirect and the notification can arrive at the same time: only the request
                // that flips isPaid from 0 to 1 grants the licenses.
                int claimed = db.Database.ExecuteSqlCommand(
                    "UPDATE [dbo].[Invoices] SET [isPaid] = 1 WHERE [invoiceId] = @p0 AND [isPaid] = 0", invoice.invoiceId);
                if (claimed == 0)
                    return PaymentOutcome.AlreadyProcessed;

                GrantLicenses(invoice, expected);
                transaction.Commit();
            }
            return PaymentOutcome.Paid;
        }

        private void GrantLicenses(Invoices invoice, double amountPaid)
        {
            invoice.isPaid = true;
            DateTime now = DateTime.Now;
            foreach (InvoiceServices item in invoice.InvoiceServices.ToList())
            {
                for (int i = 0; i < Math.Max(1, item.quantity); i++)
                {
                    invoice.ServiceLicenses.Add(new ServiceLicenses
                    {
                        invoiceId = invoice.invoiceId,
                        serviceId = item.serviceId,
                        branchId = null,
                        serviceLicenseStartDate = now,
                        serviceLicenseEndDate = item.isYearly ? now.AddYears(1) : now.AddMonths(1)
                    });
                }
            }
            db.InvoicePayments.Add(new InvoicePayments
            {
                invoiceId = invoice.invoiceId,
                invoicePaymentAmount = amountPaid,
                invoicePaymentDate = now,
                currencyId = invoice.InvoiceServices.Select(x => x.currencyId).FirstOrDefault()
            });
            db.SaveChanges();
        }

        /// <summary>Invoice total including VAT, computed from the invoice's own rows only.</summary>
        private double CalculateInvoiceTotal(int invoiceId)
        {
            List<InvoiceServices> items = db.InvoiceServices.Where(x => x.invoiceId == invoiceId).ToList();

            // Fees are looked up explicitly: rows added in this request are not proxies, so their
            // Services navigation property is not loaded.
            List<int> serviceIds = items.Select(x => x.serviceId).Distinct().ToList();
            Dictionary<int, double> monthlyFees = db.Services.Where(x => serviceIds.Contains(x.serviceId))
                                                    .ToDictionary(x => x.serviceId, x => x.serviceMonthlyFee);

            double subtotal = items
                .Where(x => x.serviceCampaignId == null)
                .Sum(x => Math.Round(monthlyFees[x.serviceId], 2) * x.quantity * (x.isYearly ? 12 : 1));

            foreach (var campaign in items.Where(x => x.serviceCampaignId != null).GroupBy(x => x.serviceCampaignId.Value))
            {
                ServiceCampaigns serviceCampaign = db.ServiceCampaigns.Find(campaign.Key);
                if (serviceCampaign != null)
                    subtotal += Math.Round(serviceCampaign.serviceCampaignPrice, 2) * campaign.First().quantity;
            }
            return Math.Round(subtotal * VatMultiplier, 2);
        }

        private static int ParseInvoiceId(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
                return 0;
            string digits = referenceId.StartsWith(InvoiceReferencePrefix, StringComparison.Ordinal) ? referenceId.Substring(InvoiceReferencePrefix.Length)
                          : referenceId.StartsWith(LegacyInvoiceReferencePrefix, StringComparison.Ordinal) ? referenceId.Substring(LegacyInvoiceReferencePrefix.Length)
                          : null;
            int id;
            return digits != null && int.TryParse(digits, out id) ? id : 0;
        }

        private static string OutcomeMessage(PaymentOutcome outcome)
        {
            switch (outcome)
            {
                case PaymentOutcome.Paid:
                case PaymentOutcome.AlreadyProcessed:
                    return "Your invoice has been paid. You can manage the licenses you bought from the company admin panel.";
                case PaymentOutcome.AmountMismatchRefunded:
                    return "The amount paid did not match your invoice, so the payment was refunded to your Papara account. Please try again.";
                case PaymentOutcome.AmountMismatchRefundFailed:
                    return "The amount paid did not match your invoice and the automatic refund failed. Please contact us.";
                case PaymentOutcome.UnknownInvoice:
                    return "We could not find the invoice for this payment. Please contact us.";
                default:
                    return "The payment was not completed. Your cart is still available; please try again.";
            }
        }

        #endregion
    }
}
