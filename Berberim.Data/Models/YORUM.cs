using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class YORUM
    {
        public int ID { get; set; }
        public int STATUS { get; set; }
        public int SALONID { get; set; }
        public int MUSTERIID { get; set; }
        public string MUSTERIAD { get; set; }
        public string MUSTERISOYAD { get; set; }
        public string MYORUM { get; set; }
        public string CİNSİYET { get; set; }
        public string TARİH { get; set; }

        public ICollection<MUSTERI> MUSTERİ { get; set; }

    }
}
