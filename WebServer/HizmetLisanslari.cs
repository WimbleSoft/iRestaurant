
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
    
public partial class HizmetLisanslari
{

    public int hizmetLisansId { get; set; }

    public System.DateTime hizmetLisansBaslangicTarihi { get; set; }

    public System.DateTime hizmetLisansBitisTarihi { get; set; }

    public int hizmetId { get; set; }

    public Nullable<int> subeId { get; set; }

    public int faturaId { get; set; }

    public bool silindi { get; set; }



    public virtual Faturalar Faturalar { get; set; }

    public virtual Hizmetler Hizmetler { get; set; }

    public virtual Subeler Subeler { get; set; }

}

}
