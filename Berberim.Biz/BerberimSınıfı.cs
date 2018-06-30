using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public class BerberimSınıfı
    {
        BerberimEntities db;
        public BerberimSınıfı() 
        { 
         db = new BerberimEntities();

        }

        public List<BerberSayfa> BerberKayitlar()
        {
            return (from i in db.BerberSayfa select i).ToList();
        }

    }
}
