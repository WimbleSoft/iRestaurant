
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
    
public partial class Odemeler
{

    public int odemeId { get; set; }

    public int oturumId { get; set; }

    public int odemeTurId { get; set; }

    public double odemeMiktar { get; set; }

    public System.DateTime odemeTarihi { get; set; }

    public int subeId { get; set; }

    public bool silindi { get; set; }



    public virtual OdemeTurleri OdemeTurleri { get; set; }

    public virtual Oturumlar Oturumlar { get; set; }

    public virtual Subeler Subeler { get; set; }

}

}
