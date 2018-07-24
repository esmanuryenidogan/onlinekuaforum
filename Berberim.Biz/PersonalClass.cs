using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berberim.Data.Models;

namespace Berberim.Biz
{
    public class PersonalClass
    {
        readonly OnlineKuaforumDbContext _db;
        public PersonalClass()
        {
            _db = new OnlineKuaforumDbContext();
        }
        public List<PERSONEL> PersonelKayıt()
        {
            return _db.PERSONEL.Where(m => m.STATUS == Constants.RecordStatu.Active).OrderBy(m => m.ADSOYAD).ToList();
        }
    }
}
