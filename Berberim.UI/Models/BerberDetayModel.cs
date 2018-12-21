using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Berberim.Data.Models;

namespace Berberim.UI.Models
{
    public class BerberDetayModel
    {
        public SALONSAYFA Berber { get; set; }
        public List<PERSONEL> Personeller { get; set; }
        public List<SALONFOTO> SalonFotolar { get; set; }
        public List<BSACMODEL> KesilenSacModeller { get; set; }
        public List<ISLEM> Islemler { get; set; }
        public List<YORUM> MusteriYorumlar { get; set; }
        public List<string> RandevuSaat { get; set; }
        public List<string> RandevuTarih { get; set; }
        public KAMPANYA Kampanya { get; set; }
        public List<SALONSAYFA> Salonlar { get; set; }
        public SALONLOGO SalonLogo { get; set; }

    }
}