# iRestaurant

Cloud-based, mobile-friendly order management for restaurants and cafés (web server project).

> **Note:** this is a university graduation project, written in 2016 and developed on and off
> until 2018. Its technology and architecture are dated, so it must **not** be used commercially
> as is. It is mostly useful as a reference — including as an example of how *not* to start a
> project.

## What you can do with it

- Take and track orders from staff tablets (in staff mode or as a tablet fixed to a table).
- Follow orders on kitchen screens.
- Print orders, order status and bills/receipts on network thermal printers.
- Manage products, product options, extras and stock.
- See detailed reports and charts.
- Use different currencies, menus and products per country, province and district.
- Add floors and tables. (Visual table placement was planned but not finished.)

The tablet app for the waiters was lost and would have to be rewritten for Android/iOS.

## Solution layout

| Project | Description |
| --- | --- |
| `WebServer/` | ASP.NET MVC 5 (.NET Framework 4.6.1) web application with Entity Framework 6 (database first). |
| `PosPrinterSoftware/` | WinForms desktop app for the cashier PC: embeds the cashier/branch panels (CefSharp) and drives the receipt printers. |
| `Database/` | SQL scripts, including the migration from the original Turkish schema. |

Inside `WebServer/`:

- `Controllers/` — `Home` (public site, sign-up, cart, Papara checkout), `CompanyAdmin`,
  `BranchAdmin`, `Cashier`, `Kitchen`, `Printer`. All derive from `BaseController`.
- `Security/` — password hashing, sign-in session, remember-me cookies, authorization and CSRF
  filters, upload validation, printer address validation.
- `Infrastructure/` — configuration access, e-mail, Papara client, thermal receipt printer.
- `iRestaurant.edmx` / `iRestaurant.edmx.sql` — data model and schema script.

## Setup

1. Create the database:
   - **New database:** run `WebServer/iRestaurant.edmx.sql` on your SQL Server.
   - **Existing database from the original (Turkish) version:** attach `iRestaurant.mdf`, back it
     up, then run `Database/Migrate-TurkishSchemaToEnglish.sql` once. It renames tables, columns,
     keys and indexes to the English names used by the code.
2. Update the connection strings in `WebServer/Web.config` and `PosPrinterSoftware/App.config`.
3. Fill in the `appSettings` in `WebServer/Web.config` **per environment, never in source
   control**: `Smtp:*` and `Mail:*` (e-mails), `Papara:*` (payments) and `App:PublicBaseUrl` (e.g.
   `https://irestaurant.example.com`). `App:PublicBaseUrl` is **required** outside local
   development: without it the server will not send sign-up/reset e-mails or start payments,
   because the alternative (the request's Host header) can be forged.
4. In `PosPrinterSoftware/App.config`, set `ServerUrl` to the web server address (use `https://`
   in production).
5. In production, publish with the Release configuration: it turns off debug compilation and
   custom error details and requires HTTPS for cookies.

## Panels

| Panel | URL | Sign in with |
| --- | --- | --- |
| Public site / checkout | `http://localhost:62422/Home/Login` | Company e-mail and password |
| Company admin | `http://localhost:62422/CompanyAdmin/Login` | Company e-mail and password |
| Branch admin | `http://localhost:62422/BranchAdmin/Login` | Company e-mail, manager e-mail and password |
| Cashier | `http://localhost:62422/Cashier/Login` | Company e-mail, manager e-mail and password |
| Kitchen | `http://localhost:62422/Kitchen/Login` | Company e-mail, manager e-mail and password |

Demo data in the original database:

- Company: `denemesirket@domain.com` / `123456`
- Branch manager (branch, cashier and kitchen panels): company e-mail `denemesirket@domain.com`,
  employee e-mail `mudurbey@domain.com`, password `123456`

Change these passwords right away on any shared machine. New passwords must be at least 8
characters long. Existing passwords are upgraded to salted PBKDF2 hashes automatically the next
time each user signs in.

## Security

See [SECURITY.md](SECURITY.md) for the security review of this code base and what was fixed.
