using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berberim.Biz
{
    public class BerberBilgiGetir
    {

        BerberimEntities db = new BerberimEntities();

        public List<Personel> PGetir ()
        {
            

            return (from i in db.Personel select i).ToList();
        
        }

        public List<Islemler> İGetir()
        {
            

            return (from i in db.Islemler select i).ToList();

        }
    

    }
}
