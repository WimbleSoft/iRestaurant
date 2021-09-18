namespace iRestaurant.Controllers
{
    using System.Collections.Generic;

    public class ViewModelDemoVM
    {
        
        public List<Departmanlar> Departmanlar { get; set; }
        public List<Duyurular> Duyurular { get; set; }
        public List<Faturalar> Faturalar { get; set; }
        public List<HizmetKampanyaHizmetleri> HizmetKampanyaHizmetleri { get; set; }
        public List<HizmetKampanyalari> HizmetKampanyalari { get; set; }
        public List<Hizmetler> Hizmetler { get; set; }
        public List<HizmetLisanslari> HizmetLisanslari { get; set; }
        public List<Ilceler> Ilceler { get; set; }
        public List<Iller> Iller { get; set; }
        public List<Kampanyalar> Kampanyalar { get; set; }
        public List<KampanyaUrunleri> KampanyaUrunleri { get; set; }
        public List<Katlar> Katlar { get; set; }
        public List<FaturadakiHizmetler> FaturadakiHizmetler { get; set; }
        public List<FaturaOdemeleri> FaturaOdemeleri { get; set; }
        public List<Masalar> Masalar { get; set; }
        public List<Menuler> Menuler { get; set; }
        public List<Odemeler> Odemeler { get; set; }
        public List<OdemeTurleri> OdemeTurleri { get; set; }
        public List<OturumKampanyalari> OturumKampanyalari { get; set; }
        public List<Oturumlar> Oturumlar { get; set; }
        public List<ParaBirimleri> ParaBirimleri { get; set; }
        public List<Personeller> Personeller { get; set; }
        public List<Siparisler> Siparisler { get; set; }
        public List<SiparisUrunleri> SiparisUrunleri { get; set; }
        public List<SiparisUrunleriEkstralari> SiparisUrunleriEkstralari { get; set; }
        public List<Sirketler> Sirketler { get; set; }
        public List<Subeler> Subeler { get; set; }
        public List<Ulkeler> Ulkeler { get; set; }
        public List<UrunEkstralar> UrunEkstralar { get; set; }
        public List<UrunKategoriler> UrunKategoriler { get; set; }
        public List<Urunler> Urunler { get; set; }
        public List<UrunOzellikler> UrunOzellikler { get; set; }
        public List<UrunStokGirdiler> UrunStokGirdiler { get; set; }
        public List<UrunStoklar> UrunStoklar { get; set; }
        public List<YazdirmaListesi> YazdirmaListesi { get; set; }
        public List<HomeController.SepettekiHizmet> SepettekiHizmetler { get; set; }
    }
}