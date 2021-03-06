
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace iRestaurantPosPrinterSoftware
{

using System;
    using System.Collections.Generic;
    
public partial class Urunler
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Urunler()
    {

        this.KampanyaUrunleri = new HashSet<KampanyaUrunleri>();

        this.SiparisUrunleri = new HashSet<SiparisUrunleri>();

        this.UrunEkstralar = new HashSet<UrunEkstralar>();

        this.UrunOzellikler = new HashSet<UrunOzellikler>();

        this.UrunStoklar = new HashSet<UrunStoklar>();

    }


    public int urunId { get; set; }

    public int urunKategoriId { get; set; }

    public string urunAd { get; set; }

    public double urunFiyat { get; set; }

    public string urunResim { get; set; }

    public string urunAciklama { get; set; }

    public Nullable<short> urunYapimSuresi { get; set; }

    public bool yayinda { get; set; }

    public int subeId { get; set; }

    public double urunStokAlarmAdet { get; set; }

    public bool silindi { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<KampanyaUrunleri> KampanyaUrunleri { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SiparisUrunleri> SiparisUrunleri { get; set; }

    public virtual Subeler Subeler { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UrunEkstralar> UrunEkstralar { get; set; }

    public virtual UrunKategoriler UrunKategoriler { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UrunOzellikler> UrunOzellikler { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UrunStoklar> UrunStoklar { get; set; }

}

}
