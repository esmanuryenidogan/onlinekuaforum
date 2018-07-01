using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public class MusteriClass
    {
        BerberimEntities db;
        public MusteriClass()
        {
            db = new BerberimEntities();
        }
        public List<MüsteriKayit> MusteriKayıtlar()
        {
            return db.MüsteriKayit.ToList();
        }

    }
}
