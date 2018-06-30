using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public class Personeller
    {
        BerberimEntities db;
        public Personeller() 
        { 
         db = new BerberimEntities();

        }

        public List<Personel> PersonelKayıt()
        {
            return (from m in db.Personel where (m.IsActive == true) orderby m.PersonelAdSoyad select m).ToList();
                    
        }
    }
}
