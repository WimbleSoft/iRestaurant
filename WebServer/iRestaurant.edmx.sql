
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/18/2021 16:25:37
-- Generated from EDMX file: C:\Projects\iRestaurant\WebServer\iRestaurant.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [iRestaurant];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Departments_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Departments] DROP CONSTRAINT [FK_Departments_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_Announcements_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Announcements] DROP CONSTRAINT [FK_Announcements_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceServices_Invoices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceServices] DROP CONSTRAINT [FK_InvoiceServices_Invoices];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceServices_Services]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceServices] DROP CONSTRAINT [FK_InvoiceServices_Services];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceServices_Currencies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceServices] DROP CONSTRAINT [FK_InvoiceServices_Currencies];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoices_Companies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_Invoices_Companies];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoicePayments_Invoices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoicePayments] DROP CONSTRAINT [FK_InvoicePayments_Invoices];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoicePayments_Currencies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoicePayments] DROP CONSTRAINT [FK_InvoicePayments_Currencies];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceCampaignServices_ServiceCampaigns]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceCampaignServices] DROP CONSTRAINT [FK_ServiceCampaignServices_ServiceCampaigns];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceCampaignServices_Services]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceCampaignServices] DROP CONSTRAINT [FK_ServiceCampaignServices_Services];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceCampaigns_Currencies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceCampaigns] DROP CONSTRAINT [FK_ServiceCampaigns_Currencies];
GO
IF OBJECT_ID(N'[dbo].[FK_Services_Currencies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Services] DROP CONSTRAINT [FK_Services_Currencies];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceLicenses_Invoices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLicenses] DROP CONSTRAINT [FK_ServiceLicenses_Invoices];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceLicenses_Services]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLicenses] DROP CONSTRAINT [FK_ServiceLicenses_Services];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceLicenses_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLicenses] DROP CONSTRAINT [FK_ServiceLicenses_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_Districts_Provinces]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Districts] DROP CONSTRAINT [FK_Districts_Provinces];
GO
IF OBJECT_ID(N'[dbo].[FK_Provinces_Countries]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Provinces] DROP CONSTRAINT [FK_Provinces_Countries];
GO
IF OBJECT_ID(N'[dbo].[FK_Campaigns_Menus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_Campaigns_Menus];
GO
IF OBJECT_ID(N'[dbo].[FK_Campaigns_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_Campaigns_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_CampaignProducts_Campaigns]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CampaignProducts] DROP CONSTRAINT [FK_CampaignProducts_Campaigns];
GO
IF OBJECT_ID(N'[dbo].[FK_CampaignProducts_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CampaignProducts] DROP CONSTRAINT [FK_CampaignProducts_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Floors_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Floors] DROP CONSTRAINT [FK_Floors_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_Tables_Floors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tables] DROP CONSTRAINT [FK_Tables_Floors];
GO
IF OBJECT_ID(N'[dbo].[FK_Menus_Currencies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menus_Currencies];
GO
IF OBJECT_ID(N'[dbo].[FK_Menus_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menus_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_Payments_PaymentTypes1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_Payments_PaymentTypes1];
GO
IF OBJECT_ID(N'[dbo].[FK_Payments_TableSessions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_Payments_TableSessions];
GO
IF OBJECT_ID(N'[dbo].[FK_Payments_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_Payments_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentTypes_Currencies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentTypes] DROP CONSTRAINT [FK_PaymentTypes_Currencies];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentTypes_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentTypes] DROP CONSTRAINT [FK_PaymentTypes_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_TableSessionCampaigns_Campaigns]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TableSessionCampaigns] DROP CONSTRAINT [FK_TableSessionCampaigns_Campaigns];
GO
IF OBJECT_ID(N'[dbo].[FK_TableSessionCampaigns_TableSessions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TableSessionCampaigns] DROP CONSTRAINT [FK_TableSessionCampaigns_TableSessions];
GO
IF OBJECT_ID(N'[dbo].[FK_TableSessions_Tables]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TableSessions] DROP CONSTRAINT [FK_TableSessions_Tables];
GO
IF OBJECT_ID(N'[dbo].[FK_TableSessions_Branches1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TableSessions] DROP CONSTRAINT [FK_TableSessions_Branches1];
GO
IF OBJECT_ID(N'[dbo].[FK_Employees_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_TableSessions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_TableSessions];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderProducts_TableSessionCampaigns]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderProducts] DROP CONSTRAINT [FK_OrderProducts_TableSessionCampaigns];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderProducts_Orders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderProducts] DROP CONSTRAINT [FK_OrderProducts_Orders];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderProducts_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderProducts] DROP CONSTRAINT [FK_OrderProducts_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderProducts_ProductOptions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderProducts] DROP CONSTRAINT [FK_OrderProducts_ProductOptions];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderProductExtras_OrderProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderProductExtras] DROP CONSTRAINT [FK_OrderProductExtras_OrderProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderProductExtras_ProductExtras]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderProductExtras] DROP CONSTRAINT [FK_OrderProductExtras_ProductExtras];
GO
IF OBJECT_ID(N'[dbo].[FK_Companies_Districts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Companies] DROP CONSTRAINT [FK_Companies_Districts];
GO
IF OBJECT_ID(N'[dbo].[FK_Branches_Districts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Branches] DROP CONSTRAINT [FK_Branches_Districts];
GO
IF OBJECT_ID(N'[dbo].[FK_Branches_Companies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Branches] DROP CONSTRAINT [FK_Branches_Companies];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductExtras_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductExtras] DROP CONSTRAINT [FK_ProductExtras_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductCategories_Departments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductCategories] DROP CONSTRAINT [FK_ProductCategories_Departments];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductCategories_Menus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductCategories] DROP CONSTRAINT [FK_ProductCategories_Menus];
GO
IF OBJECT_ID(N'[dbo].[FK_Products_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_Products_ProductCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_ProductCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductOptions_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductOptions] DROP CONSTRAINT [FK_ProductOptions_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductStockEntries_ProductStocks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductStockEntries] DROP CONSTRAINT [FK_ProductStockEntries_ProductStocks];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductStocks_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductStocks] DROP CONSTRAINT [FK_ProductStocks_Branches];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductStocks_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductStocks] DROP CONSTRAINT [FK_ProductStocks_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingList_Orders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintingList] DROP CONSTRAINT [FK_PrintingList_Orders];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingList_Branches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrintingList] DROP CONSTRAINT [FK_PrintingList_Branches];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departments];
GO
IF OBJECT_ID(N'[dbo].[Announcements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Announcements];
GO
IF OBJECT_ID(N'[dbo].[InvoiceServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceServices];
GO
IF OBJECT_ID(N'[dbo].[Invoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoices];
GO
IF OBJECT_ID(N'[dbo].[InvoicePayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoicePayments];
GO
IF OBJECT_ID(N'[dbo].[ServiceCampaignServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceCampaignServices];
GO
IF OBJECT_ID(N'[dbo].[ServiceCampaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceCampaigns];
GO
IF OBJECT_ID(N'[dbo].[Services]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Services];
GO
IF OBJECT_ID(N'[dbo].[ServiceLicenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceLicenses];
GO
IF OBJECT_ID(N'[dbo].[Districts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Districts];
GO
IF OBJECT_ID(N'[dbo].[Provinces]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Provinces];
GO
IF OBJECT_ID(N'[dbo].[Campaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Campaigns];
GO
IF OBJECT_ID(N'[dbo].[CampaignProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CampaignProducts];
GO
IF OBJECT_ID(N'[dbo].[Floors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Floors];
GO
IF OBJECT_ID(N'[dbo].[Tables]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tables];
GO
IF OBJECT_ID(N'[dbo].[Menus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menus];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[PaymentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentTypes];
GO
IF OBJECT_ID(N'[dbo].[TableSessionCampaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TableSessionCampaigns];
GO
IF OBJECT_ID(N'[dbo].[TableSessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TableSessions];
GO
IF OBJECT_ID(N'[dbo].[Currencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Currencies];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[OrderProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderProducts];
GO
IF OBJECT_ID(N'[dbo].[OrderProductExtras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderProductExtras];
GO
IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[Branches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Branches];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO
IF OBJECT_ID(N'[dbo].[ProductExtras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductExtras];
GO
IF OBJECT_ID(N'[dbo].[ProductCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductCategories];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[ProductOptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductOptions];
GO
IF OBJECT_ID(N'[dbo].[ProductStockEntries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductStockEntries];
GO
IF OBJECT_ID(N'[dbo].[ProductStocks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductStocks];
GO
IF OBJECT_ID(N'[dbo].[PrintingList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PrintingList];
GO
IF OBJECT_ID(N'[iRestaurantModelStoreContainer].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [iRestaurantModelStoreContainer].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Departments'
CREATE TABLE [dbo].[Departments] (
    [departmentId] int IDENTITY(1,1) NOT NULL,
    [departmentName] nvarchar(50)  NOT NULL,
    [departmentIp] nvarchar(50)  NOT NULL,
    [isActive] bit  NOT NULL,
    [branchId] int  NOT NULL
);
GO

-- Creating table 'Announcements'
CREATE TABLE [dbo].[Announcements] (
    [announcementId] int IDENTITY(1,1) NOT NULL,
    [announcementTitle] varchar(50)  NOT NULL,
    [announcementDescription] varchar(50)  NOT NULL,
    [announcementImage] varchar(50)  NOT NULL,
    [isPublished] bit  NOT NULL,
    [branchId] int  NOT NULL
);
GO

-- Creating table 'InvoiceServices'
CREATE TABLE [dbo].[InvoiceServices] (
    [invoiceServiceId] int IDENTITY(1,1) NOT NULL,
    [serviceId] int  NOT NULL,
    [invoiceId] int  NOT NULL,
    [currencyId] int  NOT NULL,
    [quantity] int  NOT NULL,
    [isYearly] bit  NOT NULL,
    [serviceCampaignId] int  NULL
);
GO

-- Creating table 'Invoices'
CREATE TABLE [dbo].[Invoices] (
    [invoiceId] int IDENTITY(1,1) NOT NULL,
    [invoiceDate] datetime  NOT NULL,
    [isPaid] bit  NOT NULL,
    [companyId] int  NOT NULL
);
GO

-- Creating table 'InvoicePayments'
CREATE TABLE [dbo].[InvoicePayments] (
    [invoicePaymentId] int IDENTITY(1,1) NOT NULL,
    [invoicePaymentDate] datetime  NOT NULL,
    [invoicePaymentAmount] float  NOT NULL,
    [currencyId] int  NOT NULL,
    [invoiceId] int  NOT NULL
);
GO

-- Creating table 'ServiceCampaignServices'
CREATE TABLE [dbo].[ServiceCampaignServices] (
    [serviceCampaignServiceId] int IDENTITY(1,1) NOT NULL,
    [serviceId] int  NOT NULL,
    [serviceCampaignId] int  NOT NULL
);
GO

-- Creating table 'ServiceCampaigns'
CREATE TABLE [dbo].[ServiceCampaigns] (
    [serviceCampaignId] int IDENTITY(1,1) NOT NULL,
    [serviceCampaignName] nvarchar(50)  NOT NULL,
    [serviceCampaignPrice] float  NOT NULL,
    [currencyId] int  NOT NULL,
    [isYearly] bit  NOT NULL
);
GO

-- Creating table 'Services'
CREATE TABLE [dbo].[Services] (
    [serviceId] int IDENTITY(1,1) NOT NULL,
    [serviceName] nvarchar(50)  NOT NULL,
    [serviceMonthlyFee] float  NOT NULL,
    [currencyId] int  NOT NULL,
    [serviceType] smallint  NOT NULL
);
GO

-- Creating table 'ServiceLicenses'
CREATE TABLE [dbo].[ServiceLicenses] (
    [serviceLicenseId] int IDENTITY(1,1) NOT NULL,
    [serviceLicenseStartDate] datetime  NOT NULL,
    [serviceLicenseEndDate] datetime  NOT NULL,
    [serviceId] int  NOT NULL,
    [branchId] int  NULL,
    [invoiceId] int  NOT NULL
);
GO

-- Creating table 'Districts'
CREATE TABLE [dbo].[Districts] (
    [districtId] int IDENTITY(1,1) NOT NULL,
    [districtName] varchar(50)  NOT NULL,
    [provinceId] int  NOT NULL
);
GO

-- Creating table 'Provinces'
CREATE TABLE [dbo].[Provinces] (
    [provinceId] int IDENTITY(1,1) NOT NULL,
    [provinceName] varchar(50)  NOT NULL,
    [countryId] int  NOT NULL
);
GO

-- Creating table 'Campaigns'
CREATE TABLE [dbo].[Campaigns] (
    [campaignId] int IDENTITY(1,1) NOT NULL,
    [campaignName] varchar(50)  NOT NULL,
    [campaignPrice] float  NOT NULL,
    [campaignImage] varchar(50)  NOT NULL,
    [campaignDescription] varchar(max)  NOT NULL,
    [isPublished] bit  NOT NULL,
    [branchId] int  NOT NULL,
    [menuId] int  NOT NULL
);
GO

-- Creating table 'CampaignProducts'
CREATE TABLE [dbo].[CampaignProducts] (
    [campaignProductId] int IDENTITY(1,1) NOT NULL,
    [campaignId] int  NOT NULL,
    [productId] int  NOT NULL
);
GO

-- Creating table 'Floors'
CREATE TABLE [dbo].[Floors] (
    [floorId] int IDENTITY(1,1) NOT NULL,
    [floorName] nvarchar(15)  NOT NULL,
    [branchId] int  NOT NULL,
    [isDeleted] bit  NOT NULL
);
GO

-- Creating table 'Tables'
CREATE TABLE [dbo].[Tables] (
    [tableId] int IDENTITY(1,1) NOT NULL,
    [floorId] int  NOT NULL,
    [tableName] nvarchar(15)  NULL,
    [status] bit  NULL
);
GO

-- Creating table 'Menus'
CREATE TABLE [dbo].[Menus] (
    [menuId] int IDENTITY(1,1) NOT NULL,
    [menuName] nvarchar(50)  NOT NULL,
    [currencyId] int  NOT NULL,
    [branchId] int  NOT NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [paymentId] int IDENTITY(1,1) NOT NULL,
    [tableSessionId] int  NOT NULL,
    [paymentTypeId] int  NOT NULL,
    [paymentAmount] float  NOT NULL,
    [paymentDate] datetime  NOT NULL,
    [branchId] int  NOT NULL
);
GO

-- Creating table 'PaymentTypes'
CREATE TABLE [dbo].[PaymentTypes] (
    [paymentTypeId] int IDENTITY(1,1) NOT NULL,
    [paymentTypeName] nvarchar(50)  NOT NULL,
    [branchId] int  NOT NULL,
    [currencyId] int  NOT NULL,
    [isDeleted] bit  NOT NULL
);
GO

-- Creating table 'TableSessionCampaigns'
CREATE TABLE [dbo].[TableSessionCampaigns] (
    [tableSessionCampaignId] int IDENTITY(1,1) NOT NULL,
    [campaignId] int  NOT NULL,
    [tableSessionId] int  NOT NULL,
    [isPaid] bit  NOT NULL
);
GO

-- Creating table 'TableSessions'
CREATE TABLE [dbo].[TableSessions] (
    [tableSessionId] int IDENTITY(1,1) NOT NULL,
    [tableId] int  NOT NULL,
    [tableSessionOpeningDate] datetime  NOT NULL,
    [tableSessionClosingDate] datetime  NULL,
    [kilitlendi] bit  NULL,
    [isPaid] bit  NULL,
    [waiterRequested] bit  NOT NULL,
    [billRequested] bit  NOT NULL,
    [branchId] int  NOT NULL
);
GO

-- Creating table 'Currencies'
CREATE TABLE [dbo].[Currencies] (
    [currencyId] int IDENTITY(1,1) NOT NULL,
    [currencyName] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [employeeId] int IDENTITY(1,1) NOT NULL,
    [employeeName] nvarchar(50)  NOT NULL,
    [employeePassword] nvarchar(max)  NOT NULL,
    [employeeEmail] nvarchar(50)  NOT NULL,
    [employeePhone] nvarchar(50)  NOT NULL,
    [isManager] bit  NOT NULL,
    [branchId] int  NOT NULL,
    [isDeleted] bit  NOT NULL,
    [isTableMode] bit  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [orderId] int IDENTITY(1,1) NOT NULL,
    [orderDate] datetime  NOT NULL,
    [orderStatus] bit  NOT NULL,
    [note] nvarchar(max)  NULL,
    [tableSessionId] int  NOT NULL,
    [employeeId] int  NOT NULL,
    [branchId] int  NOT NULL
);
GO

-- Creating table 'OrderProducts'
CREATE TABLE [dbo].[OrderProducts] (
    [orderProductId] int IDENTITY(1,1) NOT NULL,
    [orderId] int  NOT NULL,
    [productId] int  NOT NULL,
    [isConfirmed] bit  NOT NULL,
    [isPaid] bit  NOT NULL,
    [productOptionId] int  NOT NULL,
    [tableSessionCampaignId] int  NULL,
    [paidDate] datetime  NULL
);
GO

-- Creating table 'OrderProductExtras'
CREATE TABLE [dbo].[OrderProductExtras] (
    [orderProductExtraId] int IDENTITY(1,1) NOT NULL,
    [productExtraId] int  NOT NULL,
    [orderProductId] int  NOT NULL,
    [isPaid] bit  NOT NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [companyId] int IDENTITY(1,1) NOT NULL,
    [companyName] varchar(100)  NOT NULL,
    [companyEmail] nvarchar(80)  NOT NULL,
    [companyPhone] nvarchar(50)  NOT NULL,
    [companyPassword] nvarchar(max)  NOT NULL,
    [districtId] int  NOT NULL,
    [isActivated] bit  NOT NULL,
    [activationCode] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Branches'
CREATE TABLE [dbo].[Branches] (
    [branchId] int IDENTITY(1,1) NOT NULL,
    [branchName] nvarchar(50)  NOT NULL,
    [branchAddress] nvarchar(100)  NOT NULL,
    [isFirstSetupDone] bit  NOT NULL,
    [companyId] int  NOT NULL,
    [districtId] int  NOT NULL,
    [isAnnouncementAreaActive] bit  NOT NULL,
    [isEmployeePrinterAutomatic] bit  NOT NULL,
    [cashierPrinterIp] varchar(50)  NOT NULL,
    [employeePrinterIp] varchar(50)  NOT NULL,
    [cashierPrinterName] varchar(50)  NOT NULL,
    [employeePrinterName] varchar(50)  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [countryId] int IDENTITY(1,1) NOT NULL,
    [countryName] varchar(50)  NOT NULL
);
GO

-- Creating table 'ProductExtras'
CREATE TABLE [dbo].[ProductExtras] (
    [productExtraId] int IDENTITY(1,1) NOT NULL,
    [productId] int  NOT NULL,
    [productExtraName] nvarchar(25)  NOT NULL,
    [productExtraPrice] float  NOT NULL
);
GO

-- Creating table 'ProductCategories'
CREATE TABLE [dbo].[ProductCategories] (
    [productCategoryId] int IDENTITY(1,1) NOT NULL,
    [productCategoryName] nvarchar(50)  NOT NULL,
    [departmentId] int  NOT NULL,
    [menuId] int  NOT NULL,
    [taxPercent] float  NOT NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [productId] int IDENTITY(1,1) NOT NULL,
    [productCategoryId] int  NOT NULL,
    [productName] nvarchar(50)  NOT NULL,
    [productPrice] float  NOT NULL,
    [productImage] nvarchar(50)  NULL,
    [productDescription] varchar(max)  NULL,
    [productPreparationTime] nchar(10)  NULL,
    [isPublished] bit  NOT NULL,
    [branchId] int  NOT NULL,
    [productStockAlarmQuantity] float  NOT NULL
);
GO

-- Creating table 'ProductOptions'
CREATE TABLE [dbo].[ProductOptions] (
    [productOptionId] int IDENTITY(1,1) NOT NULL,
    [productOptionName] nvarchar(50)  NOT NULL,
    [productOptionPrice] float  NOT NULL,
    [productId] int  NOT NULL
);
GO

-- Creating table 'ProductStockEntries'
CREATE TABLE [dbo].[ProductStockEntries] (
    [productStockEntryId] int IDENTITY(1,1) NOT NULL,
    [productStockId] int  NOT NULL,
    [productStockEntryDate] datetime  NOT NULL,
    [productStockEntryQuantity] float  NOT NULL
);
GO

-- Creating table 'ProductStocks'
CREATE TABLE [dbo].[ProductStocks] (
    [productStockId] int IDENTITY(1,1) NOT NULL,
    [productId] int  NOT NULL,
    [branchId] int  NOT NULL,
    [quantity] float  NOT NULL
);
GO

-- Creating table 'PrintingList'
CREATE TABLE [dbo].[PrintingList] (
    [printingListId] int IDENTITY(1,1) NOT NULL,
    [branchId] int  NOT NULL,
    [orderId] int  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int  NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [departmentId] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [PK_Departments]
    PRIMARY KEY CLUSTERED ([departmentId] ASC);
GO

-- Creating primary key on [announcementId] in table 'Announcements'
ALTER TABLE [dbo].[Announcements]
ADD CONSTRAINT [PK_Announcements]
    PRIMARY KEY CLUSTERED ([announcementId] ASC);
GO

-- Creating primary key on [invoiceServiceId] in table 'InvoiceServices'
ALTER TABLE [dbo].[InvoiceServices]
ADD CONSTRAINT [PK_InvoiceServices]
    PRIMARY KEY CLUSTERED ([invoiceServiceId] ASC);
GO

-- Creating primary key on [invoiceId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [PK_Invoices]
    PRIMARY KEY CLUSTERED ([invoiceId] ASC);
GO

-- Creating primary key on [invoicePaymentId] in table 'InvoicePayments'
ALTER TABLE [dbo].[InvoicePayments]
ADD CONSTRAINT [PK_InvoicePayments]
    PRIMARY KEY CLUSTERED ([invoicePaymentId] ASC);
GO

-- Creating primary key on [serviceCampaignServiceId] in table 'ServiceCampaignServices'
ALTER TABLE [dbo].[ServiceCampaignServices]
ADD CONSTRAINT [PK_ServiceCampaignServices]
    PRIMARY KEY CLUSTERED ([serviceCampaignServiceId] ASC);
GO

-- Creating primary key on [serviceCampaignId] in table 'ServiceCampaigns'
ALTER TABLE [dbo].[ServiceCampaigns]
ADD CONSTRAINT [PK_ServiceCampaigns]
    PRIMARY KEY CLUSTERED ([serviceCampaignId] ASC);
GO

-- Creating primary key on [serviceId] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [PK_Services]
    PRIMARY KEY CLUSTERED ([serviceId] ASC);
GO

-- Creating primary key on [serviceLicenseId] in table 'ServiceLicenses'
ALTER TABLE [dbo].[ServiceLicenses]
ADD CONSTRAINT [PK_ServiceLicenses]
    PRIMARY KEY CLUSTERED ([serviceLicenseId] ASC);
GO

-- Creating primary key on [districtId] in table 'Districts'
ALTER TABLE [dbo].[Districts]
ADD CONSTRAINT [PK_Districts]
    PRIMARY KEY CLUSTERED ([districtId] ASC);
GO

-- Creating primary key on [provinceId] in table 'Provinces'
ALTER TABLE [dbo].[Provinces]
ADD CONSTRAINT [PK_Provinces]
    PRIMARY KEY CLUSTERED ([provinceId] ASC);
GO

-- Creating primary key on [campaignId] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [PK_Campaigns]
    PRIMARY KEY CLUSTERED ([campaignId] ASC);
GO

-- Creating primary key on [campaignProductId] in table 'CampaignProducts'
ALTER TABLE [dbo].[CampaignProducts]
ADD CONSTRAINT [PK_CampaignProducts]
    PRIMARY KEY CLUSTERED ([campaignProductId] ASC);
GO

-- Creating primary key on [floorId] in table 'Floors'
ALTER TABLE [dbo].[Floors]
ADD CONSTRAINT [PK_Floors]
    PRIMARY KEY CLUSTERED ([floorId] ASC);
GO

-- Creating primary key on [tableId] in table 'Tables'
ALTER TABLE [dbo].[Tables]
ADD CONSTRAINT [PK_Tables]
    PRIMARY KEY CLUSTERED ([tableId] ASC);
GO

-- Creating primary key on [menuId] in table 'Menus'
ALTER TABLE [dbo].[Menus]
ADD CONSTRAINT [PK_Menus]
    PRIMARY KEY CLUSTERED ([menuId] ASC);
GO

-- Creating primary key on [paymentId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([paymentId] ASC);
GO

-- Creating primary key on [paymentTypeId] in table 'PaymentTypes'
ALTER TABLE [dbo].[PaymentTypes]
ADD CONSTRAINT [PK_PaymentTypes]
    PRIMARY KEY CLUSTERED ([paymentTypeId] ASC);
GO

-- Creating primary key on [tableSessionCampaignId] in table 'TableSessionCampaigns'
ALTER TABLE [dbo].[TableSessionCampaigns]
ADD CONSTRAINT [PK_TableSessionCampaigns]
    PRIMARY KEY CLUSTERED ([tableSessionCampaignId] ASC);
GO

-- Creating primary key on [tableSessionId] in table 'TableSessions'
ALTER TABLE [dbo].[TableSessions]
ADD CONSTRAINT [PK_TableSessions]
    PRIMARY KEY CLUSTERED ([tableSessionId] ASC);
GO

-- Creating primary key on [currencyId] in table 'Currencies'
ALTER TABLE [dbo].[Currencies]
ADD CONSTRAINT [PK_Currencies]
    PRIMARY KEY CLUSTERED ([currencyId] ASC);
GO

-- Creating primary key on [employeeId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([employeeId] ASC);
GO

-- Creating primary key on [orderId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([orderId] ASC);
GO

-- Creating primary key on [orderProductId] in table 'OrderProducts'
ALTER TABLE [dbo].[OrderProducts]
ADD CONSTRAINT [PK_OrderProducts]
    PRIMARY KEY CLUSTERED ([orderProductId] ASC);
GO

-- Creating primary key on [orderProductExtraId] in table 'OrderProductExtras'
ALTER TABLE [dbo].[OrderProductExtras]
ADD CONSTRAINT [PK_OrderProductExtras]
    PRIMARY KEY CLUSTERED ([orderProductExtraId] ASC);
GO

-- Creating primary key on [companyId] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([companyId] ASC);
GO

-- Creating primary key on [branchId] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [PK_Branches]
    PRIMARY KEY CLUSTERED ([branchId] ASC);
GO

-- Creating primary key on [countryId] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([countryId] ASC);
GO

-- Creating primary key on [productExtraId] in table 'ProductExtras'
ALTER TABLE [dbo].[ProductExtras]
ADD CONSTRAINT [PK_ProductExtras]
    PRIMARY KEY CLUSTERED ([productExtraId] ASC);
GO

-- Creating primary key on [productCategoryId] in table 'ProductCategories'
ALTER TABLE [dbo].[ProductCategories]
ADD CONSTRAINT [PK_ProductCategories]
    PRIMARY KEY CLUSTERED ([productCategoryId] ASC);
GO

-- Creating primary key on [productId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([productId] ASC);
GO

-- Creating primary key on [productOptionId] in table 'ProductOptions'
ALTER TABLE [dbo].[ProductOptions]
ADD CONSTRAINT [PK_ProductOptions]
    PRIMARY KEY CLUSTERED ([productOptionId] ASC);
GO

-- Creating primary key on [productStockEntryId] in table 'ProductStockEntries'
ALTER TABLE [dbo].[ProductStockEntries]
ADD CONSTRAINT [PK_ProductStockEntries]
    PRIMARY KEY CLUSTERED ([productStockEntryId] ASC);
GO

-- Creating primary key on [productStockId] in table 'ProductStocks'
ALTER TABLE [dbo].[ProductStocks]
ADD CONSTRAINT [PK_ProductStocks]
    PRIMARY KEY CLUSTERED ([productStockId] ASC);
GO

-- Creating primary key on [printingListId] in table 'PrintingList'
ALTER TABLE [dbo].[PrintingList]
ADD CONSTRAINT [PK_PrintingList]
    PRIMARY KEY CLUSTERED ([printingListId] ASC);
GO

-- Creating primary key on [name], [principal_id], [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([name], [principal_id], [diagram_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [branchId] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [FK_Departments_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Departments_Branches'
CREATE INDEX [IX_FK_Departments_Branches]
ON [dbo].[Departments]
    ([branchId]);
GO

-- Creating foreign key on [departmentId] in table 'ProductCategories'
ALTER TABLE [dbo].[ProductCategories]
ADD CONSTRAINT [FK_ProductCategories_Departments]
    FOREIGN KEY ([departmentId])
    REFERENCES [dbo].[Departments]
        ([departmentId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCategories_Departments'
CREATE INDEX [IX_FK_ProductCategories_Departments]
ON [dbo].[ProductCategories]
    ([departmentId]);
GO

-- Creating foreign key on [branchId] in table 'Announcements'
ALTER TABLE [dbo].[Announcements]
ADD CONSTRAINT [FK_Announcements_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcements_Branches'
CREATE INDEX [IX_FK_Announcements_Branches]
ON [dbo].[Announcements]
    ([branchId]);
GO

-- Creating foreign key on [invoiceId] in table 'InvoiceServices'
ALTER TABLE [dbo].[InvoiceServices]
ADD CONSTRAINT [FK_InvoiceServices_Invoices]
    FOREIGN KEY ([invoiceId])
    REFERENCES [dbo].[Invoices]
        ([invoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceServices_Invoices'
CREATE INDEX [IX_FK_InvoiceServices_Invoices]
ON [dbo].[InvoiceServices]
    ([invoiceId]);
GO

-- Creating foreign key on [serviceId] in table 'InvoiceServices'
ALTER TABLE [dbo].[InvoiceServices]
ADD CONSTRAINT [FK_InvoiceServices_Services]
    FOREIGN KEY ([serviceId])
    REFERENCES [dbo].[Services]
        ([serviceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceServices_Services'
CREATE INDEX [IX_FK_InvoiceServices_Services]
ON [dbo].[InvoiceServices]
    ([serviceId]);
GO

-- Creating foreign key on [currencyId] in table 'InvoiceServices'
ALTER TABLE [dbo].[InvoiceServices]
ADD CONSTRAINT [FK_InvoiceServices_Currencies]
    FOREIGN KEY ([currencyId])
    REFERENCES [dbo].[Currencies]
        ([currencyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceServices_Currencies'
CREATE INDEX [IX_FK_InvoiceServices_Currencies]
ON [dbo].[InvoiceServices]
    ([currencyId]);
GO

-- Creating foreign key on [companyId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_Invoices_Companies]
    FOREIGN KEY ([companyId])
    REFERENCES [dbo].[Companies]
        ([companyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoices_Companies'
CREATE INDEX [IX_FK_Invoices_Companies]
ON [dbo].[Invoices]
    ([companyId]);
GO

-- Creating foreign key on [invoiceId] in table 'InvoicePayments'
ALTER TABLE [dbo].[InvoicePayments]
ADD CONSTRAINT [FK_InvoicePayments_Invoices]
    FOREIGN KEY ([invoiceId])
    REFERENCES [dbo].[Invoices]
        ([invoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoicePayments_Invoices'
CREATE INDEX [IX_FK_InvoicePayments_Invoices]
ON [dbo].[InvoicePayments]
    ([invoiceId]);
GO

-- Creating foreign key on [invoiceId] in table 'ServiceLicenses'
ALTER TABLE [dbo].[ServiceLicenses]
ADD CONSTRAINT [FK_ServiceLicenses_Invoices]
    FOREIGN KEY ([invoiceId])
    REFERENCES [dbo].[Invoices]
        ([invoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceLicenses_Invoices'
CREATE INDEX [IX_FK_ServiceLicenses_Invoices]
ON [dbo].[ServiceLicenses]
    ([invoiceId]);
GO

-- Creating foreign key on [currencyId] in table 'InvoicePayments'
ALTER TABLE [dbo].[InvoicePayments]
ADD CONSTRAINT [FK_InvoicePayments_Currencies]
    FOREIGN KEY ([currencyId])
    REFERENCES [dbo].[Currencies]
        ([currencyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoicePayments_Currencies'
CREATE INDEX [IX_FK_InvoicePayments_Currencies]
ON [dbo].[InvoicePayments]
    ([currencyId]);
GO

-- Creating foreign key on [serviceCampaignId] in table 'ServiceCampaignServices'
ALTER TABLE [dbo].[ServiceCampaignServices]
ADD CONSTRAINT [FK_ServiceCampaignServices_ServiceCampaigns]
    FOREIGN KEY ([serviceCampaignId])
    REFERENCES [dbo].[ServiceCampaigns]
        ([serviceCampaignId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceCampaignServices_ServiceCampaigns'
CREATE INDEX [IX_FK_ServiceCampaignServices_ServiceCampaigns]
ON [dbo].[ServiceCampaignServices]
    ([serviceCampaignId]);
GO

-- Creating foreign key on [serviceId] in table 'ServiceCampaignServices'
ALTER TABLE [dbo].[ServiceCampaignServices]
ADD CONSTRAINT [FK_ServiceCampaignServices_Services]
    FOREIGN KEY ([serviceId])
    REFERENCES [dbo].[Services]
        ([serviceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceCampaignServices_Services'
CREATE INDEX [IX_FK_ServiceCampaignServices_Services]
ON [dbo].[ServiceCampaignServices]
    ([serviceId]);
GO

-- Creating foreign key on [currencyId] in table 'ServiceCampaigns'
ALTER TABLE [dbo].[ServiceCampaigns]
ADD CONSTRAINT [FK_ServiceCampaigns_Currencies]
    FOREIGN KEY ([currencyId])
    REFERENCES [dbo].[Currencies]
        ([currencyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceCampaigns_Currencies'
CREATE INDEX [IX_FK_ServiceCampaigns_Currencies]
ON [dbo].[ServiceCampaigns]
    ([currencyId]);
GO

-- Creating foreign key on [currencyId] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [FK_Services_Currencies]
    FOREIGN KEY ([currencyId])
    REFERENCES [dbo].[Currencies]
        ([currencyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Services_Currencies'
CREATE INDEX [IX_FK_Services_Currencies]
ON [dbo].[Services]
    ([currencyId]);
GO

-- Creating foreign key on [serviceId] in table 'ServiceLicenses'
ALTER TABLE [dbo].[ServiceLicenses]
ADD CONSTRAINT [FK_ServiceLicenses_Services]
    FOREIGN KEY ([serviceId])
    REFERENCES [dbo].[Services]
        ([serviceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceLicenses_Services'
CREATE INDEX [IX_FK_ServiceLicenses_Services]
ON [dbo].[ServiceLicenses]
    ([serviceId]);
GO

-- Creating foreign key on [branchId] in table 'ServiceLicenses'
ALTER TABLE [dbo].[ServiceLicenses]
ADD CONSTRAINT [FK_ServiceLicenses_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceLicenses_Branches'
CREATE INDEX [IX_FK_ServiceLicenses_Branches]
ON [dbo].[ServiceLicenses]
    ([branchId]);
GO

-- Creating foreign key on [provinceId] in table 'Districts'
ALTER TABLE [dbo].[Districts]
ADD CONSTRAINT [FK_Districts_Provinces]
    FOREIGN KEY ([provinceId])
    REFERENCES [dbo].[Provinces]
        ([provinceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Districts_Provinces'
CREATE INDEX [IX_FK_Districts_Provinces]
ON [dbo].[Districts]
    ([provinceId]);
GO

-- Creating foreign key on [districtId] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_Companies_Districts]
    FOREIGN KEY ([districtId])
    REFERENCES [dbo].[Districts]
        ([districtId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Companies_Districts'
CREATE INDEX [IX_FK_Companies_Districts]
ON [dbo].[Companies]
    ([districtId]);
GO

-- Creating foreign key on [districtId] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [FK_Branches_Districts]
    FOREIGN KEY ([districtId])
    REFERENCES [dbo].[Districts]
        ([districtId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Branches_Districts'
CREATE INDEX [IX_FK_Branches_Districts]
ON [dbo].[Branches]
    ([districtId]);
GO

-- Creating foreign key on [countryId] in table 'Provinces'
ALTER TABLE [dbo].[Provinces]
ADD CONSTRAINT [FK_Provinces_Countries]
    FOREIGN KEY ([countryId])
    REFERENCES [dbo].[Countries]
        ([countryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Provinces_Countries'
CREATE INDEX [IX_FK_Provinces_Countries]
ON [dbo].[Provinces]
    ([countryId]);
GO

-- Creating foreign key on [menuId] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [FK_Campaigns_Menus]
    FOREIGN KEY ([menuId])
    REFERENCES [dbo].[Menus]
        ([menuId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Campaigns_Menus'
CREATE INDEX [IX_FK_Campaigns_Menus]
ON [dbo].[Campaigns]
    ([menuId]);
GO

-- Creating foreign key on [branchId] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [FK_Campaigns_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Campaigns_Branches'
CREATE INDEX [IX_FK_Campaigns_Branches]
ON [dbo].[Campaigns]
    ([branchId]);
GO

-- Creating foreign key on [campaignId] in table 'CampaignProducts'
ALTER TABLE [dbo].[CampaignProducts]
ADD CONSTRAINT [FK_CampaignProducts_Campaigns]
    FOREIGN KEY ([campaignId])
    REFERENCES [dbo].[Campaigns]
        ([campaignId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CampaignProducts_Campaigns'
CREATE INDEX [IX_FK_CampaignProducts_Campaigns]
ON [dbo].[CampaignProducts]
    ([campaignId]);
GO

-- Creating foreign key on [campaignId] in table 'TableSessionCampaigns'
ALTER TABLE [dbo].[TableSessionCampaigns]
ADD CONSTRAINT [FK_TableSessionCampaigns_Campaigns]
    FOREIGN KEY ([campaignId])
    REFERENCES [dbo].[Campaigns]
        ([campaignId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TableSessionCampaigns_Campaigns'
CREATE INDEX [IX_FK_TableSessionCampaigns_Campaigns]
ON [dbo].[TableSessionCampaigns]
    ([campaignId]);
GO

-- Creating foreign key on [productId] in table 'CampaignProducts'
ALTER TABLE [dbo].[CampaignProducts]
ADD CONSTRAINT [FK_CampaignProducts_Products]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Products]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CampaignProducts_Products'
CREATE INDEX [IX_FK_CampaignProducts_Products]
ON [dbo].[CampaignProducts]
    ([productId]);
GO

-- Creating foreign key on [branchId] in table 'Floors'
ALTER TABLE [dbo].[Floors]
ADD CONSTRAINT [FK_Floors_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Floors_Branches'
CREATE INDEX [IX_FK_Floors_Branches]
ON [dbo].[Floors]
    ([branchId]);
GO

-- Creating foreign key on [floorId] in table 'Tables'
ALTER TABLE [dbo].[Tables]
ADD CONSTRAINT [FK_Tables_Floors]
    FOREIGN KEY ([floorId])
    REFERENCES [dbo].[Floors]
        ([floorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Tables_Floors'
CREATE INDEX [IX_FK_Tables_Floors]
ON [dbo].[Tables]
    ([floorId]);
GO

-- Creating foreign key on [tableId] in table 'TableSessions'
ALTER TABLE [dbo].[TableSessions]
ADD CONSTRAINT [FK_TableSessions_Tables]
    FOREIGN KEY ([tableId])
    REFERENCES [dbo].[Tables]
        ([tableId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TableSessions_Tables'
CREATE INDEX [IX_FK_TableSessions_Tables]
ON [dbo].[TableSessions]
    ([tableId]);
GO

-- Creating foreign key on [currencyId] in table 'Menus'
ALTER TABLE [dbo].[Menus]
ADD CONSTRAINT [FK_Menus_Currencies]
    FOREIGN KEY ([currencyId])
    REFERENCES [dbo].[Currencies]
        ([currencyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Menus_Currencies'
CREATE INDEX [IX_FK_Menus_Currencies]
ON [dbo].[Menus]
    ([currencyId]);
GO

-- Creating foreign key on [branchId] in table 'Menus'
ALTER TABLE [dbo].[Menus]
ADD CONSTRAINT [FK_Menus_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Menus_Branches'
CREATE INDEX [IX_FK_Menus_Branches]
ON [dbo].[Menus]
    ([branchId]);
GO

-- Creating foreign key on [menuId] in table 'ProductCategories'
ALTER TABLE [dbo].[ProductCategories]
ADD CONSTRAINT [FK_ProductCategories_Menus]
    FOREIGN KEY ([menuId])
    REFERENCES [dbo].[Menus]
        ([menuId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCategories_Menus'
CREATE INDEX [IX_FK_ProductCategories_Menus]
ON [dbo].[ProductCategories]
    ([menuId]);
GO

-- Creating foreign key on [paymentTypeId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_Payments_PaymentTypes1]
    FOREIGN KEY ([paymentTypeId])
    REFERENCES [dbo].[PaymentTypes]
        ([paymentTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Payments_PaymentTypes1'
CREATE INDEX [IX_FK_Payments_PaymentTypes1]
ON [dbo].[Payments]
    ([paymentTypeId]);
GO

-- Creating foreign key on [tableSessionId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_Payments_TableSessions]
    FOREIGN KEY ([tableSessionId])
    REFERENCES [dbo].[TableSessions]
        ([tableSessionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Payments_TableSessions'
CREATE INDEX [IX_FK_Payments_TableSessions]
ON [dbo].[Payments]
    ([tableSessionId]);
GO

-- Creating foreign key on [branchId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_Payments_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Payments_Branches'
CREATE INDEX [IX_FK_Payments_Branches]
ON [dbo].[Payments]
    ([branchId]);
GO

-- Creating foreign key on [currencyId] in table 'PaymentTypes'
ALTER TABLE [dbo].[PaymentTypes]
ADD CONSTRAINT [FK_PaymentTypes_Currencies]
    FOREIGN KEY ([currencyId])
    REFERENCES [dbo].[Currencies]
        ([currencyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentTypes_Currencies'
CREATE INDEX [IX_FK_PaymentTypes_Currencies]
ON [dbo].[PaymentTypes]
    ([currencyId]);
GO

-- Creating foreign key on [branchId] in table 'PaymentTypes'
ALTER TABLE [dbo].[PaymentTypes]
ADD CONSTRAINT [FK_PaymentTypes_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentTypes_Branches'
CREATE INDEX [IX_FK_PaymentTypes_Branches]
ON [dbo].[PaymentTypes]
    ([branchId]);
GO

-- Creating foreign key on [tableSessionId] in table 'TableSessionCampaigns'
ALTER TABLE [dbo].[TableSessionCampaigns]
ADD CONSTRAINT [FK_TableSessionCampaigns_TableSessions]
    FOREIGN KEY ([tableSessionId])
    REFERENCES [dbo].[TableSessions]
        ([tableSessionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TableSessionCampaigns_TableSessions'
CREATE INDEX [IX_FK_TableSessionCampaigns_TableSessions]
ON [dbo].[TableSessionCampaigns]
    ([tableSessionId]);
GO

-- Creating foreign key on [tableSessionCampaignId] in table 'OrderProducts'
ALTER TABLE [dbo].[OrderProducts]
ADD CONSTRAINT [FK_OrderProducts_TableSessionCampaigns]
    FOREIGN KEY ([tableSessionCampaignId])
    REFERENCES [dbo].[TableSessionCampaigns]
        ([tableSessionCampaignId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProducts_TableSessionCampaigns'
CREATE INDEX [IX_FK_OrderProducts_TableSessionCampaigns]
ON [dbo].[OrderProducts]
    ([tableSessionCampaignId]);
GO

-- Creating foreign key on [branchId] in table 'TableSessions'
ALTER TABLE [dbo].[TableSessions]
ADD CONSTRAINT [FK_TableSessions_Branches1]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TableSessions_Branches1'
CREATE INDEX [IX_FK_TableSessions_Branches1]
ON [dbo].[TableSessions]
    ([branchId]);
GO

-- Creating foreign key on [tableSessionId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_TableSessions]
    FOREIGN KEY ([tableSessionId])
    REFERENCES [dbo].[TableSessions]
        ([tableSessionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_TableSessions'
CREATE INDEX [IX_FK_Orders_TableSessions]
ON [dbo].[Orders]
    ([tableSessionId]);
GO

-- Creating foreign key on [branchId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_Employees_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employees_Branches'
CREATE INDEX [IX_FK_Employees_Branches]
ON [dbo].[Employees]
    ([branchId]);
GO

-- Creating foreign key on [employeeId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_Employees]
    FOREIGN KEY ([employeeId])
    REFERENCES [dbo].[Employees]
        ([employeeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_Employees'
CREATE INDEX [IX_FK_Orders_Employees]
ON [dbo].[Orders]
    ([employeeId]);
GO

-- Creating foreign key on [branchId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_Branches'
CREATE INDEX [IX_FK_Orders_Branches]
ON [dbo].[Orders]
    ([branchId]);
GO

-- Creating foreign key on [orderId] in table 'OrderProducts'
ALTER TABLE [dbo].[OrderProducts]
ADD CONSTRAINT [FK_OrderProducts_Orders]
    FOREIGN KEY ([orderId])
    REFERENCES [dbo].[Orders]
        ([orderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProducts_Orders'
CREATE INDEX [IX_FK_OrderProducts_Orders]
ON [dbo].[OrderProducts]
    ([orderId]);
GO

-- Creating foreign key on [orderId] in table 'PrintingList'
ALTER TABLE [dbo].[PrintingList]
ADD CONSTRAINT [FK_PrintingList_Orders]
    FOREIGN KEY ([orderId])
    REFERENCES [dbo].[Orders]
        ([orderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingList_Orders'
CREATE INDEX [IX_FK_PrintingList_Orders]
ON [dbo].[PrintingList]
    ([orderId]);
GO

-- Creating foreign key on [productId] in table 'OrderProducts'
ALTER TABLE [dbo].[OrderProducts]
ADD CONSTRAINT [FK_OrderProducts_Products]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Products]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProducts_Products'
CREATE INDEX [IX_FK_OrderProducts_Products]
ON [dbo].[OrderProducts]
    ([productId]);
GO

-- Creating foreign key on [productOptionId] in table 'OrderProducts'
ALTER TABLE [dbo].[OrderProducts]
ADD CONSTRAINT [FK_OrderProducts_ProductOptions]
    FOREIGN KEY ([productOptionId])
    REFERENCES [dbo].[ProductOptions]
        ([productOptionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProducts_ProductOptions'
CREATE INDEX [IX_FK_OrderProducts_ProductOptions]
ON [dbo].[OrderProducts]
    ([productOptionId]);
GO

-- Creating foreign key on [orderProductId] in table 'OrderProductExtras'
ALTER TABLE [dbo].[OrderProductExtras]
ADD CONSTRAINT [FK_OrderProductExtras_OrderProducts]
    FOREIGN KEY ([orderProductId])
    REFERENCES [dbo].[OrderProducts]
        ([orderProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProductExtras_OrderProducts'
CREATE INDEX [IX_FK_OrderProductExtras_OrderProducts]
ON [dbo].[OrderProductExtras]
    ([orderProductId]);
GO

-- Creating foreign key on [productExtraId] in table 'OrderProductExtras'
ALTER TABLE [dbo].[OrderProductExtras]
ADD CONSTRAINT [FK_OrderProductExtras_ProductExtras]
    FOREIGN KEY ([productExtraId])
    REFERENCES [dbo].[ProductExtras]
        ([productExtraId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProductExtras_ProductExtras'
CREATE INDEX [IX_FK_OrderProductExtras_ProductExtras]
ON [dbo].[OrderProductExtras]
    ([productExtraId]);
GO

-- Creating foreign key on [companyId] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [FK_Branches_Companies]
    FOREIGN KEY ([companyId])
    REFERENCES [dbo].[Companies]
        ([companyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Branches_Companies'
CREATE INDEX [IX_FK_Branches_Companies]
ON [dbo].[Branches]
    ([companyId]);
GO

-- Creating foreign key on [branchId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_Branches'
CREATE INDEX [IX_FK_Products_Branches]
ON [dbo].[Products]
    ([branchId]);
GO

-- Creating foreign key on [branchId] in table 'ProductStocks'
ALTER TABLE [dbo].[ProductStocks]
ADD CONSTRAINT [FK_ProductStocks_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductStocks_Branches'
CREATE INDEX [IX_FK_ProductStocks_Branches]
ON [dbo].[ProductStocks]
    ([branchId]);
GO

-- Creating foreign key on [branchId] in table 'PrintingList'
ALTER TABLE [dbo].[PrintingList]
ADD CONSTRAINT [FK_PrintingList_Branches]
    FOREIGN KEY ([branchId])
    REFERENCES [dbo].[Branches]
        ([branchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingList_Branches'
CREATE INDEX [IX_FK_PrintingList_Branches]
ON [dbo].[PrintingList]
    ([branchId]);
GO

-- Creating foreign key on [productId] in table 'ProductExtras'
ALTER TABLE [dbo].[ProductExtras]
ADD CONSTRAINT [FK_ProductExtras_Products]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Products]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductExtras_Products'
CREATE INDEX [IX_FK_ProductExtras_Products]
ON [dbo].[ProductExtras]
    ([productId]);
GO

-- Creating foreign key on [productCategoryId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_ProductCategories]
    FOREIGN KEY ([productCategoryId])
    REFERENCES [dbo].[ProductCategories]
        ([productCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_ProductCategories'
CREATE INDEX [IX_FK_Products_ProductCategories]
ON [dbo].[Products]
    ([productCategoryId]);
GO

-- Creating foreign key on [productId] in table 'ProductOptions'
ALTER TABLE [dbo].[ProductOptions]
ADD CONSTRAINT [FK_ProductOptions_Products]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Products]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductOptions_Products'
CREATE INDEX [IX_FK_ProductOptions_Products]
ON [dbo].[ProductOptions]
    ([productId]);
GO

-- Creating foreign key on [productId] in table 'ProductStocks'
ALTER TABLE [dbo].[ProductStocks]
ADD CONSTRAINT [FK_ProductStocks_Products]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Products]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductStocks_Products'
CREATE INDEX [IX_FK_ProductStocks_Products]
ON [dbo].[ProductStocks]
    ([productId]);
GO

-- Creating foreign key on [productStockId] in table 'ProductStockEntries'
ALTER TABLE [dbo].[ProductStockEntries]
ADD CONSTRAINT [FK_ProductStockEntries_ProductStocks]
    FOREIGN KEY ([productStockId])
    REFERENCES [dbo].[ProductStocks]
        ([productStockId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductStockEntries_ProductStocks'
CREATE INDEX [IX_FK_ProductStockEntries_ProductStocks]
ON [dbo].[ProductStockEntries]
    ([productStockId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------