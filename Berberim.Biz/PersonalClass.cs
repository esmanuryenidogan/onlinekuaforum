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
        BerberimEntities db;
        public PersonalClass()
        {
            db = new BerberimEntities();
        }
        public List<Personel> PersonelKayıt()
        {
            return db.Personel.Where(m => m.IsActive == true).OrderBy(m => m.PersonelAdSoyad).ToList();
        }
    }
}
