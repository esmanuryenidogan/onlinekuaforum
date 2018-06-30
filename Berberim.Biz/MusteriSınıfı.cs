using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public class MusteriSınıfı
    {
        BerberimEntities db;
        public MusteriSınıfı()
        {
            db = new BerberimEntities();

        }

        public List<MüsteriKayit> MusteriKayıtlar()
        {
            return (from i in db.MüsteriKayit select i).ToList();
        }

    }
}
