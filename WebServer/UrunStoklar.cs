
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace iRestaurant
{

using System;
    using System.Collections.Generic;
    
public partial class UrunStoklar
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UrunStoklar()
    {

        this.UrunStokGirdiler = new HashSet<UrunStokGirdiler>();

    }


    public int urunStokId { get; set; }

    public int urunId { get; set; }

    public int subeId { get; set; }

    public double adet { get; set; }

    public bool silindi { get; set; }



    public virtual Subeler Subeler { get; set; }

    public virtual Urunler Urunler { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UrunStokGirdiler> UrunStokGirdiler { get; set; }

}

}
