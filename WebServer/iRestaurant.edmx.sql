
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

IF OBJECT_ID(N'[dbo].[FK_Departmanlar_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Departmanlar] DROP CONSTRAINT [FK_Departmanlar_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_Duyurular_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Duyurular] DROP CONSTRAINT [FK_Duyurular_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_FaturadakiHizmetler_Faturalar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FaturadakiHizmetler] DROP CONSTRAINT [FK_FaturadakiHizmetler_Faturalar];
GO
IF OBJECT_ID(N'[dbo].[FK_FaturadakiHizmetler_Hizmetler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FaturadakiHizmetler] DROP CONSTRAINT [FK_FaturadakiHizmetler_Hizmetler];
GO
IF OBJECT_ID(N'[dbo].[FK_FaturadakiHizmetler_ParaBirimleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FaturadakiHizmetler] DROP CONSTRAINT [FK_FaturadakiHizmetler_ParaBirimleri];
GO
IF OBJECT_ID(N'[dbo].[FK_Faturalar_Sirketler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Faturalar] DROP CONSTRAINT [FK_Faturalar_Sirketler];
GO
IF OBJECT_ID(N'[dbo].[FK_FaturaOdemeleri_Faturalar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FaturaOdemeleri] DROP CONSTRAINT [FK_FaturaOdemeleri_Faturalar];
GO
IF OBJECT_ID(N'[dbo].[FK_FaturaOdemeleri_ParaBirimleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FaturaOdemeleri] DROP CONSTRAINT [FK_FaturaOdemeleri_ParaBirimleri];
GO
IF OBJECT_ID(N'[dbo].[FK_HizmetKampanyaHizmetleri_HizmetKampanyalari]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HizmetKampanyaHizmetleri] DROP CONSTRAINT [FK_HizmetKampanyaHizmetleri_HizmetKampanyalari];
GO
IF OBJECT_ID(N'[dbo].[FK_HizmetKampanyaHizmetleri_Hizmetler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HizmetKampanyaHizmetleri] DROP CONSTRAINT [FK_HizmetKampanyaHizmetleri_Hizmetler];
GO
IF OBJECT_ID(N'[dbo].[FK_HizmetKampanyalari_ParaBirimleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HizmetKampanyalari] DROP CONSTRAINT [FK_HizmetKampanyalari_ParaBirimleri];
GO
IF OBJECT_ID(N'[dbo].[FK_Hizmetler_ParaBirimleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Hizmetler] DROP CONSTRAINT [FK_Hizmetler_ParaBirimleri];
GO
IF OBJECT_ID(N'[dbo].[FK_HizmetLisanslari_Faturalar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HizmetLisanslari] DROP CONSTRAINT [FK_HizmetLisanslari_Faturalar];
GO
IF OBJECT_ID(N'[dbo].[FK_HizmetLisanslari_Hizmetler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HizmetLisanslari] DROP CONSTRAINT [FK_HizmetLisanslari_Hizmetler];
GO
IF OBJECT_ID(N'[dbo].[FK_HizmetLisanslari_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HizmetLisanslari] DROP CONSTRAINT [FK_HizmetLisanslari_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_Ilceler_Iller]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ilceler] DROP CONSTRAINT [FK_Ilceler_Iller];
GO
IF OBJECT_ID(N'[dbo].[FK_Iller_Ulkeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Iller] DROP CONSTRAINT [FK_Iller_Ulkeler];
GO
IF OBJECT_ID(N'[dbo].[FK_Kampanyalar_Menuler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Kampanyalar] DROP CONSTRAINT [FK_Kampanyalar_Menuler];
GO
IF OBJECT_ID(N'[dbo].[FK_Kampanyalar_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Kampanyalar] DROP CONSTRAINT [FK_Kampanyalar_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_KampanyaUrunleri_Kampanyalar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KampanyaUrunleri] DROP CONSTRAINT [FK_KampanyaUrunleri_Kampanyalar];
GO
IF OBJECT_ID(N'[dbo].[FK_KampanyaUrunleri_Urunler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KampanyaUrunleri] DROP CONSTRAINT [FK_KampanyaUrunleri_Urunler];
GO
IF OBJECT_ID(N'[dbo].[FK_Katlar_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Katlar] DROP CONSTRAINT [FK_Katlar_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_Masalar_Katlar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Masalar] DROP CONSTRAINT [FK_Masalar_Katlar];
GO
IF OBJECT_ID(N'[dbo].[FK_Menuler_ParaBirimleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Menuler] DROP CONSTRAINT [FK_Menuler_ParaBirimleri];
GO
IF OBJECT_ID(N'[dbo].[FK_Menuler_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Menuler] DROP CONSTRAINT [FK_Menuler_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_Odemeler_OdemeTurleri1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Odemeler] DROP CONSTRAINT [FK_Odemeler_OdemeTurleri1];
GO
IF OBJECT_ID(N'[dbo].[FK_Odemeler_Oturumlar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Odemeler] DROP CONSTRAINT [FK_Odemeler_Oturumlar];
GO
IF OBJECT_ID(N'[dbo].[FK_Odemeler_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Odemeler] DROP CONSTRAINT [FK_Odemeler_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_OdemeTurleri_ParaBirimleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OdemeTurleri] DROP CONSTRAINT [FK_OdemeTurleri_ParaBirimleri];
GO
IF OBJECT_ID(N'[dbo].[FK_OdemeTurleri_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OdemeTurleri] DROP CONSTRAINT [FK_OdemeTurleri_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_OturumKampanyalari_Kampanyalar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OturumKampanyalari] DROP CONSTRAINT [FK_OturumKampanyalari_Kampanyalar];
GO
IF OBJECT_ID(N'[dbo].[FK_OturumKampanyalari_Oturumlar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OturumKampanyalari] DROP CONSTRAINT [FK_OturumKampanyalari_Oturumlar];
GO
IF OBJECT_ID(N'[dbo].[FK_Oturumlar_Masalar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Oturumlar] DROP CONSTRAINT [FK_Oturumlar_Masalar];
GO
IF OBJECT_ID(N'[dbo].[FK_Oturumlar_Subeler1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Oturumlar] DROP CONSTRAINT [FK_Oturumlar_Subeler1];
GO
IF OBJECT_ID(N'[dbo].[FK_Personeller_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Personeller] DROP CONSTRAINT [FK_Personeller_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_Siparisler_Oturumlar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Siparisler] DROP CONSTRAINT [FK_Siparisler_Oturumlar];
GO
IF OBJECT_ID(N'[dbo].[FK_Siparisler_Personeller]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Siparisler] DROP CONSTRAINT [FK_Siparisler_Personeller];
GO
IF OBJECT_ID(N'[dbo].[FK_Siparisler_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Siparisler] DROP CONSTRAINT [FK_Siparisler_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_SiparisUrunleri_OturumKampanyalari]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiparisUrunleri] DROP CONSTRAINT [FK_SiparisUrunleri_OturumKampanyalari];
GO
IF OBJECT_ID(N'[dbo].[FK_SiparisUrunleri_Siparisler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiparisUrunleri] DROP CONSTRAINT [FK_SiparisUrunleri_Siparisler];
GO
IF OBJECT_ID(N'[dbo].[FK_SiparisUrunleri_Urunler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiparisUrunleri] DROP CONSTRAINT [FK_SiparisUrunleri_Urunler];
GO
IF OBJECT_ID(N'[dbo].[FK_SiparisUrunleri_UrunOzellikler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiparisUrunleri] DROP CONSTRAINT [FK_SiparisUrunleri_UrunOzellikler];
GO
IF OBJECT_ID(N'[dbo].[FK_SiparisUrunleriEkstralari_SiparisUrunleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiparisUrunleriEkstralari] DROP CONSTRAINT [FK_SiparisUrunleriEkstralari_SiparisUrunleri];
GO
IF OBJECT_ID(N'[dbo].[FK_SiparisUrunleriEkstralari_UrunEkstralar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiparisUrunleriEkstralari] DROP CONSTRAINT [FK_SiparisUrunleriEkstralari_UrunEkstralar];
GO
IF OBJECT_ID(N'[dbo].[FK_Sirketler_Ilceler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sirketler] DROP CONSTRAINT [FK_Sirketler_Ilceler];
GO
IF OBJECT_ID(N'[dbo].[FK_Subeler_Ilceler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subeler] DROP CONSTRAINT [FK_Subeler_Ilceler];
GO
IF OBJECT_ID(N'[dbo].[FK_Subeler_Sirketler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subeler] DROP CONSTRAINT [FK_Subeler_Sirketler];
GO
IF OBJECT_ID(N'[dbo].[FK_UrunEkstralar_Urunler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrunEkstralar] DROP CONSTRAINT [FK_UrunEkstralar_Urunler];
GO
IF OBJECT_ID(N'[dbo].[FK_UrunKategoriler_Departmanlar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrunKategoriler] DROP CONSTRAINT [FK_UrunKategoriler_Departmanlar];
GO
IF OBJECT_ID(N'[dbo].[FK_UrunKategoriler_Menuler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrunKategoriler] DROP CONSTRAINT [FK_UrunKategoriler_Menuler];
GO
IF OBJECT_ID(N'[dbo].[FK_Urunler_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Urunler] DROP CONSTRAINT [FK_Urunler_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_Urunler_UrunKategoriler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Urunler] DROP CONSTRAINT [FK_Urunler_UrunKategoriler];
GO
IF OBJECT_ID(N'[dbo].[FK_UrunOzellikler_Urunler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrunOzellikler] DROP CONSTRAINT [FK_UrunOzellikler_Urunler];
GO
IF OBJECT_ID(N'[dbo].[FK_UrunStokGirdiler_UrunStoklar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrunStokGirdiler] DROP CONSTRAINT [FK_UrunStokGirdiler_UrunStoklar];
GO
IF OBJECT_ID(N'[dbo].[FK_UrunStoklar_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrunStoklar] DROP CONSTRAINT [FK_UrunStoklar_Subeler];
GO
IF OBJECT_ID(N'[dbo].[FK_UrunStoklar_Urunler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UrunStoklar] DROP CONSTRAINT [FK_UrunStoklar_Urunler];
GO
IF OBJECT_ID(N'[dbo].[FK_YazdirmaListesi_Siparisler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[YazdirmaListesi] DROP CONSTRAINT [FK_YazdirmaListesi_Siparisler];
GO
IF OBJECT_ID(N'[dbo].[FK_YazdirmaListesi_Subeler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[YazdirmaListesi] DROP CONSTRAINT [FK_YazdirmaListesi_Subeler];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Departmanlar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departmanlar];
GO
IF OBJECT_ID(N'[dbo].[Duyurular]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Duyurular];
GO
IF OBJECT_ID(N'[dbo].[FaturadakiHizmetler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FaturadakiHizmetler];
GO
IF OBJECT_ID(N'[dbo].[Faturalar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Faturalar];
GO
IF OBJECT_ID(N'[dbo].[FaturaOdemeleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FaturaOdemeleri];
GO
IF OBJECT_ID(N'[dbo].[HizmetKampanyaHizmetleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HizmetKampanyaHizmetleri];
GO
IF OBJECT_ID(N'[dbo].[HizmetKampanyalari]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HizmetKampanyalari];
GO
IF OBJECT_ID(N'[dbo].[Hizmetler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Hizmetler];
GO
IF OBJECT_ID(N'[dbo].[HizmetLisanslari]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HizmetLisanslari];
GO
IF OBJECT_ID(N'[dbo].[Ilceler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ilceler];
GO
IF OBJECT_ID(N'[dbo].[Iller]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Iller];
GO
IF OBJECT_ID(N'[dbo].[Kampanyalar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Kampanyalar];
GO
IF OBJECT_ID(N'[dbo].[KampanyaUrunleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KampanyaUrunleri];
GO
IF OBJECT_ID(N'[dbo].[Katlar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Katlar];
GO
IF OBJECT_ID(N'[dbo].[Masalar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Masalar];
GO
IF OBJECT_ID(N'[dbo].[Menuler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menuler];
GO
IF OBJECT_ID(N'[dbo].[Odemeler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Odemeler];
GO
IF OBJECT_ID(N'[dbo].[OdemeTurleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OdemeTurleri];
GO
IF OBJECT_ID(N'[dbo].[OturumKampanyalari]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OturumKampanyalari];
GO
IF OBJECT_ID(N'[dbo].[Oturumlar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Oturumlar];
GO
IF OBJECT_ID(N'[dbo].[ParaBirimleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParaBirimleri];
GO
IF OBJECT_ID(N'[dbo].[Personeller]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Personeller];
GO
IF OBJECT_ID(N'[dbo].[Siparisler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Siparisler];
GO
IF OBJECT_ID(N'[dbo].[SiparisUrunleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SiparisUrunleri];
GO
IF OBJECT_ID(N'[dbo].[SiparisUrunleriEkstralari]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SiparisUrunleriEkstralari];
GO
IF OBJECT_ID(N'[dbo].[Sirketler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sirketler];
GO
IF OBJECT_ID(N'[dbo].[Subeler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subeler];
GO
IF OBJECT_ID(N'[dbo].[Ulkeler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ulkeler];
GO
IF OBJECT_ID(N'[dbo].[UrunEkstralar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UrunEkstralar];
GO
IF OBJECT_ID(N'[dbo].[UrunKategoriler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UrunKategoriler];
GO
IF OBJECT_ID(N'[dbo].[Urunler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Urunler];
GO
IF OBJECT_ID(N'[dbo].[UrunOzellikler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UrunOzellikler];
GO
IF OBJECT_ID(N'[dbo].[UrunStokGirdiler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UrunStokGirdiler];
GO
IF OBJECT_ID(N'[dbo].[UrunStoklar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UrunStoklar];
GO
IF OBJECT_ID(N'[dbo].[YazdirmaListesi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[YazdirmaListesi];
GO
IF OBJECT_ID(N'[iRestaurantModelStoreContainer].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [iRestaurantModelStoreContainer].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Departmanlar'
CREATE TABLE [dbo].[Departmanlar] (
    [departmanId] int IDENTITY(1,1) NOT NULL,
    [departmanAd] nvarchar(50)  NOT NULL,
    [departmanIp] nvarchar(50)  NOT NULL,
    [aktifMi] bit  NOT NULL,
    [subeId] int  NOT NULL
);
GO

-- Creating table 'Duyurular'
CREATE TABLE [dbo].[Duyurular] (
    [duyuruId] int IDENTITY(1,1) NOT NULL,
    [duyuruBaslik] varchar(50)  NOT NULL,
    [duyuruAciklama] varchar(50)  NOT NULL,
    [duyuruResim] varchar(50)  NOT NULL,
    [yayinda] bit  NOT NULL,
    [subeId] int  NOT NULL
);
GO

-- Creating table 'FaturadakiHizmetler'
CREATE TABLE [dbo].[FaturadakiHizmetler] (
    [faturadakiHizmetId] int IDENTITY(1,1) NOT NULL,
    [hizmetId] int  NOT NULL,
    [faturaId] int  NOT NULL,
    [paraBirimiId] int  NOT NULL,
    [adet] int  NOT NULL,
    [yillikMi] bit  NOT NULL,
    [hizmetKampanyaId] int  NULL
);
GO

-- Creating table 'Faturalar'
CREATE TABLE [dbo].[Faturalar] (
    [faturaId] int IDENTITY(1,1) NOT NULL,
    [faturaTarih] datetime  NOT NULL,
    [odendiMi] bit  NOT NULL,
    [sirketId] int  NOT NULL
);
GO

-- Creating table 'FaturaOdemeleri'
CREATE TABLE [dbo].[FaturaOdemeleri] (
    [faturaOdemeId] int IDENTITY(1,1) NOT NULL,
    [faturaOdemeTarihi] datetime  NOT NULL,
    [faturaOdemeMiktari] float  NOT NULL,
    [paraBirimiId] int  NOT NULL,
    [faturaId] int  NOT NULL
);
GO

-- Creating table 'HizmetKampanyaHizmetleri'
CREATE TABLE [dbo].[HizmetKampanyaHizmetleri] (
    [hizmetKampanyaHizmetId] int IDENTITY(1,1) NOT NULL,
    [hizmetId] int  NOT NULL,
    [hizmetKampanyaId] int  NOT NULL
);
GO

-- Creating table 'HizmetKampanyalari'
CREATE TABLE [dbo].[HizmetKampanyalari] (
    [hizmetKampanyaId] int IDENTITY(1,1) NOT NULL,
    [hizmetKampanyaAd] nvarchar(50)  NOT NULL,
    [hizmetKampanyaFiyat] float  NOT NULL,
    [paraBirimiId] int  NOT NULL,
    [yillikMi] bit  NOT NULL
);
GO

-- Creating table 'Hizmetler'
CREATE TABLE [dbo].[Hizmetler] (
    [hizmetId] int IDENTITY(1,1) NOT NULL,
    [hizmetAd] nvarchar(50)  NOT NULL,
    [hizmetAylikUcret] float  NOT NULL,
    [paraBirimiId] int  NOT NULL,
    [hizmetTuru] smallint  NOT NULL
);
GO

-- Creating table 'HizmetLisanslari'
CREATE TABLE [dbo].[HizmetLisanslari] (
    [hizmetLisansId] int IDENTITY(1,1) NOT NULL,
    [hizmetLisansBaslangicTarihi] datetime  NOT NULL,
    [hizmetLisansBitisTarihi] datetime  NOT NULL,
    [hizmetId] int  NOT NULL,
    [subeId] int  NULL,
    [faturaId] int  NOT NULL
);
GO

-- Creating table 'Ilceler'
CREATE TABLE [dbo].[Ilceler] (
    [ilceId] int IDENTITY(1,1) NOT NULL,
    [ilceAd] varchar(50)  NOT NULL,
    [ilId] int  NOT NULL
);
GO

-- Creating table 'Iller'
CREATE TABLE [dbo].[Iller] (
    [ilId] int IDENTITY(1,1) NOT NULL,
    [ilAd] varchar(50)  NOT NULL,
    [ulkeId] int  NOT NULL
);
GO

-- Creating table 'Kampanyalar'
CREATE TABLE [dbo].[Kampanyalar] (
    [kampanyaId] int IDENTITY(1,1) NOT NULL,
    [kampanyaAd] varchar(50)  NOT NULL,
    [kampanyaFiyat] float  NOT NULL,
    [kampanyaResim] varchar(50)  NOT NULL,
    [kampanyaAciklama] varchar(max)  NOT NULL,
    [yayindaMi] bit  NOT NULL,
    [subeId] int  NOT NULL,
    [menuId] int  NOT NULL
);
GO

-- Creating table 'KampanyaUrunleri'
CREATE TABLE [dbo].[KampanyaUrunleri] (
    [kampanyaUrunleriId] int IDENTITY(1,1) NOT NULL,
    [kampanyaId] int  NOT NULL,
    [urunId] int  NOT NULL
);
GO

-- Creating table 'Katlar'
CREATE TABLE [dbo].[Katlar] (
    [katId] int IDENTITY(1,1) NOT NULL,
    [katAd] nvarchar(15)  NOT NULL,
    [subeId] int  NOT NULL,
    [silindi] bit  NOT NULL
);
GO

-- Creating table 'Masalar'
CREATE TABLE [dbo].[Masalar] (
    [masaId] int IDENTITY(1,1) NOT NULL,
    [katId] int  NOT NULL,
    [masaAd] nvarchar(15)  NULL,
    [durum] bit  NULL
);
GO

-- Creating table 'Menuler'
CREATE TABLE [dbo].[Menuler] (
    [menuId] int IDENTITY(1,1) NOT NULL,
    [menuAd] nvarchar(50)  NOT NULL,
    [paraBirimiId] int  NOT NULL,
    [subeId] int  NOT NULL
);
GO

-- Creating table 'Odemeler'
CREATE TABLE [dbo].[Odemeler] (
    [odemeId] int IDENTITY(1,1) NOT NULL,
    [oturumId] int  NOT NULL,
    [odemeTurId] int  NOT NULL,
    [odemeMiktar] float  NOT NULL,
    [odemeTarihi] datetime  NOT NULL,
    [subeId] int  NOT NULL
);
GO

-- Creating table 'OdemeTurleri'
CREATE TABLE [dbo].[OdemeTurleri] (
    [odemeTurId] int IDENTITY(1,1) NOT NULL,
    [odemeTurAd] nvarchar(50)  NOT NULL,
    [subeId] int  NOT NULL,
    [paraBirimiId] int  NOT NULL,
    [silindi] bit  NOT NULL
);
GO

-- Creating table 'OturumKampanyalari'
CREATE TABLE [dbo].[OturumKampanyalari] (
    [oturumKampanyaId] int IDENTITY(1,1) NOT NULL,
    [kampanyaId] int  NOT NULL,
    [oturumId] int  NOT NULL,
    [odendi] bit  NOT NULL
);
GO

-- Creating table 'Oturumlar'
CREATE TABLE [dbo].[Oturumlar] (
    [oturumId] int IDENTITY(1,1) NOT NULL,
    [masaId] int  NOT NULL,
    [oturumAcilmaTarihi] datetime  NOT NULL,
    [oturumKapamaTarihi] datetime  NULL,
    [kilitlendi] bit  NULL,
    [odendi] bit  NULL,
    [garsonIstendi] bit  NOT NULL,
    [hesapIstendi] bit  NOT NULL,
    [subeId] int  NOT NULL
);
GO

-- Creating table 'ParaBirimleri'
CREATE TABLE [dbo].[ParaBirimleri] (
    [paraBirimiId] int IDENTITY(1,1) NOT NULL,
    [paraBirimiAd] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'Personeller'
CREATE TABLE [dbo].[Personeller] (
    [personelId] int IDENTITY(1,1) NOT NULL,
    [personelAd] nvarchar(50)  NOT NULL,
    [personelParola] nvarchar(max)  NOT NULL,
    [personelEmail] nvarchar(50)  NOT NULL,
    [personelTelefon] nvarchar(50)  NOT NULL,
    [yetki] bit  NOT NULL,
    [subeId] int  NOT NULL,
    [silindi] bit  NOT NULL,
    [masaModuMu] bit  NOT NULL
);
GO

-- Creating table 'Siparisler'
CREATE TABLE [dbo].[Siparisler] (
    [siparislerId] int IDENTITY(1,1) NOT NULL,
    [siparisTarihi] datetime  NOT NULL,
    [siparisDurumu] bit  NOT NULL,
    [note] nvarchar(max)  NULL,
    [oturumId] int  NOT NULL,
    [personelId] int  NOT NULL,
    [subeId] int  NOT NULL
);
GO

-- Creating table 'SiparisUrunleri'
CREATE TABLE [dbo].[SiparisUrunleri] (
    [siparisUrunlerId] int IDENTITY(1,1) NOT NULL,
    [siparislerId] int  NOT NULL,
    [urunId] int  NOT NULL,
    [onaylandi] bit  NOT NULL,
    [odendi] bit  NOT NULL,
    [urunOzellikId] int  NOT NULL,
    [oturumKampanyaId] int  NULL,
    [odenmeTarihi] datetime  NULL
);
GO

-- Creating table 'SiparisUrunleriEkstralari'
CREATE TABLE [dbo].[SiparisUrunleriEkstralari] (
    [siparisUrunEkstraId] int IDENTITY(1,1) NOT NULL,
    [urunEkstraId] int  NOT NULL,
    [siparisUrunlerId] int  NOT NULL,
    [odendi] bit  NOT NULL
);
GO

-- Creating table 'Sirketler'
CREATE TABLE [dbo].[Sirketler] (
    [sirketId] int IDENTITY(1,1) NOT NULL,
    [sirketAd] varchar(100)  NOT NULL,
    [sirketEmail] nvarchar(80)  NOT NULL,
    [sirketTelefon] nvarchar(50)  NOT NULL,
    [sirketParola] nvarchar(max)  NOT NULL,
    [ilceId] int  NOT NULL,
    [aktiflestirildiMi] bit  NOT NULL,
    [aktiflestirmeKodu] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Subeler'
CREATE TABLE [dbo].[Subeler] (
    [subeId] int IDENTITY(1,1) NOT NULL,
    [subeAd] nvarchar(50)  NOT NULL,
    [subeAdres] nvarchar(100)  NOT NULL,
    [ilkKurulumYapildiMi] bit  NOT NULL,
    [sirketId] int  NOT NULL,
    [ilceId] int  NOT NULL,
    [duyuruAlaniAktifMi] bit  NOT NULL,
    [personelYazicisiOtomatikMi] bit  NOT NULL,
    [kasaYaziciIp] varchar(50)  NOT NULL,
    [personelYaziciIp] varchar(50)  NOT NULL,
    [kasaYaziciAd] varchar(50)  NOT NULL,
    [personelYaziciAd] varchar(50)  NOT NULL
);
GO

-- Creating table 'Ulkeler'
CREATE TABLE [dbo].[Ulkeler] (
    [ulkeId] int IDENTITY(1,1) NOT NULL,
    [ulkeAd] varchar(50)  NOT NULL
);
GO

-- Creating table 'UrunEkstralar'
CREATE TABLE [dbo].[UrunEkstralar] (
    [urunEkstraId] int IDENTITY(1,1) NOT NULL,
    [urunId] int  NOT NULL,
    [urunEkstraAd] nvarchar(25)  NOT NULL,
    [urunEkstraFiyat] float  NOT NULL
);
GO

-- Creating table 'UrunKategoriler'
CREATE TABLE [dbo].[UrunKategoriler] (
    [urunKategoriId] int IDENTITY(1,1) NOT NULL,
    [urunKategoriAd] nvarchar(50)  NOT NULL,
    [departmanId] int  NOT NULL,
    [menuId] int  NOT NULL,
    [vergiYuzde] float  NOT NULL
);
GO

-- Creating table 'Urunler'
CREATE TABLE [dbo].[Urunler] (
    [urunId] int IDENTITY(1,1) NOT NULL,
    [urunKategoriId] int  NOT NULL,
    [urunAd] nvarchar(50)  NOT NULL,
    [urunFiyat] float  NOT NULL,
    [urunResim] nvarchar(50)  NULL,
    [urunAciklama] varchar(max)  NULL,
    [urunYapimSuresi] nchar(10)  NULL,
    [yayinda] bit  NOT NULL,
    [subeId] int  NOT NULL,
    [urunStokAlarmAdet] float  NOT NULL
);
GO

-- Creating table 'UrunOzellikler'
CREATE TABLE [dbo].[UrunOzellikler] (
    [urunOzellikId] int IDENTITY(1,1) NOT NULL,
    [urunOzellikAd] nvarchar(50)  NOT NULL,
    [urunOzellikFiyat] float  NOT NULL,
    [urunId] int  NOT NULL
);
GO

-- Creating table 'UrunStokGirdiler'
CREATE TABLE [dbo].[UrunStokGirdiler] (
    [urunStokGirdiId] int IDENTITY(1,1) NOT NULL,
    [urunStokId] int  NOT NULL,
    [urunStokGirdiTarih] datetime  NOT NULL,
    [urunStokGirdiAdet] float  NOT NULL
);
GO

-- Creating table 'UrunStoklar'
CREATE TABLE [dbo].[UrunStoklar] (
    [urunStokId] int IDENTITY(1,1) NOT NULL,
    [urunId] int  NOT NULL,
    [subeId] int  NOT NULL,
    [adet] float  NOT NULL
);
GO

-- Creating table 'YazdirmaListesi'
CREATE TABLE [dbo].[YazdirmaListesi] (
    [yazdirmaListeId] int IDENTITY(1,1) NOT NULL,
    [subeId] int  NOT NULL,
    [siparislerId] int  NOT NULL
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

-- Creating primary key on [departmanId] in table 'Departmanlar'
ALTER TABLE [dbo].[Departmanlar]
ADD CONSTRAINT [PK_Departmanlar]
    PRIMARY KEY CLUSTERED ([departmanId] ASC);
GO

-- Creating primary key on [duyuruId] in table 'Duyurular'
ALTER TABLE [dbo].[Duyurular]
ADD CONSTRAINT [PK_Duyurular]
    PRIMARY KEY CLUSTERED ([duyuruId] ASC);
GO

-- Creating primary key on [faturadakiHizmetId] in table 'FaturadakiHizmetler'
ALTER TABLE [dbo].[FaturadakiHizmetler]
ADD CONSTRAINT [PK_FaturadakiHizmetler]
    PRIMARY KEY CLUSTERED ([faturadakiHizmetId] ASC);
GO

-- Creating primary key on [faturaId] in table 'Faturalar'
ALTER TABLE [dbo].[Faturalar]
ADD CONSTRAINT [PK_Faturalar]
    PRIMARY KEY CLUSTERED ([faturaId] ASC);
GO

-- Creating primary key on [faturaOdemeId] in table 'FaturaOdemeleri'
ALTER TABLE [dbo].[FaturaOdemeleri]
ADD CONSTRAINT [PK_FaturaOdemeleri]
    PRIMARY KEY CLUSTERED ([faturaOdemeId] ASC);
GO

-- Creating primary key on [hizmetKampanyaHizmetId] in table 'HizmetKampanyaHizmetleri'
ALTER TABLE [dbo].[HizmetKampanyaHizmetleri]
ADD CONSTRAINT [PK_HizmetKampanyaHizmetleri]
    PRIMARY KEY CLUSTERED ([hizmetKampanyaHizmetId] ASC);
GO

-- Creating primary key on [hizmetKampanyaId] in table 'HizmetKampanyalari'
ALTER TABLE [dbo].[HizmetKampanyalari]
ADD CONSTRAINT [PK_HizmetKampanyalari]
    PRIMARY KEY CLUSTERED ([hizmetKampanyaId] ASC);
GO

-- Creating primary key on [hizmetId] in table 'Hizmetler'
ALTER TABLE [dbo].[Hizmetler]
ADD CONSTRAINT [PK_Hizmetler]
    PRIMARY KEY CLUSTERED ([hizmetId] ASC);
GO

-- Creating primary key on [hizmetLisansId] in table 'HizmetLisanslari'
ALTER TABLE [dbo].[HizmetLisanslari]
ADD CONSTRAINT [PK_HizmetLisanslari]
    PRIMARY KEY CLUSTERED ([hizmetLisansId] ASC);
GO

-- Creating primary key on [ilceId] in table 'Ilceler'
ALTER TABLE [dbo].[Ilceler]
ADD CONSTRAINT [PK_Ilceler]
    PRIMARY KEY CLUSTERED ([ilceId] ASC);
GO

-- Creating primary key on [ilId] in table 'Iller'
ALTER TABLE [dbo].[Iller]
ADD CONSTRAINT [PK_Iller]
    PRIMARY KEY CLUSTERED ([ilId] ASC);
GO

-- Creating primary key on [kampanyaId] in table 'Kampanyalar'
ALTER TABLE [dbo].[Kampanyalar]
ADD CONSTRAINT [PK_Kampanyalar]
    PRIMARY KEY CLUSTERED ([kampanyaId] ASC);
GO

-- Creating primary key on [kampanyaUrunleriId] in table 'KampanyaUrunleri'
ALTER TABLE [dbo].[KampanyaUrunleri]
ADD CONSTRAINT [PK_KampanyaUrunleri]
    PRIMARY KEY CLUSTERED ([kampanyaUrunleriId] ASC);
GO

-- Creating primary key on [katId] in table 'Katlar'
ALTER TABLE [dbo].[Katlar]
ADD CONSTRAINT [PK_Katlar]
    PRIMARY KEY CLUSTERED ([katId] ASC);
GO

-- Creating primary key on [masaId] in table 'Masalar'
ALTER TABLE [dbo].[Masalar]
ADD CONSTRAINT [PK_Masalar]
    PRIMARY KEY CLUSTERED ([masaId] ASC);
GO

-- Creating primary key on [menuId] in table 'Menuler'
ALTER TABLE [dbo].[Menuler]
ADD CONSTRAINT [PK_Menuler]
    PRIMARY KEY CLUSTERED ([menuId] ASC);
GO

-- Creating primary key on [odemeId] in table 'Odemeler'
ALTER TABLE [dbo].[Odemeler]
ADD CONSTRAINT [PK_Odemeler]
    PRIMARY KEY CLUSTERED ([odemeId] ASC);
GO

-- Creating primary key on [odemeTurId] in table 'OdemeTurleri'
ALTER TABLE [dbo].[OdemeTurleri]
ADD CONSTRAINT [PK_OdemeTurleri]
    PRIMARY KEY CLUSTERED ([odemeTurId] ASC);
GO

-- Creating primary key on [oturumKampanyaId] in table 'OturumKampanyalari'
ALTER TABLE [dbo].[OturumKampanyalari]
ADD CONSTRAINT [PK_OturumKampanyalari]
    PRIMARY KEY CLUSTERED ([oturumKampanyaId] ASC);
GO

-- Creating primary key on [oturumId] in table 'Oturumlar'
ALTER TABLE [dbo].[Oturumlar]
ADD CONSTRAINT [PK_Oturumlar]
    PRIMARY KEY CLUSTERED ([oturumId] ASC);
GO

-- Creating primary key on [paraBirimiId] in table 'ParaBirimleri'
ALTER TABLE [dbo].[ParaBirimleri]
ADD CONSTRAINT [PK_ParaBirimleri]
    PRIMARY KEY CLUSTERED ([paraBirimiId] ASC);
GO

-- Creating primary key on [personelId] in table 'Personeller'
ALTER TABLE [dbo].[Personeller]
ADD CONSTRAINT [PK_Personeller]
    PRIMARY KEY CLUSTERED ([personelId] ASC);
GO

-- Creating primary key on [siparislerId] in table 'Siparisler'
ALTER TABLE [dbo].[Siparisler]
ADD CONSTRAINT [PK_Siparisler]
    PRIMARY KEY CLUSTERED ([siparislerId] ASC);
GO

-- Creating primary key on [siparisUrunlerId] in table 'SiparisUrunleri'
ALTER TABLE [dbo].[SiparisUrunleri]
ADD CONSTRAINT [PK_SiparisUrunleri]
    PRIMARY KEY CLUSTERED ([siparisUrunlerId] ASC);
GO

-- Creating primary key on [siparisUrunEkstraId] in table 'SiparisUrunleriEkstralari'
ALTER TABLE [dbo].[SiparisUrunleriEkstralari]
ADD CONSTRAINT [PK_SiparisUrunleriEkstralari]
    PRIMARY KEY CLUSTERED ([siparisUrunEkstraId] ASC);
GO

-- Creating primary key on [sirketId] in table 'Sirketler'
ALTER TABLE [dbo].[Sirketler]
ADD CONSTRAINT [PK_Sirketler]
    PRIMARY KEY CLUSTERED ([sirketId] ASC);
GO

-- Creating primary key on [subeId] in table 'Subeler'
ALTER TABLE [dbo].[Subeler]
ADD CONSTRAINT [PK_Subeler]
    PRIMARY KEY CLUSTERED ([subeId] ASC);
GO

-- Creating primary key on [ulkeId] in table 'Ulkeler'
ALTER TABLE [dbo].[Ulkeler]
ADD CONSTRAINT [PK_Ulkeler]
    PRIMARY KEY CLUSTERED ([ulkeId] ASC);
GO

-- Creating primary key on [urunEkstraId] in table 'UrunEkstralar'
ALTER TABLE [dbo].[UrunEkstralar]
ADD CONSTRAINT [PK_UrunEkstralar]
    PRIMARY KEY CLUSTERED ([urunEkstraId] ASC);
GO

-- Creating primary key on [urunKategoriId] in table 'UrunKategoriler'
ALTER TABLE [dbo].[UrunKategoriler]
ADD CONSTRAINT [PK_UrunKategoriler]
    PRIMARY KEY CLUSTERED ([urunKategoriId] ASC);
GO

-- Creating primary key on [urunId] in table 'Urunler'
ALTER TABLE [dbo].[Urunler]
ADD CONSTRAINT [PK_Urunler]
    PRIMARY KEY CLUSTERED ([urunId] ASC);
GO

-- Creating primary key on [urunOzellikId] in table 'UrunOzellikler'
ALTER TABLE [dbo].[UrunOzellikler]
ADD CONSTRAINT [PK_UrunOzellikler]
    PRIMARY KEY CLUSTERED ([urunOzellikId] ASC);
GO

-- Creating primary key on [urunStokGirdiId] in table 'UrunStokGirdiler'
ALTER TABLE [dbo].[UrunStokGirdiler]
ADD CONSTRAINT [PK_UrunStokGirdiler]
    PRIMARY KEY CLUSTERED ([urunStokGirdiId] ASC);
GO

-- Creating primary key on [urunStokId] in table 'UrunStoklar'
ALTER TABLE [dbo].[UrunStoklar]
ADD CONSTRAINT [PK_UrunStoklar]
    PRIMARY KEY CLUSTERED ([urunStokId] ASC);
GO

-- Creating primary key on [yazdirmaListeId] in table 'YazdirmaListesi'
ALTER TABLE [dbo].[YazdirmaListesi]
ADD CONSTRAINT [PK_YazdirmaListesi]
    PRIMARY KEY CLUSTERED ([yazdirmaListeId] ASC);
GO

-- Creating primary key on [name], [principal_id], [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([name], [principal_id], [diagram_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [subeId] in table 'Departmanlar'
ALTER TABLE [dbo].[Departmanlar]
ADD CONSTRAINT [FK_Departmanlar_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Departmanlar_Subeler'
CREATE INDEX [IX_FK_Departmanlar_Subeler]
ON [dbo].[Departmanlar]
    ([subeId]);
GO

-- Creating foreign key on [departmanId] in table 'UrunKategoriler'
ALTER TABLE [dbo].[UrunKategoriler]
ADD CONSTRAINT [FK_UrunKategoriler_Departmanlar]
    FOREIGN KEY ([departmanId])
    REFERENCES [dbo].[Departmanlar]
        ([departmanId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrunKategoriler_Departmanlar'
CREATE INDEX [IX_FK_UrunKategoriler_Departmanlar]
ON [dbo].[UrunKategoriler]
    ([departmanId]);
GO

-- Creating foreign key on [subeId] in table 'Duyurular'
ALTER TABLE [dbo].[Duyurular]
ADD CONSTRAINT [FK_Duyurular_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Duyurular_Subeler'
CREATE INDEX [IX_FK_Duyurular_Subeler]
ON [dbo].[Duyurular]
    ([subeId]);
GO

-- Creating foreign key on [faturaId] in table 'FaturadakiHizmetler'
ALTER TABLE [dbo].[FaturadakiHizmetler]
ADD CONSTRAINT [FK_FaturadakiHizmetler_Faturalar]
    FOREIGN KEY ([faturaId])
    REFERENCES [dbo].[Faturalar]
        ([faturaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FaturadakiHizmetler_Faturalar'
CREATE INDEX [IX_FK_FaturadakiHizmetler_Faturalar]
ON [dbo].[FaturadakiHizmetler]
    ([faturaId]);
GO

-- Creating foreign key on [hizmetId] in table 'FaturadakiHizmetler'
ALTER TABLE [dbo].[FaturadakiHizmetler]
ADD CONSTRAINT [FK_FaturadakiHizmetler_Hizmetler]
    FOREIGN KEY ([hizmetId])
    REFERENCES [dbo].[Hizmetler]
        ([hizmetId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FaturadakiHizmetler_Hizmetler'
CREATE INDEX [IX_FK_FaturadakiHizmetler_Hizmetler]
ON [dbo].[FaturadakiHizmetler]
    ([hizmetId]);
GO

-- Creating foreign key on [paraBirimiId] in table 'FaturadakiHizmetler'
ALTER TABLE [dbo].[FaturadakiHizmetler]
ADD CONSTRAINT [FK_FaturadakiHizmetler_ParaBirimleri]
    FOREIGN KEY ([paraBirimiId])
    REFERENCES [dbo].[ParaBirimleri]
        ([paraBirimiId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FaturadakiHizmetler_ParaBirimleri'
CREATE INDEX [IX_FK_FaturadakiHizmetler_ParaBirimleri]
ON [dbo].[FaturadakiHizmetler]
    ([paraBirimiId]);
GO

-- Creating foreign key on [sirketId] in table 'Faturalar'
ALTER TABLE [dbo].[Faturalar]
ADD CONSTRAINT [FK_Faturalar_Sirketler]
    FOREIGN KEY ([sirketId])
    REFERENCES [dbo].[Sirketler]
        ([sirketId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Faturalar_Sirketler'
CREATE INDEX [IX_FK_Faturalar_Sirketler]
ON [dbo].[Faturalar]
    ([sirketId]);
GO

-- Creating foreign key on [faturaId] in table 'FaturaOdemeleri'
ALTER TABLE [dbo].[FaturaOdemeleri]
ADD CONSTRAINT [FK_FaturaOdemeleri_Faturalar]
    FOREIGN KEY ([faturaId])
    REFERENCES [dbo].[Faturalar]
        ([faturaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FaturaOdemeleri_Faturalar'
CREATE INDEX [IX_FK_FaturaOdemeleri_Faturalar]
ON [dbo].[FaturaOdemeleri]
    ([faturaId]);
GO

-- Creating foreign key on [faturaId] in table 'HizmetLisanslari'
ALTER TABLE [dbo].[HizmetLisanslari]
ADD CONSTRAINT [FK_HizmetLisanslari_Faturalar]
    FOREIGN KEY ([faturaId])
    REFERENCES [dbo].[Faturalar]
        ([faturaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HizmetLisanslari_Faturalar'
CREATE INDEX [IX_FK_HizmetLisanslari_Faturalar]
ON [dbo].[HizmetLisanslari]
    ([faturaId]);
GO

-- Creating foreign key on [paraBirimiId] in table 'FaturaOdemeleri'
ALTER TABLE [dbo].[FaturaOdemeleri]
ADD CONSTRAINT [FK_FaturaOdemeleri_ParaBirimleri]
    FOREIGN KEY ([paraBirimiId])
    REFERENCES [dbo].[ParaBirimleri]
        ([paraBirimiId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FaturaOdemeleri_ParaBirimleri'
CREATE INDEX [IX_FK_FaturaOdemeleri_ParaBirimleri]
ON [dbo].[FaturaOdemeleri]
    ([paraBirimiId]);
GO

-- Creating foreign key on [hizmetKampanyaId] in table 'HizmetKampanyaHizmetleri'
ALTER TABLE [dbo].[HizmetKampanyaHizmetleri]
ADD CONSTRAINT [FK_HizmetKampanyaHizmetleri_HizmetKampanyalari]
    FOREIGN KEY ([hizmetKampanyaId])
    REFERENCES [dbo].[HizmetKampanyalari]
        ([hizmetKampanyaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HizmetKampanyaHizmetleri_HizmetKampanyalari'
CREATE INDEX [IX_FK_HizmetKampanyaHizmetleri_HizmetKampanyalari]
ON [dbo].[HizmetKampanyaHizmetleri]
    ([hizmetKampanyaId]);
GO

-- Creating foreign key on [hizmetId] in table 'HizmetKampanyaHizmetleri'
ALTER TABLE [dbo].[HizmetKampanyaHizmetleri]
ADD CONSTRAINT [FK_HizmetKampanyaHizmetleri_Hizmetler]
    FOREIGN KEY ([hizmetId])
    REFERENCES [dbo].[Hizmetler]
        ([hizmetId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HizmetKampanyaHizmetleri_Hizmetler'
CREATE INDEX [IX_FK_HizmetKampanyaHizmetleri_Hizmetler]
ON [dbo].[HizmetKampanyaHizmetleri]
    ([hizmetId]);
GO

-- Creating foreign key on [paraBirimiId] in table 'HizmetKampanyalari'
ALTER TABLE [dbo].[HizmetKampanyalari]
ADD CONSTRAINT [FK_HizmetKampanyalari_ParaBirimleri]
    FOREIGN KEY ([paraBirimiId])
    REFERENCES [dbo].[ParaBirimleri]
        ([paraBirimiId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HizmetKampanyalari_ParaBirimleri'
CREATE INDEX [IX_FK_HizmetKampanyalari_ParaBirimleri]
ON [dbo].[HizmetKampanyalari]
    ([paraBirimiId]);
GO

-- Creating foreign key on [paraBirimiId] in table 'Hizmetler'
ALTER TABLE [dbo].[Hizmetler]
ADD CONSTRAINT [FK_Hizmetler_ParaBirimleri]
    FOREIGN KEY ([paraBirimiId])
    REFERENCES [dbo].[ParaBirimleri]
        ([paraBirimiId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Hizmetler_ParaBirimleri'
CREATE INDEX [IX_FK_Hizmetler_ParaBirimleri]
ON [dbo].[Hizmetler]
    ([paraBirimiId]);
GO

-- Creating foreign key on [hizmetId] in table 'HizmetLisanslari'
ALTER TABLE [dbo].[HizmetLisanslari]
ADD CONSTRAINT [FK_HizmetLisanslari_Hizmetler]
    FOREIGN KEY ([hizmetId])
    REFERENCES [dbo].[Hizmetler]
        ([hizmetId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HizmetLisanslari_Hizmetler'
CREATE INDEX [IX_FK_HizmetLisanslari_Hizmetler]
ON [dbo].[HizmetLisanslari]
    ([hizmetId]);
GO

-- Creating foreign key on [subeId] in table 'HizmetLisanslari'
ALTER TABLE [dbo].[HizmetLisanslari]
ADD CONSTRAINT [FK_HizmetLisanslari_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HizmetLisanslari_Subeler'
CREATE INDEX [IX_FK_HizmetLisanslari_Subeler]
ON [dbo].[HizmetLisanslari]
    ([subeId]);
GO

-- Creating foreign key on [ilId] in table 'Ilceler'
ALTER TABLE [dbo].[Ilceler]
ADD CONSTRAINT [FK_Ilceler_Iller]
    FOREIGN KEY ([ilId])
    REFERENCES [dbo].[Iller]
        ([ilId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ilceler_Iller'
CREATE INDEX [IX_FK_Ilceler_Iller]
ON [dbo].[Ilceler]
    ([ilId]);
GO

-- Creating foreign key on [ilceId] in table 'Sirketler'
ALTER TABLE [dbo].[Sirketler]
ADD CONSTRAINT [FK_Sirketler_Ilceler]
    FOREIGN KEY ([ilceId])
    REFERENCES [dbo].[Ilceler]
        ([ilceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sirketler_Ilceler'
CREATE INDEX [IX_FK_Sirketler_Ilceler]
ON [dbo].[Sirketler]
    ([ilceId]);
GO

-- Creating foreign key on [ilceId] in table 'Subeler'
ALTER TABLE [dbo].[Subeler]
ADD CONSTRAINT [FK_Subeler_Ilceler]
    FOREIGN KEY ([ilceId])
    REFERENCES [dbo].[Ilceler]
        ([ilceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Subeler_Ilceler'
CREATE INDEX [IX_FK_Subeler_Ilceler]
ON [dbo].[Subeler]
    ([ilceId]);
GO

-- Creating foreign key on [ulkeId] in table 'Iller'
ALTER TABLE [dbo].[Iller]
ADD CONSTRAINT [FK_Iller_Ulkeler]
    FOREIGN KEY ([ulkeId])
    REFERENCES [dbo].[Ulkeler]
        ([ulkeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Iller_Ulkeler'
CREATE INDEX [IX_FK_Iller_Ulkeler]
ON [dbo].[Iller]
    ([ulkeId]);
GO

-- Creating foreign key on [menuId] in table 'Kampanyalar'
ALTER TABLE [dbo].[Kampanyalar]
ADD CONSTRAINT [FK_Kampanyalar_Menuler]
    FOREIGN KEY ([menuId])
    REFERENCES [dbo].[Menuler]
        ([menuId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Kampanyalar_Menuler'
CREATE INDEX [IX_FK_Kampanyalar_Menuler]
ON [dbo].[Kampanyalar]
    ([menuId]);
GO

-- Creating foreign key on [subeId] in table 'Kampanyalar'
ALTER TABLE [dbo].[Kampanyalar]
ADD CONSTRAINT [FK_Kampanyalar_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Kampanyalar_Subeler'
CREATE INDEX [IX_FK_Kampanyalar_Subeler]
ON [dbo].[Kampanyalar]
    ([subeId]);
GO

-- Creating foreign key on [kampanyaId] in table 'KampanyaUrunleri'
ALTER TABLE [dbo].[KampanyaUrunleri]
ADD CONSTRAINT [FK_KampanyaUrunleri_Kampanyalar]
    FOREIGN KEY ([kampanyaId])
    REFERENCES [dbo].[Kampanyalar]
        ([kampanyaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KampanyaUrunleri_Kampanyalar'
CREATE INDEX [IX_FK_KampanyaUrunleri_Kampanyalar]
ON [dbo].[KampanyaUrunleri]
    ([kampanyaId]);
GO

-- Creating foreign key on [kampanyaId] in table 'OturumKampanyalari'
ALTER TABLE [dbo].[OturumKampanyalari]
ADD CONSTRAINT [FK_OturumKampanyalari_Kampanyalar]
    FOREIGN KEY ([kampanyaId])
    REFERENCES [dbo].[Kampanyalar]
        ([kampanyaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OturumKampanyalari_Kampanyalar'
CREATE INDEX [IX_FK_OturumKampanyalari_Kampanyalar]
ON [dbo].[OturumKampanyalari]
    ([kampanyaId]);
GO

-- Creating foreign key on [urunId] in table 'KampanyaUrunleri'
ALTER TABLE [dbo].[KampanyaUrunleri]
ADD CONSTRAINT [FK_KampanyaUrunleri_Urunler]
    FOREIGN KEY ([urunId])
    REFERENCES [dbo].[Urunler]
        ([urunId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KampanyaUrunleri_Urunler'
CREATE INDEX [IX_FK_KampanyaUrunleri_Urunler]
ON [dbo].[KampanyaUrunleri]
    ([urunId]);
GO

-- Creating foreign key on [subeId] in table 'Katlar'
ALTER TABLE [dbo].[Katlar]
ADD CONSTRAINT [FK_Katlar_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Katlar_Subeler'
CREATE INDEX [IX_FK_Katlar_Subeler]
ON [dbo].[Katlar]
    ([subeId]);
GO

-- Creating foreign key on [katId] in table 'Masalar'
ALTER TABLE [dbo].[Masalar]
ADD CONSTRAINT [FK_Masalar_Katlar]
    FOREIGN KEY ([katId])
    REFERENCES [dbo].[Katlar]
        ([katId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Masalar_Katlar'
CREATE INDEX [IX_FK_Masalar_Katlar]
ON [dbo].[Masalar]
    ([katId]);
GO

-- Creating foreign key on [masaId] in table 'Oturumlar'
ALTER TABLE [dbo].[Oturumlar]
ADD CONSTRAINT [FK_Oturumlar_Masalar]
    FOREIGN KEY ([masaId])
    REFERENCES [dbo].[Masalar]
        ([masaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Oturumlar_Masalar'
CREATE INDEX [IX_FK_Oturumlar_Masalar]
ON [dbo].[Oturumlar]
    ([masaId]);
GO

-- Creating foreign key on [paraBirimiId] in table 'Menuler'
ALTER TABLE [dbo].[Menuler]
ADD CONSTRAINT [FK_Menuler_ParaBirimleri]
    FOREIGN KEY ([paraBirimiId])
    REFERENCES [dbo].[ParaBirimleri]
        ([paraBirimiId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Menuler_ParaBirimleri'
CREATE INDEX [IX_FK_Menuler_ParaBirimleri]
ON [dbo].[Menuler]
    ([paraBirimiId]);
GO

-- Creating foreign key on [subeId] in table 'Menuler'
ALTER TABLE [dbo].[Menuler]
ADD CONSTRAINT [FK_Menuler_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Menuler_Subeler'
CREATE INDEX [IX_FK_Menuler_Subeler]
ON [dbo].[Menuler]
    ([subeId]);
GO

-- Creating foreign key on [menuId] in table 'UrunKategoriler'
ALTER TABLE [dbo].[UrunKategoriler]
ADD CONSTRAINT [FK_UrunKategoriler_Menuler]
    FOREIGN KEY ([menuId])
    REFERENCES [dbo].[Menuler]
        ([menuId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrunKategoriler_Menuler'
CREATE INDEX [IX_FK_UrunKategoriler_Menuler]
ON [dbo].[UrunKategoriler]
    ([menuId]);
GO

-- Creating foreign key on [odemeTurId] in table 'Odemeler'
ALTER TABLE [dbo].[Odemeler]
ADD CONSTRAINT [FK_Odemeler_OdemeTurleri1]
    FOREIGN KEY ([odemeTurId])
    REFERENCES [dbo].[OdemeTurleri]
        ([odemeTurId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Odemeler_OdemeTurleri1'
CREATE INDEX [IX_FK_Odemeler_OdemeTurleri1]
ON [dbo].[Odemeler]
    ([odemeTurId]);
GO

-- Creating foreign key on [oturumId] in table 'Odemeler'
ALTER TABLE [dbo].[Odemeler]
ADD CONSTRAINT [FK_Odemeler_Oturumlar]
    FOREIGN KEY ([oturumId])
    REFERENCES [dbo].[Oturumlar]
        ([oturumId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Odemeler_Oturumlar'
CREATE INDEX [IX_FK_Odemeler_Oturumlar]
ON [dbo].[Odemeler]
    ([oturumId]);
GO

-- Creating foreign key on [subeId] in table 'Odemeler'
ALTER TABLE [dbo].[Odemeler]
ADD CONSTRAINT [FK_Odemeler_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Odemeler_Subeler'
CREATE INDEX [IX_FK_Odemeler_Subeler]
ON [dbo].[Odemeler]
    ([subeId]);
GO

-- Creating foreign key on [paraBirimiId] in table 'OdemeTurleri'
ALTER TABLE [dbo].[OdemeTurleri]
ADD CONSTRAINT [FK_OdemeTurleri_ParaBirimleri]
    FOREIGN KEY ([paraBirimiId])
    REFERENCES [dbo].[ParaBirimleri]
        ([paraBirimiId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OdemeTurleri_ParaBirimleri'
CREATE INDEX [IX_FK_OdemeTurleri_ParaBirimleri]
ON [dbo].[OdemeTurleri]
    ([paraBirimiId]);
GO

-- Creating foreign key on [subeId] in table 'OdemeTurleri'
ALTER TABLE [dbo].[OdemeTurleri]
ADD CONSTRAINT [FK_OdemeTurleri_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OdemeTurleri_Subeler'
CREATE INDEX [IX_FK_OdemeTurleri_Subeler]
ON [dbo].[OdemeTurleri]
    ([subeId]);
GO

-- Creating foreign key on [oturumId] in table 'OturumKampanyalari'
ALTER TABLE [dbo].[OturumKampanyalari]
ADD CONSTRAINT [FK_OturumKampanyalari_Oturumlar]
    FOREIGN KEY ([oturumId])
    REFERENCES [dbo].[Oturumlar]
        ([oturumId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OturumKampanyalari_Oturumlar'
CREATE INDEX [IX_FK_OturumKampanyalari_Oturumlar]
ON [dbo].[OturumKampanyalari]
    ([oturumId]);
GO

-- Creating foreign key on [oturumKampanyaId] in table 'SiparisUrunleri'
ALTER TABLE [dbo].[SiparisUrunleri]
ADD CONSTRAINT [FK_SiparisUrunleri_OturumKampanyalari]
    FOREIGN KEY ([oturumKampanyaId])
    REFERENCES [dbo].[OturumKampanyalari]
        ([oturumKampanyaId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiparisUrunleri_OturumKampanyalari'
CREATE INDEX [IX_FK_SiparisUrunleri_OturumKampanyalari]
ON [dbo].[SiparisUrunleri]
    ([oturumKampanyaId]);
GO

-- Creating foreign key on [subeId] in table 'Oturumlar'
ALTER TABLE [dbo].[Oturumlar]
ADD CONSTRAINT [FK_Oturumlar_Subeler1]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Oturumlar_Subeler1'
CREATE INDEX [IX_FK_Oturumlar_Subeler1]
ON [dbo].[Oturumlar]
    ([subeId]);
GO

-- Creating foreign key on [oturumId] in table 'Siparisler'
ALTER TABLE [dbo].[Siparisler]
ADD CONSTRAINT [FK_Siparisler_Oturumlar]
    FOREIGN KEY ([oturumId])
    REFERENCES [dbo].[Oturumlar]
        ([oturumId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Siparisler_Oturumlar'
CREATE INDEX [IX_FK_Siparisler_Oturumlar]
ON [dbo].[Siparisler]
    ([oturumId]);
GO

-- Creating foreign key on [subeId] in table 'Personeller'
ALTER TABLE [dbo].[Personeller]
ADD CONSTRAINT [FK_Personeller_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Personeller_Subeler'
CREATE INDEX [IX_FK_Personeller_Subeler]
ON [dbo].[Personeller]
    ([subeId]);
GO

-- Creating foreign key on [personelId] in table 'Siparisler'
ALTER TABLE [dbo].[Siparisler]
ADD CONSTRAINT [FK_Siparisler_Personeller]
    FOREIGN KEY ([personelId])
    REFERENCES [dbo].[Personeller]
        ([personelId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Siparisler_Personeller'
CREATE INDEX [IX_FK_Siparisler_Personeller]
ON [dbo].[Siparisler]
    ([personelId]);
GO

-- Creating foreign key on [subeId] in table 'Siparisler'
ALTER TABLE [dbo].[Siparisler]
ADD CONSTRAINT [FK_Siparisler_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Siparisler_Subeler'
CREATE INDEX [IX_FK_Siparisler_Subeler]
ON [dbo].[Siparisler]
    ([subeId]);
GO

-- Creating foreign key on [siparislerId] in table 'SiparisUrunleri'
ALTER TABLE [dbo].[SiparisUrunleri]
ADD CONSTRAINT [FK_SiparisUrunleri_Siparisler]
    FOREIGN KEY ([siparislerId])
    REFERENCES [dbo].[Siparisler]
        ([siparislerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiparisUrunleri_Siparisler'
CREATE INDEX [IX_FK_SiparisUrunleri_Siparisler]
ON [dbo].[SiparisUrunleri]
    ([siparislerId]);
GO

-- Creating foreign key on [siparislerId] in table 'YazdirmaListesi'
ALTER TABLE [dbo].[YazdirmaListesi]
ADD CONSTRAINT [FK_YazdirmaListesi_Siparisler]
    FOREIGN KEY ([siparislerId])
    REFERENCES [dbo].[Siparisler]
        ([siparislerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_YazdirmaListesi_Siparisler'
CREATE INDEX [IX_FK_YazdirmaListesi_Siparisler]
ON [dbo].[YazdirmaListesi]
    ([siparislerId]);
GO

-- Creating foreign key on [urunId] in table 'SiparisUrunleri'
ALTER TABLE [dbo].[SiparisUrunleri]
ADD CONSTRAINT [FK_SiparisUrunleri_Urunler]
    FOREIGN KEY ([urunId])
    REFERENCES [dbo].[Urunler]
        ([urunId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiparisUrunleri_Urunler'
CREATE INDEX [IX_FK_SiparisUrunleri_Urunler]
ON [dbo].[SiparisUrunleri]
    ([urunId]);
GO

-- Creating foreign key on [urunOzellikId] in table 'SiparisUrunleri'
ALTER TABLE [dbo].[SiparisUrunleri]
ADD CONSTRAINT [FK_SiparisUrunleri_UrunOzellikler]
    FOREIGN KEY ([urunOzellikId])
    REFERENCES [dbo].[UrunOzellikler]
        ([urunOzellikId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiparisUrunleri_UrunOzellikler'
CREATE INDEX [IX_FK_SiparisUrunleri_UrunOzellikler]
ON [dbo].[SiparisUrunleri]
    ([urunOzellikId]);
GO

-- Creating foreign key on [siparisUrunlerId] in table 'SiparisUrunleriEkstralari'
ALTER TABLE [dbo].[SiparisUrunleriEkstralari]
ADD CONSTRAINT [FK_SiparisUrunleriEkstralari_SiparisUrunleri]
    FOREIGN KEY ([siparisUrunlerId])
    REFERENCES [dbo].[SiparisUrunleri]
        ([siparisUrunlerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiparisUrunleriEkstralari_SiparisUrunleri'
CREATE INDEX [IX_FK_SiparisUrunleriEkstralari_SiparisUrunleri]
ON [dbo].[SiparisUrunleriEkstralari]
    ([siparisUrunlerId]);
GO

-- Creating foreign key on [urunEkstraId] in table 'SiparisUrunleriEkstralari'
ALTER TABLE [dbo].[SiparisUrunleriEkstralari]
ADD CONSTRAINT [FK_SiparisUrunleriEkstralari_UrunEkstralar]
    FOREIGN KEY ([urunEkstraId])
    REFERENCES [dbo].[UrunEkstralar]
        ([urunEkstraId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiparisUrunleriEkstralari_UrunEkstralar'
CREATE INDEX [IX_FK_SiparisUrunleriEkstralari_UrunEkstralar]
ON [dbo].[SiparisUrunleriEkstralari]
    ([urunEkstraId]);
GO

-- Creating foreign key on [sirketId] in table 'Subeler'
ALTER TABLE [dbo].[Subeler]
ADD CONSTRAINT [FK_Subeler_Sirketler]
    FOREIGN KEY ([sirketId])
    REFERENCES [dbo].[Sirketler]
        ([sirketId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Subeler_Sirketler'
CREATE INDEX [IX_FK_Subeler_Sirketler]
ON [dbo].[Subeler]
    ([sirketId]);
GO

-- Creating foreign key on [subeId] in table 'Urunler'
ALTER TABLE [dbo].[Urunler]
ADD CONSTRAINT [FK_Urunler_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Urunler_Subeler'
CREATE INDEX [IX_FK_Urunler_Subeler]
ON [dbo].[Urunler]
    ([subeId]);
GO

-- Creating foreign key on [subeId] in table 'UrunStoklar'
ALTER TABLE [dbo].[UrunStoklar]
ADD CONSTRAINT [FK_UrunStoklar_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrunStoklar_Subeler'
CREATE INDEX [IX_FK_UrunStoklar_Subeler]
ON [dbo].[UrunStoklar]
    ([subeId]);
GO

-- Creating foreign key on [subeId] in table 'YazdirmaListesi'
ALTER TABLE [dbo].[YazdirmaListesi]
ADD CONSTRAINT [FK_YazdirmaListesi_Subeler]
    FOREIGN KEY ([subeId])
    REFERENCES [dbo].[Subeler]
        ([subeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_YazdirmaListesi_Subeler'
CREATE INDEX [IX_FK_YazdirmaListesi_Subeler]
ON [dbo].[YazdirmaListesi]
    ([subeId]);
GO

-- Creating foreign key on [urunId] in table 'UrunEkstralar'
ALTER TABLE [dbo].[UrunEkstralar]
ADD CONSTRAINT [FK_UrunEkstralar_Urunler]
    FOREIGN KEY ([urunId])
    REFERENCES [dbo].[Urunler]
        ([urunId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrunEkstralar_Urunler'
CREATE INDEX [IX_FK_UrunEkstralar_Urunler]
ON [dbo].[UrunEkstralar]
    ([urunId]);
GO

-- Creating foreign key on [urunKategoriId] in table 'Urunler'
ALTER TABLE [dbo].[Urunler]
ADD CONSTRAINT [FK_Urunler_UrunKategoriler]
    FOREIGN KEY ([urunKategoriId])
    REFERENCES [dbo].[UrunKategoriler]
        ([urunKategoriId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Urunler_UrunKategoriler'
CREATE INDEX [IX_FK_Urunler_UrunKategoriler]
ON [dbo].[Urunler]
    ([urunKategoriId]);
GO

-- Creating foreign key on [urunId] in table 'UrunOzellikler'
ALTER TABLE [dbo].[UrunOzellikler]
ADD CONSTRAINT [FK_UrunOzellikler_Urunler]
    FOREIGN KEY ([urunId])
    REFERENCES [dbo].[Urunler]
        ([urunId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrunOzellikler_Urunler'
CREATE INDEX [IX_FK_UrunOzellikler_Urunler]
ON [dbo].[UrunOzellikler]
    ([urunId]);
GO

-- Creating foreign key on [urunId] in table 'UrunStoklar'
ALTER TABLE [dbo].[UrunStoklar]
ADD CONSTRAINT [FK_UrunStoklar_Urunler]
    FOREIGN KEY ([urunId])
    REFERENCES [dbo].[Urunler]
        ([urunId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrunStoklar_Urunler'
CREATE INDEX [IX_FK_UrunStoklar_Urunler]
ON [dbo].[UrunStoklar]
    ([urunId]);
GO

-- Creating foreign key on [urunStokId] in table 'UrunStokGirdiler'
ALTER TABLE [dbo].[UrunStokGirdiler]
ADD CONSTRAINT [FK_UrunStokGirdiler_UrunStoklar]
    FOREIGN KEY ([urunStokId])
    REFERENCES [dbo].[UrunStoklar]
        ([urunStokId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UrunStokGirdiler_UrunStoklar'
CREATE INDEX [IX_FK_UrunStokGirdiler_UrunStoklar]
ON [dbo].[UrunStokGirdiler]
    ([urunStokId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------