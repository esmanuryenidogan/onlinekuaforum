using Berberim.Biz;
using Berberim.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Berberim.UI.Controllers
{
    public class AboutController : Controller
    {
        OnlineKuaforumDbContext _db = new OnlineKuaforumDbContext();
        // GET: Contact
        public ActionResult Index()
        {
            var data = new tabMenu
            {
                salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteri = _db.MUSTERİ.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                randevu = _db.RANDEVU.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
            };
            return View(data);
        }
    }
}