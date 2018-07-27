﻿using System;
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

        public ICollection<MUSTERİ> MUSTERİ { get; set; }

    }
}