using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Berberim.Data.Models;

namespace Berberim.UI.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        OnlineKuaforumDbContext _db = new OnlineKuaforumDbContext();

        public PartialViewResult MusteriPartial()
        {
            return PartialView();
        }

        public ActionResult MusteriLayout()
        {
            return View();
        }
    }
}