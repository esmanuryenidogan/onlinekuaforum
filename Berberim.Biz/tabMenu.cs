using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berberim.Data;

namespace Berberim.Biz
{
    public class tabMenu
    {
        public List<SALONSAYFA> salon { get; set; }
        public List<KAMPANYA> kampanya { get; set; }
        public List<TRENDHAIRS> trendSac { get; set; }
        public List<YORUM> musteriYorum { get; set; }
    }
}
