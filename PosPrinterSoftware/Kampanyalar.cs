
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
    
public partial class Kampanyalar
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Kampanyalar()
    {

        this.KampanyaUrunleri = new HashSet<KampanyaUrunleri>();

        this.OturumKampanyalari = new HashSet<OturumKampanyalari>();

    }


    public int kampanyaId { get; set; }

    public string kampanyaAd { get; set; }

    public double kampanyaFiyat { get; set; }

    public string kampanyaResim { get; set; }

    public string kampanyaAciklama { get; set; }

    public bool yayindaMi { get; set; }

    public int subeId { get; set; }

    public int menuId { get; set; }

    public bool silindi { get; set; }



    public virtual Menuler Menuler { get; set; }

    public virtual Subeler Subeler { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<KampanyaUrunleri> KampanyaUrunleri { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OturumKampanyalari> OturumKampanyalari { get; set; }

}

}
