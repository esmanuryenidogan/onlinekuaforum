using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Data.Models
{
    public class MAIL
    {
        public int ID { get; set; }
        public string TO { get; set; }
        public string FROM { get; set; }
        public string SUBJECT { get; set; }
        public string MAILADRESS { get; set; }
        public string ISSEND { get; set; }
    }
}
