using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berberim.UI.Models
{
    public class BerberDetayModel
    {

        

        public BerberSayfa Berber { get; set; }
        public List<Personel> Personeller  { get; set; }
        public List<SalonFotolar> SalonFotolar { get; set; }
        public List<KesilenSacModeller> KesilenSacModeller { get; set; }
        public List<Islemler> islemler { get; set; }
        public List<MusteriYorumlari> MusteriYorumlar { get; set; }
        public List<string> RandevuSaat { get; set; }
        public List<string> RandevuTarih { get; set; }

    }
}