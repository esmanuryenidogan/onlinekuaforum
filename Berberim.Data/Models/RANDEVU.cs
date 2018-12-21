using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class RANDEVU
    {
        public int ID { get; set; }
        public int SALONID { get; set; }
        public int STATUS { get; set; }    
        public int MUSTERIID { get; set; }
        public int ISLEMID { get; set; }
        public string SALONAD { get; set; }
        public int KOLTUKSAY { get; set; }
        public string SALONTEL { get; set; }
        public string SALONMAIL { get; set; }
        public string PERSONEL { get; set; }
        public string MUSTERIAD { get; set; }
        public string MUSTERISOYAD { get; set; }
        public string MUSTERITEL { get; set; }
        public string MUSTERIMAIL { get; set; }
        public DateTime RANDEVUTARIH { get; set; }
        public string RANDEVUSAAT { get; set; }
        public string ISLEMADI { get; set; }
        public string ISLEMFIYAT { get; set; }

        public ICollection<SALON> SALON { get; set; }
        

    }
}
