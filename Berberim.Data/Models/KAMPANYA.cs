using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class KAMPANYA
    {
        public int ID { get; set; }
        public int SALONID { get; set; }
        public int STATUS { get; set; }
        public string SALONAD { get; set; }
        public string BASLIK { get; set; }
        public string ICERIK { get; set; }
        public int FIYAT { get; set; }
        public DateTime SONGUN { get; set; }

        public ICollection<SALON> SALON { get; set; }

    }
}
