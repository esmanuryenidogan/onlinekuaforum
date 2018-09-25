using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class SALON
    {
        public int ID { get; set; }
        public int STATUS { get; set; }
        public string ADSOYAD { get; set; }
        public string EMAIL { get; set; }
        public string TELEFON { get; set; }
        public DateTime CREATEDATE { get; set; }
        public string SIFRE { get; set; }

         public int SALONID { get; set; }

        public BSACMODEL BSACMODEL { get; set; }
    }
}
