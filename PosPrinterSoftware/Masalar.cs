
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
    
public partial class Masalar
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Masalar()
    {

        this.Oturumlar = new HashSet<Oturumlar>();

    }


    public int masaId { get; set; }

    public int katId { get; set; }

    public string masaAd { get; set; }

    public bool durum { get; set; }

    public bool silindi { get; set; }



    public virtual Katlar Katlar { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Oturumlar> Oturumlar { get; set; }

}

}
