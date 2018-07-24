using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class MUSTERİ
    {
        public int ID { get; set; }
        public int STATUS { get; set; }
        public string AD { get; set; }
        public string SOYAD { get; set; }
        public string ADRES { get; set; }
        public string EMAIL { get; set; }
        public string TEL { get; set; }
        public DateTime DOGUMTARIHI { get; set; }
        public string SIFRE { get; set; }
        public string FOTO { get; set; }

        public int MUSTERIID { get; set; }
        public RANDEVU RANDEVU { get; set; }
    }
}
