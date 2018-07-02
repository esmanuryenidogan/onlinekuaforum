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
        public List<BerberSayfa> berber { get; set; }
        public List<Kampanyalar> kampanyalar { get; set; }
        public List<TrendSaclar> trendSac { get; set; }
        //public List<MusteriYorumlari> musteriYorumlar { get; set; }
    }
}
