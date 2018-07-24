using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class BSACMODEL
    {
        public int ID { get; set; }
        public int SALONID { get; set; }
        public int STATUS { get; set; }
        public string FOTO { get; set; }

        public ICollection<SALON> SALON { get; set; }
    }
}
