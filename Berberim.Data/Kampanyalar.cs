//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Berberim.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Kampanyalar
    {
        public int id { get; set; }
        public Nullable<int> BerberId { get; set; }
        public string SalonAd { get; set; }
        public string KampanyaBaslik { get; set; }
        public string KampanyaIcerik { get; set; }
        public Nullable<decimal> KampanyaFiyat { get; set; }
        public Nullable<System.DateTime> KampanyaSonGun { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual BerberSayfa BerberSayfa { get; set; }
    }
}
