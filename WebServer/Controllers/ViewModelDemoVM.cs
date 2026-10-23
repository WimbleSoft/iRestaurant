namespace iRestaurant.Controllers
{
    using System.Collections.Generic;

    public class ViewModelDemoVM
    {
        
        public List<Departments> Departments { get; set; }
        public List<Announcements> Announcements { get; set; }
        public List<Invoices> Invoices { get; set; }
        public List<ServiceCampaignServices> ServiceCampaignServices { get; set; }
        public List<ServiceCampaigns> ServiceCampaigns { get; set; }
        public List<Services> Services { get; set; }
        public List<ServiceLicenses> ServiceLicenses { get; set; }
        public List<Districts> Districts { get; set; }
        public List<Provinces> Provinces { get; set; }
        public List<Campaigns> Campaigns { get; set; }
        public List<CampaignProducts> CampaignProducts { get; set; }
        public List<Floors> Floors { get; set; }
        public List<InvoiceServices> InvoiceServices { get; set; }
        public List<InvoicePayments> InvoicePayments { get; set; }
        public List<Tables> Tables { get; set; }
        public List<Menus> Menus { get; set; }
        public List<Payments> Payments { get; set; }
        public List<PaymentTypes> PaymentTypes { get; set; }
        public List<TableSessionCampaigns> TableSessionCampaigns { get; set; }
        public List<TableSessions> TableSessions { get; set; }
        public List<Currencies> Currencies { get; set; }
        public List<Employees> Employees { get; set; }
        public List<Orders> Orders { get; set; }
        public List<OrderProducts> OrderProducts { get; set; }
        public List<OrderProductExtras> OrderProductExtras { get; set; }
        public List<Companies> Companies { get; set; }
        public List<Branches> Branches { get; set; }
        public List<Countries> Countries { get; set; }
        public List<ProductExtras> ProductExtras { get; set; }
        public List<ProductCategories> ProductCategories { get; set; }
        public List<Products> Products { get; set; }
        public List<ProductOptions> ProductOptions { get; set; }
        public List<ProductStockEntries> ProductStockEntries { get; set; }
        public List<ProductStocks> ProductStocks { get; set; }
        public List<PrintingList> PrintingList { get; set; }
        public List<HomeController.CartService> CartServices { get; set; }
    }
}