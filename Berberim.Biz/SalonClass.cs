using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public class SalonClass
    {
        private readonly BerberimEntities _db = new BerberimEntities();
        public List<PERSONEL> GetPersonels()
        {
            return _db.PERSONEL.ToList();
        }
        public List<ISLEM> GetIslems()
        {
            return _db.ISLEM.ToList();
        }
        public List<SALONSAYFA> GetSalons()
        {
            return _db.SALONSAYFA.ToList();
        }
    }
}
