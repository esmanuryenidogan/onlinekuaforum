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
    
    public partial class SALON
    {
        public SALON()
        {
            this.BSACMODEL = new HashSet<BSACMODEL>();
            this.ISLEM = new HashSet<ISLEM>();
            this.KAMPANYA = new HashSet<KAMPANYA>();
            this.PERSONEL = new HashSet<PERSONEL>();
            this.RANDEVU = new HashSet<RANDEVU>();
            this.SALONFOTO = new HashSet<SALONFOTO>();
            this.SALONSAYFA = new HashSet<SALONSAYFA>();
            this.YORUM = new HashSet<YORUM>();
        }
    
        public int ID { get; set; }
        public int STATUS { get; set; }
        public string SALONADI { get; set; }
        public string EMAIL { get; set; }
        public string SIFRE { get; set; }
        public System.DateTime CREATEDATE { get; set; }
    
        public virtual ICollection<BSACMODEL> BSACMODEL { get; set; }
        public virtual ICollection<ISLEM> ISLEM { get; set; }
        public virtual ICollection<KAMPANYA> KAMPANYA { get; set; }
        public virtual ICollection<PERSONEL> PERSONEL { get; set; }
        public virtual ICollection<RANDEVU> RANDEVU { get; set; }
        public virtual ICollection<SALONFOTO> SALONFOTO { get; set; }
        public virtual ICollection<SALONSAYFA> SALONSAYFA { get; set; }
        public virtual ICollection<YORUM> YORUM { get; set; }
    }
}