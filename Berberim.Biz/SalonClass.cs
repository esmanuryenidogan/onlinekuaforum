using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berberim.Data.Models;

namespace Berberim.Biz
{
    public class SalonClass
    {
        OnlineKuaforumDbContext _db = new OnlineKuaforumDbContext();

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
