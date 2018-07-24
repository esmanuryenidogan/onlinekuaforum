using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class PERSONEL
    {
        public int ID { get; set; }
        public int SALONID { get; set; }
        public string ADSOYAD { get; set; }
        public string UNVAN { get; set; }
        public string CINSIYET { get; set; }
        public string FOTO { get; set; }
        public int STATUS { get; set; }

        public ICollection<SALON> SALON { get; set; }

    }
}
