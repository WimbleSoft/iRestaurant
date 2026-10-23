# Security review

This document records the security problems found in iRestaurant and how each one was addressed.
Severity is from the point of view of an internet-facing deployment.

## Action required by the maintainers

These cannot be fixed in code because the secrets are already public in the git history:

1. **Revoke the Gmail app password** that was hard-coded in `HomeController` (account used for
   outgoing mail) and create a new one. Anyone with access to the repository can send mail as that
   account.
2. **Rotate the Papara API key and merchant secret** that were hard-coded in `HomeController`.
3. Put the new values only in the deployed `Web.config` (or a config transform kept out of source
   control). The committed `Web.config` now contains empty placeholders.
4. Review the database for unexpected `.aspx`/`.ashx`/`.config` files under
   `WebServer/assets/*-images/` on any server that ran the old code (see finding 2), and for
   invoices marked paid without a matching Papara payment (finding 5).

## Findings and fixes

### Critical

1. **Every AJAX/JSON endpoint was effectively unauthenticated.** The `*AsyncFunctionRoleControl`
   attributes implemented `OnActionExecuted`, i.e. they ran *after* the action had already
   changed data, and only replaced the response body with `-1`. Anyone could, without signing in,
   add/delete products, staff, tables or payments, mark orders paid, change permissions, etc.
   *Fix:* `Security/RequireLoginAttribute` is an authorization filter that runs before the action.
   Pages redirect to the right login page; JSON endpoints answer `-1` without executing.

2. **Remote code execution through image uploads.** Product images were saved under their
   original file name, campaign images with the client's file extension, and announcement images
   with an extension taken from the client-supplied `Content-Type` (which also allowed path
   traversal). Uploading `shell.aspx` put an executable page in the web root. Combined with
   finding 1 this needed no account at all.
   *Fix:* `Security/ImageUpload` accepts only JPEG/PNG/GIF files whose content matches the file
   signature, chooses the file name on the server and refuses paths outside the target folder.
   Each upload folder also has a `web.config` that disables script handlers and serves only image
   extensions.

3. **Cross-tenant data access (IDOR) everywhere.** Endpoints compared a client-supplied
   `branchId`/`companyId` with the session and then loaded the target record by its id alone, so a
   manager of one restaurant could edit or delete records of any other restaurant (products,
   employees, branches, licenses, payments, ...). Several reports and counters also aggregated data
   of all tenants.
   *Fix:* the tenant always comes from the session, and every record is loaded through an
   "owned by the current branch/company" lookup (`Own...`/`FindOwn...` helpers). Referenced ids on
   create/update (category, floor, menu, product, payment type, ...) are checked the same way.

4. **Pass-the-hash and credential leaks.** Passwords were unsalted SHA-256 hashes, and:
   - the "remember me" cookies stored the e-mail and the password hash in plain text (not
     `HttpOnly`), and re-login only compared the stored hash, so the hash *was* the password;
   - `Login?webView=true` accepted the hash directly from the query string;
   - `*AsyncAttributeCookieSessionLogin` methods were public actions that logged in with a hash;
   - the POS app hashed on the client and sent the hash in URLs.

   *Fix:* `Security/PasswordHasher` uses salted PBKDF2 (100k iterations) and upgrades legacy
   hashes on the next successful login. Remember-me cookies (`Security/RememberMeCookie`) carry an
   encrypted, signed ticket (account id, expiry, password fingerprint) protected by the machine key
   and are `HttpOnly`. The POS app sends the password once over the API and receives a single-use,
   two-minute token (`Security/OneTimeLoginTokens`) to open the panels. Its saved credentials are
   encrypted with Windows DPAPI instead of a plain text file.

5. **Forged payment notifications.** `Home/BuyNotification` trusted the posted body, so anyone
   could mark an invoice as paid and receive licenses. `BuyRedirect` used a hard-coded invoice id
   (`16`), and the expected amount was summed over the services of *all* invoices.
   *Fix:* the notification must carry the configured merchant secret, the payment is re-fetched
   from Papara, its status must be "paid", the amount must match the invoice's own total, and
   processing is idempotent: an atomic `UPDATE ... WHERE isPaid = 0` inside a transaction ensures
   the redirect and the notification cannot both grant licenses. `BuyRedirect` also checks that the invoice belongs to the signed-in
   company.

6. **Open mail relay.** `HomeController.Email(...)` was a public action: anyone could send HTML
   e-mail with any recipient, subject and body through the company's Gmail account.
   *Fix:* e-mail sending is in `Infrastructure/EmailService`, not reachable by URL. User input in
   e-mails (contact form, names) is HTML-encoded.

### High

7. **Publicly callable printer methods and shared printer state.** `YazdiriciController` exposed
   `DokumYazdir`, `FisYazdir`, `HesapYazdir`, `Initialize` and `YazicilariAktifEt` as anonymous
   URLs, and kept printer IPs in `static` fields shared by all tenants (one tenant's request could
   redirect another tenant's receipts). It also opened raw TCP connections to any address stored
   by a tenant (SSRF) and started an external program with unquoted, tenant-controlled arguments.
   *Fix:* `Infrastructure/ReceiptPrinter` is a per-request class (not a controller), only prints to
   private-network IPv4 addresses (validated on save as well, see `Security/PrinterAddress`), uses
   connection timeouts, and passes a sanitized, quoted argument to an optional, configured helper
   program. `PrinterController.ActivatePrinters` requires a signed-in employee of the order's branch.

8. **No CSRF protection.** All state-changing requests were plain cookie-authenticated POSTs.
   *Fix:* a global `ValidateAntiForgeryOnPostAttribute` checks an anti-forgery token on every POST
   (header for AJAX, hidden field for forms); `Views/Shared/_AntiForgery.cshtml` adds it
   automatically. Only the Papara server callback and the desktop login API are exempt.

9. **Weak account recovery and activation.** Codes came from `System.Random` (and for new
   accounts were derived from an id that was still 0), were never invalidated after use, links
   used a hard-coded host, and messages revealed whether an e-mail was registered.
   *Fix:* 160-bit codes from a cryptographic RNG, single use, constant-time comparison, and a
   uniform response for the reset request. Links are built only from the configured
   `App:PublicBaseUrl` (the Host header is trusted only for local requests), so a forged Host
   header cannot send a victim a valid reset link pointing to an attacker's site.

10. **No brute-force protection.** *Fix:* `Security/LoginThrottle` locks an account (and a client
    address) for 15 minutes after repeated failures on every login form, the logout-confirmation
    dialogs and the branch-deletion password prompt.

11. **Session fixation.** *Fix:* signing in clears the session and binds it to a new random,
    `HttpOnly` cookie that must match on every request (`Security/UserSession`).

### Medium / low

12. **Open redirect / action injection via `returnUrl`.** *Fix:* `Security/SafeRedirect` only
    accepts an action name or a local path inside the same panel.
13. **Mass assignment on registration** (binding the whole `Companies` entity). *Fix:* explicit
    parameters.
14. **Negative/unbounded cart quantities.** *Fix:* clamped to 0–100.
15. **Information leaks.** Exception messages were shown to users; `compilation debug="true"`,
    no custom errors, version headers, WCF metadata publishing enabled. *Fix:* generic messages,
    `customErrors`, Release transform removes debug and requires SSL cookies, version headers
    removed, `X-Frame-Options`, `X-Content-Type-Options` and `Referrer-Policy` headers added,
    request size limited to 8 MB.
16. **Hard-coded secrets in source.** *Fix:* moved to `appSettings` placeholders (see above).
17. **POS app**: the embedded browser had file-URL access enabled and dev tools opened
    automatically; printer IPs were saved without validation. *Fix:* removed/validated.

## Recent advisories

The October 2026 Microsoft ASP.NET security updates (request smuggling, XSS, Identity
password-reset and SignalR/MessagePack issues) target **ASP.NET Core** and do not apply to this
ASP.NET MVC 5 / .NET Framework application. The 2026 class of attacks that *does* apply to .NET
Framework apps is ViewState/ticket forgery with a leaked or hard-coded `<machineKey>`. This project
does not commit a machine key; keep it that way, since the new remember-me cookies rely on it.
Keep the servers on the latest .NET Framework / Windows cumulative updates.

## Known remaining risks

- The client libraries are old (jQuery 1.10/2.2, Bootstrap 3.3, CKEditor, Newtonsoft.Json 10,
  CefSharp 57 / Chromium 57 in the POS app) and have published vulnerabilities. Upgrading them
  changes behavior and needs testing, so it was not done here.
- The POS desktop app still connects to the database directly with integrated security for its
  printing loop; it should go through the web API instead.
- Sessions are in-process; use a shared session store for more than one web server.
- `MachineKey`-protected cookies require the same machine key on every server of a farm.
