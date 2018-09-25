using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berberim.Data.Models;

namespace Berberim.Biz
{
    public class MusteriClass
    {
        readonly OnlineKuaforumDbContext _db;
        public MusteriClass()
        {
            _db = new OnlineKuaforumDbContext();
        }
        public List<MUSTERI> MusteriKayıtlar()
        {
            return _db.MUSTERİ.ToList();
        }

    }
}
