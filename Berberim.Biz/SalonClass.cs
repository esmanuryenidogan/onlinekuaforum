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
        private BerberimEntities _db = new BerberimEntities();
        public List<Personel> GetPersonels()
        {
            return _db.Personel.ToList();
        }
        public List<Islemler> GetIslems()
        {
            return _db.Islemler.ToList();
        }
        public List<BerberSayfa> GetSalons()
        {
            return _db.BerberSayfa.ToList();
        }
    }
}
