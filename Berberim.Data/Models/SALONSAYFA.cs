using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class SALONSAYFA
    {
        public int ID { get; set; }
        public int STATUS { get; set; }
        public int SALONID { get; set; }
        public string AD { get; set; }
        public string ADRES { get; set; }
        public string IL { get; set; }
        public string ILCE { get; set; }
        public string EMAIL { get; set; }
        public string TEL { get; set; }
        public string VITRINFOTO { get; set; }
        public string VITRINYAZI { get; set; }
        public string HAKKINDA { get; set; }
        public string CADDE { get; set; }
        public string MAHALLE { get; set; }
        public string NOPOSTAKODU { get; set; }
        public string ACIKADRES { get; set; }
        public string FACEBOOK { get; set; }
        public string INSTGRAM { get; set; }
        public string TWITTER { get; set; }
        public string LOGO { get; set; }

        public ICollection<SALON> SALON { get; set; }

    }
}
