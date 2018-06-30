using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Berberim.UI.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial

        BerberimEntities db = new BerberimEntities();
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