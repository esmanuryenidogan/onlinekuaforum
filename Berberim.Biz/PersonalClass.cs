using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public class PersonalClass
    {
        readonly BerberimEntities _db;
        public PersonalClass()
        {
            _db = new BerberimEntities();
        }
        public List<PERSONEL> PersonelKayıt()
        {
            return _db.PERSONEL.Where(m => m.STATUS == Constants.RecordStatu.Active).OrderBy(m => m.ADSOYAD).ToList();
        }
    }
}
