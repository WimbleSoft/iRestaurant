using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iRestaurant.Models;
using iRestaurant.Security;

namespace iRestaurant.Controllers
{
    /// <summary>Company (franchise owner) panel: branches, branch managers, invoices and licenses.</summary>
    [RequireLogin(AuthArea.CompanyAdmin, AuthArea.Home)]
    public class CompanyAdminController : BaseController
    {
        /// <summary>Service type of the "cloud server" license every branch needs before other services.</summary>
        private const short CloudServerServiceType = 1;

        #region Pages

        public ActionResult Dashboard() => View(BuildCompanyViewModel(CurrentCompanyId));
        public ActionResult Employees() => View(BuildCompanyViewModel(CurrentCompanyId));
        public ActionResult Branches() => View(BuildCompanyViewModel(CurrentCompanyId));
        public ActionResult Settings() => View(BuildCompanyViewModel(CurrentCompanyId));
        public ActionResult Licenses() => View(BuildCompanyViewModel(CurrentCompanyId, includeBilling: true));
        public ActionResult Invoices() => View(BuildCompanyViewModel(CurrentCompanyId, includeBilling: true));

        [AllowAnonymous]
        public ActionResult Logout()
        {
            SignOutEverywhere();
            return RedirectToAction("Login", "CompanyAdmin");
        }

        [AllowAnonymous]
        public ActionResult Login(CompanyAccountViewModel model)
        {
            if (Request.HttpMethod != "POST")
            {
                if (UserSession.IsAuthenticated(HttpContext) && UserSession.IsCompanyArea(UserSession.GetArea(Session)))
                    return Redirect(SafeRedirect.Resolve(model?.returnUrl, "CompanyAdmin", "Dashboard"));

                int rememberedId = RememberMeCookie.Read(HttpContext, RememberMeCookie.Kind.Company,
                    id => db.Companies.Where(x => x.companyId == id && x.isActivated && !x.isDeleted).Select(x => x.companyPassword).FirstOrDefault());
                Companies remembered = rememberedId == 0 ? null : db.Companies.Find(rememberedId);
                if (remembered != null)
                {
                    UserSession.SignInCompany(HttpContext, remembered, AuthArea.CompanyAdmin);
                    return Redirect(SafeRedirect.Resolve(model?.returnUrl, "CompanyAdmin", "Dashboard"));
                }
                ModelState.Clear();
                return View();
            }

            if (model == null || string.IsNullOrWhiteSpace(model.companyEmail) || string.IsNullOrEmpty(model.companyPassword))
            {
                TempData["Message"] = "Please fill in all fields.";
                return View();
            }
            if (LoginThrottle.IsBlocked(model.companyEmail, ClientAddress))
            {
                TempData["Message"] = ThrottledLoginError;
                return View();
            }

            Companies company = db.Companies.FirstOrDefault(x => x.companyEmail == model.companyEmail && !x.isDeleted);
            bool needsRehash;
            if (company == null || !PasswordHasher.Verify(model.companyPassword, company.companyPassword, out needsRehash))
            {
                LoginThrottle.RegisterFailure(model.companyEmail, ClientAddress);
                TempData["Message"] = GenericLoginError;
                return View();
            }
            LoginThrottle.RegisterSuccess(model.companyEmail);
            if (needsRehash)
            {
                company.companyPassword = PasswordHasher.Hash(model.companyPassword);
                db.SaveChanges();
            }

            if (!company.isActivated)
            {
                SignOutEverywhere();
                TempData["Message"] = "Your account is waiting for e-mail verification.";
                return View();
            }

            UserSession.SignInCompany(HttpContext, company, AuthArea.CompanyAdmin);
            RememberMeCookie.Issue(HttpContext, RememberMeCookie.Kind.Company, company.companyId, company.companyPassword, TimeSpan.FromDays(3));
            return Redirect(SafeRedirect.Resolve(model.returnUrl, "CompanyAdmin", "Dashboard"));
        }

        #endregion

        #region Location lookups (public)

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetDistricts(int? provinceId)
        {
            var districts = provinceId.HasValue
                ? db.Districts.Where(x => x.provinceId == provinceId).Select(x => new { x.districtId, x.districtName }).ToList()
                : null;
            return JsonCode(districts ?? Enumerable.Empty<object>());
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetProvinces(int? countryId)
        {
            var provinces = countryId.HasValue
                ? db.Provinces.Where(x => x.countryId == countryId).Select(x => new { x.provinceId, x.provinceName }).ToList()
                : null;
            return JsonCode(provinces ?? Enumerable.Empty<object>());
        }

        #endregion

        #region Branches

        [HttpPost]
        public JsonResult AddBranch(Branches branchInfo)
        {
            if (branchInfo == null || string.IsNullOrWhiteSpace(branchInfo.branchName))
                return JsonCode(Failure);
            if (!PrinterAddress.IsEmptyOrAllowed(branchInfo.cashierPrinterIp) || !PrinterAddress.IsEmptyOrAllowed(branchInfo.employeePrinterIp))
                return JsonCode(Failure);

            db.Branches.Add(new Branches
            {
                branchName = branchInfo.branchName,
                branchAddress = branchInfo.branchAddress,
                isFirstSetupDone = branchInfo.isFirstSetupDone,
                districtId = branchInfo.districtId,
                companyId = CurrentCompanyId,
                isAnnouncementAreaActive = branchInfo.isAnnouncementAreaActive,
                cashierPrinterName = branchInfo.cashierPrinterName,
                cashierPrinterIp = branchInfo.cashierPrinterIp,
                employeePrinterName = branchInfo.employeePrinterName,
                employeePrinterIp = branchInfo.employeePrinterIp,
                isEmployeePrinterAutomatic = branchInfo.isEmployeePrinterAutomatic,
                isDeleted = false
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateBranch(Branches branchInfo)
        {
            Branches branch = branchInfo == null ? null : FindOwnBranch(branchInfo.branchId);
            if (branch == null)
                return JsonCode(NotAllowed);
            if (!PrinterAddress.IsEmptyOrAllowed(branchInfo.cashierPrinterIp) || !PrinterAddress.IsEmptyOrAllowed(branchInfo.employeePrinterIp))
                return JsonCode(Failure);

            branch.branchName = branchInfo.branchName;
            branch.districtId = branchInfo.districtId;
            branch.isFirstSetupDone = branchInfo.isFirstSetupDone;
            branch.branchAddress = branchInfo.branchAddress;
            branch.isEmployeePrinterAutomatic = branchInfo.isEmployeePrinterAutomatic;
            branch.employeePrinterName = branchInfo.employeePrinterName;
            branch.employeePrinterIp = branchInfo.employeePrinterIp;
            branch.cashierPrinterName = branchInfo.cashierPrinterName;
            branch.cashierPrinterIp = branchInfo.cashierPrinterIp;
            branch.isAnnouncementAreaActive = branchInfo.isAnnouncementAreaActive;
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>
        /// Deletes a branch after re-confirming the company password. The branch is soft-deleted
        /// (its history stays intact for reports) and its licenses are released for re-use.
        /// The old implementation compared against an MD5 hash that was never stored, so it never
        /// succeeded.
        /// </summary>
        [HttpPost]
        public JsonResult DeleteBranch(int branchId, string companyPassword)
        {
            Branches branch = FindOwnBranch(branchId);
            if (branch == null)
                return JsonCode(NotAllowed);

            Companies company = db.Companies.Find(CurrentCompanyId);
            bool needsRehash;
            if (company == null || LoginThrottle.IsBlocked("delete-branch/" + company.companyId, ClientAddress)
                || !PasswordHasher.Verify(companyPassword, company.companyPassword, out needsRehash))
            {
                LoginThrottle.RegisterFailure("delete-branch/" + CurrentCompanyId, ClientAddress);
                return JsonCode(2);
            }

            branch.isDeleted = true;
            foreach (ServiceLicenses license in branch.ServiceLicenses.ToList())
                license.branchId = null;
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Branch managers

        [HttpPost]
        public JsonResult AddEmployee(Employees employeeInfo)
        {
            if (!IsCompleteEmployee(employeeInfo, requirePassword: true) || FindOwnBranch(employeeInfo.branchId) == null)
                return JsonCode(Failure);
            if (!PasswordHasher.IsAcceptablePassword(employeeInfo.employeePassword))
                return JsonCode(2);

            db.Employees.Add(new Employees
            {
                employeeName = employeeInfo.employeeName,
                employeeEmail = employeeInfo.employeeEmail,
                employeePhone = employeeInfo.employeePhone,
                employeePassword = PasswordHasher.Hash(employeeInfo.employeePassword),
                isManager = true,
                branchId = employeeInfo.branchId
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateEmployee(Employees employeeInfo)
        {
            if (!IsCompleteEmployee(employeeInfo, requirePassword: false) || FindOwnBranch(employeeInfo.branchId) == null)
                return JsonCode(Failure);

            Employees employee = FindOwnEmployee(employeeInfo.employeeId);
            if (employee == null)
                return JsonCode(NotAllowed);

            if (!string.IsNullOrEmpty(employeeInfo.employeePassword))
            {
                if (!PasswordHasher.IsAcceptablePassword(employeeInfo.employeePassword))
                    return JsonCode(2);
                employee.employeePassword = PasswordHasher.Hash(employeeInfo.employeePassword);
            }
            employee.employeeName = employeeInfo.employeeName;
            employee.employeeEmail = employeeInfo.employeeEmail;
            employee.employeePhone = employeeInfo.employeePhone;
            employee.branchId = employeeInfo.branchId;
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>Employees are only flagged as deleted because their orders reference them.</summary>
        [HttpPost]
        public JsonResult DeleteEmployee(int employeeId)
        {
            return SetEmployeeDeleted(employeeId, true);
        }

        [HttpPost]
        public JsonResult RestoreEmployee(int employeeId)
        {
            return SetEmployeeDeleted(employeeId, false);
        }

        private JsonResult SetEmployeeDeleted(int employeeId, bool deleted)
        {
            Employees employee = FindOwnEmployee(employeeId);
            if (employee == null)
                return JsonCode(NotAllowed);
            employee.isDeleted = deleted;
            db.SaveChanges();
            return JsonCode(Success);
        }

        private static bool IsCompleteEmployee(Employees e, bool requirePassword)
        {
            return e != null && !string.IsNullOrWhiteSpace(e.employeeName) && !string.IsNullOrWhiteSpace(e.employeeEmail)
                   && !string.IsNullOrWhiteSpace(e.employeePhone) && e.branchId > 0
                   && (!requirePassword || !string.IsNullOrEmpty(e.employeePassword));
        }

        #endregion

        #region Service licenses

        /// <summary>
        /// Assigns a purchased license to a branch (or releases it when <paramref name="branchId"/> is null).
        /// Result codes used by the views:
        /// 0 = done, 1 = error, 2 = branch already has this license, 3 = branch already has an active
        /// license of the same type, 4 = unknown branch/license, 5 = unknown license,
        /// 6 = assign a cloud server license first, 7 = cloud license released together with all
        /// licenses that depend on it.
        /// </summary>
        [HttpPost]
        public JsonResult UpdateServiceLicense(int? branchId, int serviceLicenseId)
        {
            int companyId = CurrentCompanyId;
            ServiceLicenses license = db.ServiceLicenses.FirstOrDefault(x => x.serviceLicenseId == serviceLicenseId && x.Invoices.companyId == companyId);
            if (license == null)
                return JsonCode(branchId.HasValue ? 4 : 5);

            DateTime now = DateTime.Now;

            if (!branchId.HasValue)
            {
                if (license.Services.serviceType == CloudServerServiceType && license.Branches != null)
                {
                    // Without a cloud server no other service can run, so release everything active on that branch.
                    foreach (ServiceLicenses active in license.Branches.ServiceLicenses.Where(x => x.serviceLicenseEndDate >= now).ToList())
                        active.branchId = null;
                    license.branchId = null;
                    db.SaveChanges();
                    return JsonCode(7);
                }
                license.branchId = null;
                db.SaveChanges();
                return JsonCode(Success);
            }

            Branches branch = FindOwnBranch(branchId.Value);
            if (branch == null)
                return JsonCode(4);

            List<ServiceLicenses> branchLicenses = branch.ServiceLicenses.ToList();
            if (branchLicenses.Any(x => x.serviceLicenseId == serviceLicenseId))
                return JsonCode(2);
            if (branchLicenses.Any(x => x.serviceLicenseEndDate >= now && x.Services.serviceType == license.Services.serviceType))
                return JsonCode(3);
            if (license.Services.serviceType != CloudServerServiceType
                && !branchLicenses.Any(x => x.serviceLicenseEndDate >= now && x.Services.serviceType == CloudServerServiceType))
                return JsonCode(6);

            license.branchId = branch.branchId;
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        private Branches FindOwnBranch(int branchId)
        {
            int companyId = CurrentCompanyId;
            return db.Branches.FirstOrDefault(x => x.branchId == branchId && x.companyId == companyId && !x.isDeleted);
        }

        private Employees FindOwnEmployee(int employeeId)
        {
            int companyId = CurrentCompanyId;
            return db.Employees.FirstOrDefault(x => x.employeeId == employeeId && x.Branches.companyId == companyId);
        }
    }
}
