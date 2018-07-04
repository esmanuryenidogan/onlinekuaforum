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
        readonly BerberimEntities _db;
        public MusteriClass()
        {
            _db = new BerberimEntities();
        }
        public List<MUSTERI> MusteriKayıtlar()
        {
            return _db.MUSTERI.ToList();
        }

    }
}
