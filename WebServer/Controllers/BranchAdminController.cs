using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using iRestaurant.Models;
using iRestaurant.Security;

namespace iRestaurant.Controllers
{
    /// <summary>
    /// Branch manager panel: menus, products, stock, floors/tables, staff, campaigns, announcements.
    ///
    /// Every JSON endpoint still accepts the <c>branchId</c> parameter the views send, but it is
    /// ignored: the branch always comes from the signed-in session and every record is looked up
    /// through the "Own..." helpers below, so one tenant can no longer read or modify another
    /// tenant's data by changing an id.
    /// </summary>
    [RequireLogin(AuthArea.BranchAdmin)]
    public class BranchAdminController : BaseController
    {
        private const string ProductImageFolder = "~/assets/product-images";
        private const string AnnouncementImageFolder = "~/assets/announcement-images";
        private const string CampaignImageFolder = "~/assets/campaign-images";
        private const string NoImage = "no-image.jpg";

        #region Pages

        public ActionResult Dashboard()
        {
            int branchId = CurrentBranchId;
            ViewModelDemoVM vm = BuildBranchViewModel(branchId, includeDeletedEmployees: true);

            // Order counts per weekday for the current ISO week, for this branch only.
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            Func<DateTime, int> weekOf = d => calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int thisWeek = weekOf(DateTime.Now.Date);
            DateTime from = DateTime.Now.Date.AddDays(-7);
            List<DateTime> orderDates = vm.OrderProducts
                .Where(x => x.Orders.orderDate >= from && weekOf(x.Orders.orderDate) == thisWeek)
                .Select(x => x.Orders.orderDate)
                .ToList();

            ViewBag.mondayCount = orderDates.Count(d => d.DayOfWeek == DayOfWeek.Monday);
            ViewBag.tuesdayCount = orderDates.Count(d => d.DayOfWeek == DayOfWeek.Tuesday);
            ViewBag.wednesdayCount = orderDates.Count(d => d.DayOfWeek == DayOfWeek.Wednesday);
            ViewBag.thursdayCount = orderDates.Count(d => d.DayOfWeek == DayOfWeek.Thursday);
            ViewBag.fridayCount = orderDates.Count(d => d.DayOfWeek == DayOfWeek.Friday);
            ViewBag.saturdayCount = orderDates.Count(d => d.DayOfWeek == DayOfWeek.Saturday);
            ViewBag.sundayCount = orderDates.Count(d => d.DayOfWeek == DayOfWeek.Sunday);
            ViewBag.totalCount = orderDates.Count;

            ViewBag.orderData = vm.OrderProductExtras
                .GroupBy(x => x.OrderProducts.Products.productName)
                .OrderByDescending(g => g.Count())
                .Select(g => new { productsName = g.Key, quantity = g.Count() })
                .ToList();
            return View(vm);
        }

        public ActionResult PaymentTypes() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult Campaigns() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult Floors() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult Menus() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult Announcements() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult Departments() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult ProductCategories() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult Tables() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult Employees() => View(BuildBranchViewModel(CurrentBranchId));
        public ActionResult ProductsWithOptions() => View(BuildBranchViewModel(CurrentBranchId));

        [AllowAnonymous]
        public ActionResult Login(EmployeeAccountViewModel model, bool? webView, string loginToken)
        {
            return EmployeeLogin(model, webView, loginToken, AuthArea.BranchAdmin, "Dashboard");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            SignOutEverywhere();
            return RedirectToAction("Login", "BranchAdmin");
        }

        /// <summary>
        /// Sign-in check for the POS desktop app. Expects the plain password over HTTPS and answers
        /// "true_&lt;branchId&gt;_&lt;one-time token&gt;"; the token opens the web panels inside the app
        /// without putting credentials in a URL. (It used to compare a client-side hash, which made
        /// the hash itself the password.)
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [AllowWithoutAntiForgeryToken]
        public JsonResult JsonCheckLogin(EmployeeAccountViewModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.companyEmail) || string.IsNullOrWhiteSpace(model.employeeEmail) || string.IsNullOrEmpty(model.employeePassword))
                return JsonCode("Please fill in all fields.");

            string throttleKey = model.companyEmail + "/" + model.employeeEmail;
            if (LoginThrottle.IsBlocked(throttleKey, ClientAddress))
                return JsonCode(ThrottledLoginError);

            Employees employee = FindEmployeeByCredentials(model.companyEmail, model.employeeEmail, model.employeePassword);
            if (employee == null)
            {
                LoginThrottle.RegisterFailure(throttleKey, ClientAddress);
                return JsonCode(GenericLoginError);
            }
            LoginThrottle.RegisterSuccess(throttleKey);

            if (!employee.isManager)
                return JsonCode(NoPanelAccessError);

            return JsonCode("true_" + employee.branchId + "_" + OneTimeLoginTokens.Issue(employee.employeeId));
        }

        #endregion

        #region Own settings

        /// <summary>The signed-in manager edits their own profile. 2 = password does not meet the policy.</summary>
        [HttpPost]
        public JsonResult UpdateSettings(Employees employeeInfo)
        {
            Employees me = OwnEmployee(CurrentEmployeeId);
            if (me == null || employeeInfo == null)
                return JsonCode(NotAllowed);

            if (!string.IsNullOrEmpty(employeeInfo.employeePassword))
            {
                if (!PasswordHasher.IsAcceptablePassword(employeeInfo.employeePassword))
                    return JsonCode(2);
                me.employeePassword = PasswordHasher.Hash(employeeInfo.employeePassword);
            }
            if (!string.IsNullOrWhiteSpace(employeeInfo.employeeName))
                me.employeeName = employeeInfo.employeeName;
            me.employeePhone = employeeInfo.employeePhone;
            if (!string.IsNullOrWhiteSpace(employeeInfo.employeeEmail))
                me.employeeEmail = employeeInfo.employeeEmail;
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Products

        [HttpPost]
        public JsonResult AddProduct(Products productInfo, HttpPostedFileBase productImageFile)
        {
            if (productInfo == null || OwnCategory(productInfo.productCategoryId) == null)
                return JsonCode(NotAllowed);
            if (!ImageUpload.IsEmptyOrAcceptable(productImageFile))
                return JsonCode(Failure);

            var product = new Products
            {
                productName = productInfo.productName,
                productDescription = productInfo.productDescription,
                productPrice = productInfo.productPrice,
                productStockAlarmQuantity = productInfo.productStockAlarmQuantity,
                productCategoryId = productInfo.productCategoryId,
                productPreparationTime = productInfo.productPreparationTime,
                isPublished = productInfo.isPublished,
                productImage = NoImage,
                branchId = CurrentBranchId
            };
            db.Products.Add(product);
            db.SaveChanges();

            if (ImageUpload.HasFile(productImageFile))
            {
                string fileName = ImageUpload.Save(productImageFile, Server, ProductImageFolder, "product-" + product.productId);
                if (fileName == null)
                    return JsonCode(Failure);
                product.productImage = fileName;
            }

            db.ProductOptions.Add(new ProductOptions { productOptionName = "Normal", productOptionPrice = 0, productId = product.productId });
            db.ProductStocks.Add(new ProductStocks { productId = product.productId, quantity = 0, branchId = product.branchId });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateProduct(Products productInfo, HttpPostedFileBase productImageFile)
        {
            Products product = productInfo == null ? null : OwnProduct(productInfo.productId);
            if (product == null)
                return JsonCode(2);
            if (OwnCategory(productInfo.productCategoryId) == null)
                return JsonCode(NotAllowed);

            if (ImageUpload.HasFile(productImageFile))
            {
                string fileName = ImageUpload.Save(productImageFile, Server, ProductImageFolder, "product-" + product.productId);
                if (fileName == null)
                    return JsonCode(Failure);
                product.productImage = fileName;
            }

            product.productName = productInfo.productName;
            product.productCategoryId = productInfo.productCategoryId;
            product.productPrice = productInfo.productPrice;
            product.productDescription = productInfo.productDescription;
            product.isPublished = productInfo.isPublished;
            product.productPreparationTime = productInfo.productPreparationTime;
            product.productStockAlarmQuantity = productInfo.productStockAlarmQuantity;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int productId)
        {
            if (OwnProduct(productId) == null)
                return JsonCode(NotAllowed);

            // Remove dependents first: ordered items, extras, options, stock, then the product.
            db.OrderProductExtras.RemoveRange(db.OrderProductExtras.Where(x => x.OrderProducts.productId == productId));
            db.OrderProducts.RemoveRange(db.OrderProducts.Where(x => x.productId == productId));
            db.ProductExtras.RemoveRange(db.ProductExtras.Where(x => x.productId == productId));
            db.ProductOptions.RemoveRange(db.ProductOptions.Where(x => x.productId == productId));
            db.ProductStockEntries.RemoveRange(db.ProductStockEntries.Where(x => x.ProductStocks.productId == productId));
            db.ProductStocks.RemoveRange(db.ProductStocks.Where(x => x.productId == productId));
            db.CampaignProducts.RemoveRange(db.CampaignProducts.Where(x => x.productId == productId));
            db.Products.RemoveRange(db.Products.Where(x => x.productId == productId));
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Product stock

        public class ProductStockEntryJsonReturn
        {
            public double productStockEntryQuantity { get; set; }
            public string productStockEntryDate { get; set; }
            public int productStockEntryId { get; set; }
        }

        [HttpPost]
        public JsonResult AddProductStockEntry(ProductStockEntries productStockEntry)
        {
            ProductStocks stock = productStockEntry == null ? null : OwnStock(productStockEntry.productStockId);
            if (stock == null)
                return JsonCode(NotAllowed);

            var entry = new ProductStockEntries
            {
                productStockId = stock.productStockId,
                productStockEntryDate = DateTime.Now,
                productStockEntryQuantity = productStockEntry.productStockEntryQuantity
            };
            db.ProductStockEntries.Add(entry);
            stock.quantity += entry.productStockEntryQuantity;
            db.SaveChanges();

            // The view expects the entry serialized as a JSON string.
            return JsonCode(new JavaScriptSerializer().Serialize(new ProductStockEntryJsonReturn
            {
                productStockEntryQuantity = entry.productStockEntryQuantity,
                productStockEntryId = entry.productStockEntryId,
                productStockEntryDate = entry.productStockEntryDate.ToString()
            }));
        }

        [HttpPost]
        public JsonResult UpdateProductStockEntry(ProductStockEntries productStockEntry)
        {
            ProductStockEntries entry = productStockEntry == null ? null : OwnStockEntry(productStockEntry.productStockEntryId);
            if (entry == null)
                return JsonCode(2);

            // Keep the stock total consistent with the corrected entry.
            entry.ProductStocks.quantity += productStockEntry.productStockEntryQuantity - entry.productStockEntryQuantity;
            entry.productStockEntryQuantity = productStockEntry.productStockEntryQuantity;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteProductStockEntry(int productStockEntryId)
        {
            ProductStockEntries entry = OwnStockEntry(productStockEntryId);
            if (entry == null)
                return JsonCode(2);

            entry.ProductStocks.quantity -= entry.productStockEntryQuantity;
            db.ProductStockEntries.Remove(entry);
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Product options and extras

        [HttpPost]
        public JsonResult AddOption(ProductOptions productOptionInfo)
        {
            if (productOptionInfo == null || OwnProduct(productOptionInfo.productId) == null)
                return JsonCode(NotAllowed);

            db.ProductOptions.Add(new ProductOptions
            {
                productId = productOptionInfo.productId,
                productOptionName = productOptionInfo.productOptionName,
                productOptionPrice = productOptionInfo.productOptionPrice
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>2 = a product must keep at least one option.</summary>
        [HttpPost]
        public JsonResult DeleteOption(int productOptionId)
        {
            ProductOptions option = OwnOption(productOptionId);
            if (option == null)
                return JsonCode(NotAllowed);
            if (db.ProductOptions.Count(x => x.productId == option.productId) == 1)
                return JsonCode(2);

            db.OrderProductExtras.RemoveRange(db.OrderProductExtras.Where(x => x.OrderProducts.productOptionId == productOptionId));
            db.OrderProducts.RemoveRange(db.OrderProducts.Where(x => x.productOptionId == productOptionId));
            db.ProductOptions.Remove(option);
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult AddExtra(ProductExtras productExtraInfo)
        {
            if (productExtraInfo == null || OwnProduct(productExtraInfo.productId) == null)
                return JsonCode(NotAllowed);

            db.ProductExtras.Add(new ProductExtras
            {
                productId = productExtraInfo.productId,
                productExtraName = productExtraInfo.productExtraName,
                productExtraPrice = productExtraInfo.productExtraPrice
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteExtra(int productExtraId)
        {
            ProductExtras extra = OwnExtra(productExtraId);
            if (extra == null)
                return JsonCode(NotAllowed);

            db.OrderProductExtras.RemoveRange(db.OrderProductExtras.Where(x => x.productExtraId == productExtraId));
            db.ProductExtras.Remove(extra);
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Departments

        [HttpPost]
        public JsonResult AddDepartment(Departments departmentInfo)
        {
            if (departmentInfo == null || !PrinterAddress.IsEmptyOrAllowed(departmentInfo.departmentIp))
                return JsonCode(Failure);

            db.Departments.Add(new Departments
            {
                departmentName = departmentInfo.departmentName,
                departmentIp = departmentInfo.departmentIp,
                isActive = true,
                branchId = CurrentBranchId
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateDepartment(Departments departmentInfo)
        {
            Departments department = departmentInfo == null ? null : OwnDepartment(departmentInfo.departmentId);
            if (department == null)
                return JsonCode(NotAllowed);
            if (!PrinterAddress.IsEmptyOrAllowed(departmentInfo.departmentIp))
                return JsonCode(Failure);

            department.departmentName = departmentInfo.departmentName;
            department.departmentIp = departmentInfo.departmentIp;
            department.isActive = departmentInfo.isActive;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteDepartment(int departmentId)
        {
            if (OwnDepartment(departmentId) == null)
                return JsonCode(NotAllowed);

            db.OrderProductExtras.RemoveRange(db.OrderProductExtras.Where(x => x.OrderProducts.Products.ProductCategories.departmentId == departmentId));
            db.OrderProducts.RemoveRange(db.OrderProducts.Where(x => x.Products.ProductCategories.departmentId == departmentId));
            db.ProductExtras.RemoveRange(db.ProductExtras.Where(x => x.Products.ProductCategories.departmentId == departmentId));
            db.ProductOptions.RemoveRange(db.ProductOptions.Where(x => x.Products.ProductCategories.departmentId == departmentId));
            db.Products.RemoveRange(db.Products.Where(x => x.ProductCategories.departmentId == departmentId));
            db.ProductCategories.RemoveRange(db.ProductCategories.Where(x => x.departmentId == departmentId));
            db.Departments.RemoveRange(db.Departments.Where(x => x.departmentId == departmentId));
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Floors and tables

        [HttpPost]
        public JsonResult AddFloor(Floors floorInfo)
        {
            if (floorInfo == null || string.IsNullOrWhiteSpace(floorInfo.floorName))
                return JsonCode(Failure);

            db.Floors.Add(new Floors { floorName = floorInfo.floorName, branchId = CurrentBranchId, isDeleted = false });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateFloor(Floors floorInfo)
        {
            Floors floor = floorInfo == null ? null : OwnFloor(floorInfo.floorId);
            if (floor == null)
                return JsonCode(NotAllowed);

            floor.floorName = floorInfo.floorName;
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>Floors are soft-deleted so past orders keep their table/floor information.</summary>
        [HttpPost]
        public JsonResult DeleteFloor(int floorId)
        {
            Floors floor = OwnFloor(floorId);
            if (floor == null)
                return JsonCode(NotAllowed);

            floor.isDeleted = true;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult AddTable(Tables tableInfo)
        {
            if (tableInfo == null || OwnFloor(tableInfo.floorId) == null)
                return JsonCode(NotAllowed);

            db.Tables.Add(new Tables { tableName = tableInfo.tableName, floorId = tableInfo.floorId });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateTable(Tables tableInfo)
        {
            Tables table = tableInfo == null ? null : OwnTable(tableInfo.tableId);
            if (table == null || OwnFloor(tableInfo.floorId) == null)
                return JsonCode(NotAllowed);

            table.tableName = tableInfo.tableName;
            table.floorId = tableInfo.floorId;
            table.status = false;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteTable(int tableId)
        {
            if (OwnTable(tableId) == null)
                return JsonCode(NotAllowed);

            db.OrderProductExtras.RemoveRange(db.OrderProductExtras.Where(x => x.OrderProducts.Orders.TableSessions.tableId == tableId));
            db.OrderProducts.RemoveRange(db.OrderProducts.Where(x => x.Orders.TableSessions.tableId == tableId));
            db.Orders.RemoveRange(db.Orders.Where(x => x.TableSessions.tableId == tableId));
            db.TableSessions.RemoveRange(db.TableSessions.Where(x => x.tableId == tableId));
            db.Tables.RemoveRange(db.Tables.Where(x => x.tableId == tableId));
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Menus and product categories

        [HttpPost]
        public JsonResult AddMenu(Menus menuInfo)
        {
            if (menuInfo == null || string.IsNullOrWhiteSpace(menuInfo.menuName))
                return JsonCode(Failure);

            db.Menus.Add(new Menus { menuName = menuInfo.menuName, currencyId = menuInfo.currencyId, branchId = CurrentBranchId });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateMenu(Menus menuInfo)
        {
            Menus menu = menuInfo == null ? null : OwnMenu(menuInfo.menuId);
            if (menu == null)
                return JsonCode(NotAllowed);

            menu.menuName = menuInfo.menuName;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteMenu(int menuId)
        {
            if (OwnMenu(menuId) == null)
                return JsonCode(NotAllowed);

            db.OrderProductExtras.RemoveRange(db.OrderProductExtras.Where(x => x.OrderProducts.Products.ProductCategories.menuId == menuId));
            db.OrderProducts.RemoveRange(db.OrderProducts.Where(x => x.Products.ProductCategories.menuId == menuId));
            db.CampaignProducts.RemoveRange(db.CampaignProducts.Where(x => x.Products.ProductCategories.menuId == menuId));
            db.Campaigns.RemoveRange(db.Campaigns.Where(x => x.menuId == menuId));
            db.ProductExtras.RemoveRange(db.ProductExtras.Where(x => x.Products.ProductCategories.menuId == menuId));
            db.ProductOptions.RemoveRange(db.ProductOptions.Where(x => x.Products.ProductCategories.menuId == menuId));
            db.ProductStockEntries.RemoveRange(db.ProductStockEntries.Where(x => x.ProductStocks.Products.ProductCategories.menuId == menuId));
            db.ProductStocks.RemoveRange(db.ProductStocks.Where(x => x.Products.ProductCategories.menuId == menuId));
            db.Products.RemoveRange(db.Products.Where(x => x.ProductCategories.menuId == menuId));
            db.ProductCategories.RemoveRange(db.ProductCategories.Where(x => x.menuId == menuId));
            db.Menus.RemoveRange(db.Menus.Where(x => x.menuId == menuId));
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult AddProductCategory(ProductCategories productCategoryInfo)
        {
            if (productCategoryInfo == null || OwnDepartment(productCategoryInfo.departmentId) == null || OwnMenu(productCategoryInfo.menuId) == null)
                return JsonCode(NotAllowed);

            db.ProductCategories.Add(new ProductCategories
            {
                productCategoryName = productCategoryInfo.productCategoryName,
                departmentId = productCategoryInfo.departmentId,
                menuId = productCategoryInfo.menuId,
                taxPercent = productCategoryInfo.taxPercent
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateProductCategory(ProductCategories productCategoryInfo)
        {
            ProductCategories category = productCategoryInfo == null ? null : OwnCategory(productCategoryInfo.productCategoryId);
            if (category == null || OwnDepartment(productCategoryInfo.departmentId) == null)
                return JsonCode(NotAllowed);

            category.productCategoryName = productCategoryInfo.productCategoryName;
            category.departmentId = productCategoryInfo.departmentId;
            category.taxPercent = productCategoryInfo.taxPercent;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteProductCategory(int productCategoryId)
        {
            if (OwnCategory(productCategoryId) == null)
                return JsonCode(NotAllowed);

            db.OrderProductExtras.RemoveRange(db.OrderProductExtras.Where(x => x.OrderProducts.Products.productCategoryId == productCategoryId));
            db.OrderProducts.RemoveRange(db.OrderProducts.Where(x => x.Products.productCategoryId == productCategoryId));
            db.ProductOptions.RemoveRange(db.ProductOptions.Where(x => x.Products.productCategoryId == productCategoryId));
            db.ProductExtras.RemoveRange(db.ProductExtras.Where(x => x.Products.productCategoryId == productCategoryId));
            db.Products.RemoveRange(db.Products.Where(x => x.productCategoryId == productCategoryId));
            db.ProductCategories.RemoveRange(db.ProductCategories.Where(x => x.productCategoryId == productCategoryId));
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Employees

        /// <summary>Adds a waiter (non-manager). 2 = password does not meet the policy.</summary>
        [HttpPost]
        public JsonResult AddEmployee(Employees employeeInfo)
        {
            if (employeeInfo == null || string.IsNullOrWhiteSpace(employeeInfo.employeeName) || string.IsNullOrWhiteSpace(employeeInfo.employeeEmail)
                || string.IsNullOrEmpty(employeeInfo.employeePassword) || string.IsNullOrWhiteSpace(employeeInfo.employeePhone))
                return JsonCode(Failure);
            if (!PasswordHasher.IsAcceptablePassword(employeeInfo.employeePassword))
                return JsonCode(2);

            db.Employees.Add(new Employees
            {
                employeeName = employeeInfo.employeeName,
                employeeEmail = employeeInfo.employeeEmail,
                employeePhone = employeeInfo.employeePhone,
                employeePassword = PasswordHasher.Hash(employeeInfo.employeePassword),
                isManager = false,
                isTableMode = employeeInfo.isTableMode,
                branchId = CurrentBranchId
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>The password is only changed when a new one is supplied.</summary>
        [HttpPost]
        public JsonResult UpdateEmployee(Employees employeeInfo)
        {
            Employees employee = employeeInfo == null ? null : OwnEmployee(employeeInfo.employeeId);
            if (employee == null)
                return JsonCode(NotAllowed);
            if (string.IsNullOrWhiteSpace(employeeInfo.employeeName) || string.IsNullOrWhiteSpace(employeeInfo.employeeEmail) || string.IsNullOrWhiteSpace(employeeInfo.employeePhone))
                return JsonCode(Failure);

            if (!string.IsNullOrEmpty(employeeInfo.employeePassword))
            {
                if (!PasswordHasher.IsAcceptablePassword(employeeInfo.employeePassword))
                    return JsonCode(2);
                employee.employeePassword = PasswordHasher.Hash(employeeInfo.employeePassword);
            }
            employee.employeeName = employeeInfo.employeeName;
            employee.employeeEmail = employeeInfo.employeeEmail;
            employee.employeePhone = employeeInfo.employeePhone;
            employee.isTableMode = employeeInfo.isTableMode;
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>2 = managers cannot be removed here; 1 = no employee given.</summary>
        [HttpPost]
        public JsonResult DeleteEmployee(int employeeId)
        {
            if (employeeId == 0)
                return JsonCode(Failure);
            Employees employee = OwnEmployee(employeeId);
            if (employee == null)
                return JsonCode(NotAllowed);
            if (employee.isManager)
                return JsonCode(2);

            employee.isDeleted = true;
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>Toggles manager rights. 2 = you cannot change your own rights.</summary>
        [HttpPost]
        public JsonResult ChangePermission(int employeeId)
        {
            if (employeeId == CurrentEmployeeId)
                return JsonCode(2);
            Employees employee = OwnEmployee(employeeId);
            if (employee == null)
                return JsonCode(NotAllowed);

            employee.isManager = !employee.isManager;
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Announcements

        [HttpPost]
        public JsonResult AddAnnouncement(Announcements announcementInfo, HttpPostedFileBase announcementImageFile)
        {
            if (announcementInfo == null || !ImageUpload.IsEmptyOrAcceptable(announcementImageFile))
                return JsonCode(Failure);

            var announcement = new Announcements
            {
                announcementTitle = announcementInfo.announcementTitle,
                announcementDescription = announcementInfo.announcementDescription,
                isPublished = announcementInfo.isPublished,
                branchId = CurrentBranchId,
                announcementImage = NoImage
            };
            db.Announcements.Add(announcement);
            db.SaveChanges();

            if (ImageUpload.HasFile(announcementImageFile))
            {
                string fileName = ImageUpload.Save(announcementImageFile, Server, AnnouncementImageFolder, announcement.announcementId.ToString());
                if (fileName == null)
                    return JsonCode(Failure);
                announcement.announcementImage = fileName;
                db.SaveChanges();
            }
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateAnnouncement(Announcements announcementInfo, HttpPostedFileBase announcementImageFile)
        {
            Announcements announcement = announcementInfo == null ? null : OwnAnnouncement(announcementInfo.announcementId);
            if (announcement == null)
                return JsonCode(NotAllowed);

            if (ImageUpload.HasFile(announcementImageFile))
            {
                string fileName = ImageUpload.Save(announcementImageFile, Server, AnnouncementImageFolder, announcement.announcementId.ToString());
                if (fileName == null)
                    return JsonCode(Failure);
                announcement.announcementImage = fileName;
            }
            announcement.announcementTitle = announcementInfo.announcementTitle;
            announcement.announcementDescription = announcementInfo.announcementDescription;
            announcement.isPublished = announcementInfo.isPublished;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteAnnouncement(int announcementId)
        {
            Announcements announcement = OwnAnnouncement(announcementId);
            if (announcement == null)
                return JsonCode(NotAllowed);

            ImageUpload.Delete(Server, AnnouncementImageFolder, announcement.announcementImage);
            db.Announcements.Remove(announcement);
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Campaigns

        [HttpPost]
        public JsonResult AddCampaign(Campaigns campaignInfo, HttpPostedFileBase campaignImageFile)
        {
            if (campaignInfo == null || OwnMenu(campaignInfo.menuId) == null)
                return JsonCode(NotAllowed);
            if (!ImageUpload.IsEmptyOrAcceptable(campaignImageFile))
                return JsonCode(Failure);

            var campaign = new Campaigns
            {
                campaignName = campaignInfo.campaignName,
                campaignPrice = campaignInfo.campaignPrice,
                isPublished = campaignInfo.isPublished,
                campaignDescription = campaignInfo.campaignDescription,
                branchId = CurrentBranchId,
                menuId = campaignInfo.menuId,
                campaignImage = NoImage
            };
            db.Campaigns.Add(campaign);
            db.SaveChanges();

            if (ImageUpload.HasFile(campaignImageFile))
            {
                string fileName = ImageUpload.Save(campaignImageFile, Server, CampaignImageFolder, campaign.campaignId.ToString());
                if (fileName == null)
                    return JsonCode(Failure);
                campaign.campaignImage = fileName;
                db.SaveChanges();
            }
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdateCampaign(Campaigns campaignInfo, HttpPostedFileBase campaignImageFile)
        {
            Campaigns campaign = campaignInfo == null ? null : OwnCampaign(campaignInfo.campaignId);
            if (campaign == null)
                return JsonCode(NotAllowed);

            if (ImageUpload.HasFile(campaignImageFile))
            {
                string fileName = ImageUpload.Save(campaignImageFile, Server, CampaignImageFolder, campaign.campaignId.ToString());
                if (fileName == null)
                    return JsonCode(Failure);
                campaign.campaignImage = fileName;
            }
            campaign.campaignName = campaignInfo.campaignName;
            campaign.campaignPrice = campaignInfo.campaignPrice;
            campaign.isPublished = campaignInfo.isPublished;
            campaign.campaignDescription = campaignInfo.campaignDescription;
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteCampaign(int campaignId)
        {
            Campaigns campaign = OwnCampaign(campaignId);
            if (campaign == null)
                return JsonCode(NotAllowed);

            ImageUpload.Delete(Server, CampaignImageFolder, campaign.campaignImage);
            db.Campaigns.Remove(campaign);
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult AddCampaignProduct(CampaignProducts campaignProductInfo)
        {
            if (campaignProductInfo == null || OwnCampaign(campaignProductInfo.campaignId) == null || OwnProduct(campaignProductInfo.productId) == null)
                return JsonCode(NotAllowed);

            db.CampaignProducts.Add(new CampaignProducts { campaignId = campaignProductInfo.campaignId, productId = campaignProductInfo.productId });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult DeleteCampaignProduct(int campaignProductId)
        {
            int branchId = CurrentBranchId;
            CampaignProducts campaignProduct = db.CampaignProducts.FirstOrDefault(x => x.campaignProductId == campaignProductId && x.Campaigns.branchId == branchId);
            if (campaignProduct == null)
                return JsonCode(NotAllowed);

            db.CampaignProducts.Remove(campaignProduct);
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Payment types

        [HttpPost]
        public JsonResult AddPaymentType(PaymentTypes paymentTypeInfo)
        {
            if (paymentTypeInfo == null || string.IsNullOrWhiteSpace(paymentTypeInfo.paymentTypeName))
                return JsonCode(Failure);

            db.PaymentTypes.Add(new PaymentTypes
            {
                paymentTypeName = paymentTypeInfo.paymentTypeName,
                currencyId = paymentTypeInfo.currencyId,
                branchId = CurrentBranchId
            });
            db.SaveChanges();
            return JsonCode(Success);
        }

        [HttpPost]
        public JsonResult UpdatePaymentType(PaymentTypes paymentTypeInfo)
        {
            PaymentTypes paymentType = paymentTypeInfo == null ? null : OwnPaymentType(paymentTypeInfo.paymentTypeId);
            if (paymentType == null)
                return JsonCode(NotAllowed);

            paymentType.paymentTypeName = paymentTypeInfo.paymentTypeName;
            paymentType.currencyId = paymentTypeInfo.currencyId;
            db.SaveChanges();
            return JsonCode(Success);
        }

        /// <summary>Soft delete: the cashier report still lists revenue of deleted payment types.</summary>
        [HttpPost]
        public JsonResult DeletePaymentType(int paymentTypeId)
        {
            PaymentTypes paymentType = OwnPaymentType(paymentTypeId);
            if (paymentType == null)
                return JsonCode(NotAllowed);

            paymentType.isDeleted = true;
            db.SaveChanges();
            return JsonCode(Success);
        }

        #endregion

        #region Ownership lookups (all scoped to the signed-in branch)

        private Products OwnProduct(int id) { int b = CurrentBranchId; return db.Products.FirstOrDefault(x => x.productId == id && x.branchId == b); }
        private ProductCategories OwnCategory(int id) { int b = CurrentBranchId; return db.ProductCategories.FirstOrDefault(x => x.productCategoryId == id && x.Departments.branchId == b); }
        private ProductOptions OwnOption(int id) { int b = CurrentBranchId; return db.ProductOptions.FirstOrDefault(x => x.productOptionId == id && x.Products.branchId == b); }
        private ProductExtras OwnExtra(int id) { int b = CurrentBranchId; return db.ProductExtras.FirstOrDefault(x => x.productExtraId == id && x.Products.branchId == b); }
        private ProductStocks OwnStock(int id) { int b = CurrentBranchId; return db.ProductStocks.FirstOrDefault(x => x.productStockId == id && x.branchId == b); }
        private ProductStockEntries OwnStockEntry(int id) { int b = CurrentBranchId; return db.ProductStockEntries.FirstOrDefault(x => x.productStockEntryId == id && x.ProductStocks.branchId == b); }
        private Departments OwnDepartment(int id) { int b = CurrentBranchId; return db.Departments.FirstOrDefault(x => x.departmentId == id && x.branchId == b); }
        private Floors OwnFloor(int id) { int b = CurrentBranchId; return db.Floors.FirstOrDefault(x => x.floorId == id && x.branchId == b); }
        private Tables OwnTable(int id) { int b = CurrentBranchId; return db.Tables.FirstOrDefault(x => x.tableId == id && x.Floors.branchId == b); }
        private Menus OwnMenu(int id) { int b = CurrentBranchId; return db.Menus.FirstOrDefault(x => x.menuId == id && x.branchId == b); }
        private Employees OwnEmployee(int id) { int b = CurrentBranchId; return db.Employees.FirstOrDefault(x => x.employeeId == id && x.branchId == b); }
        private Announcements OwnAnnouncement(int id) { int b = CurrentBranchId; return db.Announcements.FirstOrDefault(x => x.announcementId == id && x.branchId == b); }
        private Campaigns OwnCampaign(int id) { int b = CurrentBranchId; return db.Campaigns.FirstOrDefault(x => x.campaignId == id && x.branchId == b); }
        private PaymentTypes OwnPaymentType(int id) { int b = CurrentBranchId; return db.PaymentTypes.FirstOrDefault(x => x.paymentTypeId == id && x.branchId == b); }

        #endregion
    }
}
