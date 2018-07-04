using Berberim.Biz;
using Berberim.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace Berberim.UI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        BerberimEntities db = new BerberimEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return View(gelen != null ? "Index" : "AdminGiris");
        }

        public ActionResult AdminEkle()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return View(gelen != null ? "AdminEkle" : "AdminGiris");
        }

        [HttpPost]
        public ActionResult AdminEkle(string adminad,string email,string sifre)
        {
            var gelen = (ADMIN)Session["loginadmin"];
            if (gelen != null)
            {
                var adminekle = new ADMIN
                {
                    AD = adminad,
                    EMAIL = email,
                    SIFRE = sifre,
                    STATUS = Constants.RecordStatu.Active
                };
                db.ADMIN.Add(adminekle);
                db.SaveChanges();

                return View("Index");
            }
                return View("AdminGiris");                   
        }

        public ActionResult AdminGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminGiris(string email,string sifre)
        {
            var sonuc = (from i in db.ADMIN where i.STATUS == Constants.RecordStatu.Active && i.EMAIL == email && i.SIFRE == sifre select i).FirstOrDefault();

            if (sonuc != null)
            {
                Session["loginadmin"] = sonuc;
                Session["adminkuladi"] = sonuc.EMAIL;

                return View("Index");
            }
                ViewBag.girismesaj = "Kullanıcı Adı veya Şifre Hatalı !";
                return View();          
        }

        #region OLD

                //  public ActionResult BerberKayıt()
        //  {
        //      var gelen = (ADMIN)Session["loginadmin"];

        //      return View(gelen != null ? "BerberKayıt" : "AdminGiris");
        //  }

        //  [HttpPost]
        //  public ActionResult BerberKayıt(string salonad, string email, string sifre)
        //  {
        //      var salon = (from i in db.SALON where i.EMAIL == email && i.SIFRE == sifre select i).FirstOrDefault();

        //      if (salon != null)
        //      {
        //          ViewBag.uyarı = "Böyle Bir Kullanıcı Adı veya Şifre Ekledin !";
        //          return View();
        //      }

        //      else
        //      {
        //          SALONSAYFA berber = new SALONSAYFA();
        //          berber.STATUS = Constants.RecordStatu.Active;
        //          db.SALONSAYFA.Add(berber);
        //          db.SaveChanges();
        //          return View();
        //      }
        //}

        #endregion

        public ActionResult CikisYap()
        {
            Session["loginadmin"] = null;
            return View("AdminGiris");
        }

        public ActionResult TrendSacGor()
        {
            var gelen = (ADMIN)Session["loginadmin"];

            if (gelen != null)
            {
                var trendsac = (from i in db.TRENDHAIRS select i).ToList();
                return View(trendsac);
            }
                return View("AdminGiris");            
        }

        public ActionResult TrendSacSil(int id)
        {
            var trendsacsil = new TRENDHAIRS();
            db.TRENDHAIRS.Remove(db.TRENDHAIRS.Find(id));
            db.SaveChanges();
            return View("TrendSacGor", db.TRENDHAIRS);
        }

        public ActionResult TrendSac()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return View(gelen != null ? "Index" : "AdminGiris");
        }

        [HttpPost]
        public ActionResult TrendSac(string foto)
        {
            var sacekle = new TRENDHAIRS();
            if (Request.Files.Count > 0)
            {
                string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string TamYolYeri = "~/Resimler/TrendSac/" + DosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));
                sacekle.FOTO = DosyaAdi + uzanti;
                sacekle.STATUS = Constants.RecordStatu.Active;

                //MemoryStream ms = new MemoryStream();
                //FileStream fs = new FileStream(Server.MapPath(TamYolYeri),FileMode.Open);
                //byte[] resim = new byte[fs.Length];
                //fs.Write(resim, 0, resim.Length);
                //sacekle.SacFoto = resim;
            }

            db.TRENDHAIRS.Add(sacekle);
            db.SaveChanges();

            return View("Index");
        }

       SalonClass ekle = new SalonClass();
       
        public ActionResult BerberKayitGor()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            if (gelen == null) return View("AdminGiris");
            var sonuc = (from i in db.SALONSAYFA select i).ToList();

            return View(sonuc);
        }

        public ActionResult Guncelle(int id)
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return gelen != null ? View("Guncelle", db.SALONSAYFA.Find(id)) : View("AdminGiris");
        }

        [HttpPost]
        public ActionResult Guncelle(SALONSAYFA u)
        {
            var mevcut = db.SALONSAYFA.Find(u.ID);
            if (mevcut != null) mevcut.STATUS = u.STATUS;
            db.SaveChanges();
            return View("BerberKayitGor",db.SALONSAYFA);
        }

        MusteriClass mekle = new MusteriClass();
        public ActionResult MusteriKayitGor()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            if (gelen == null) return View("AdminGiris");
            var sonuc = (from i in db.MUSTERI select i).ToList();
            return View(sonuc);
        }

        public ActionResult MusteriGuncelle(int id)
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return gelen != null ? View("MusteriGuncelle", db.MUSTERI.Find(id)) : View("AdminGiris");
        }

        [HttpPost]
        public ActionResult MusteriGuncelle(MUSTERI u)
        {
            var mevcut = db.MUSTERI.Find(u.ID);
            if (mevcut != null) mevcut.STATUS = u.STATUS;
            db.SaveChanges();
            return View("MusteriKayitGor",db.MUSTERI);
        }

        public ActionResult OtomatikMail()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return View(gelen != null ? "OtomatikMail" : "AdminGiris");
        }

        [HttpPost]
        public ActionResult OtomatikMail(string mesaj,string konu)
        {
            try
            {
                var musteriler = (from i in db.MUSTERI where i.STATUS == Constants.RecordStatu.Active select i).FirstOrDefault();
                MailMessage ePosta = new MailMessage();
                ePosta.From = new MailAddress("berberim@gmail.com");
                ePosta.To.Add(musteriler.EMAIL);
                ePosta.Subject = konu;
                ePosta.Body = mesaj;
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential("berberim@gmail.com", "sifre");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                object userState = ePosta;
                var omail = new MAIL
                {
                    TO = musteriler.EMAIL,
                    FROM = Constants.ContactMail,
                    MAIL1 = mesaj,
                    SUBJECT = konu
                };
                return View();

            }
            catch (Exception)
            {
                return View();
            }
        }        
    }
}