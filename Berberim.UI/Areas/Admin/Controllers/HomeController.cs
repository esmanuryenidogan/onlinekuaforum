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
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                return View("Index");

            }

            else
            {
                return View("AdminGiris");
            }
            
        }

        public ActionResult AdminEkle()
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                return View("AdminEkle");

            }

            else
            {
                return View("AdminGiris");
            }
        }

        [HttpPost]
        public ActionResult AdminEkle(string adminad,string kulad,string sifre)
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                AdminGiris adminekle = new AdminGiris();
                adminekle.AdminAd = adminad;
                adminekle.AdminKulAdi = kulad;
                adminekle.AdminSifre = sifre;
                adminekle.IsActive = true;
                adminekle.YetkiID = 1;

                db.AdminGiris.Add(adminekle);
                db.SaveChanges();

                return View("Index");

            }

            else
            {
                return View("AdminGiris");
            }

            
           
        }

        public ActionResult AdminGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminGiris(string kuladi,string sifre)
        {
            var sonuc = (from i in db.AdminGiris where i.IsActive == true && i.YetkiID == 1 && i.AdminKulAdi == kuladi && i.AdminSifre == sifre select i).SingleOrDefault();


            Session["loginadmin"] = sonuc;
            Session["adminkuladi"] = kuladi;

            if (sonuc != null)
            {
                return View("Index");
            }
            else
            {
                ViewBag.girismesaj = "Kullanıcı Adı veya Şifre Hatalı !";
                return View();
            }

            
        }




        public ActionResult BerberKayıt()
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                return View("BerberKayıt");

            }

            else
            {
                return View("AdminGiris");
            }
        }

        
        [HttpPost]
        public ActionResult BerberKayıt(string salonad, string kuladi, string sifre)
        
        {


            var berberkuladi = (from i in db.AdminIslem where i.BerberKulAdi == kuladi select i.BerberKulAdi).SingleOrDefault();

            var berbersifre = (from i in db.AdminIslem where i.BerberSifre == sifre select i.BerberSifre).SingleOrDefault();

            

            if (berberkuladi == kuladi || berbersifre == sifre)
            {

                ViewBag.uyarı = "Böyle Bir Kullanıcı Adı veya Şifre Ekledin Admin !";
                return View();

            }

            else
            {
                AdminIslem bekle = new AdminIslem();
                BerberSayfa berber = new BerberSayfa();
                bekle.YetkiID = 2;
                bekle.IsActive = true;
                bekle.SalonAd = salonad;
                bekle.BerberKulAdi = kuladi;
                bekle.BerberSifre = sifre;
                db.AdminIslem.Add(bekle);
                db.SaveChanges();

                return View();
                
            }
            
      }
        

        public ActionResult CikisYap()
        {


            Session["loginadmin"] = null;
            return View("AdminGiris");
        }

        public ActionResult TrendSacGor()
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                var trendsac = (from i in db.TrendSaclar select i).ToList();

                return View(trendsac);

            }

            else
            {
                return View("AdminGiris");
            }
            
        }

        public ActionResult TrendSacSil(int id)
        {
            TrendSaclar trendsacsil = new TrendSaclar();
            db.TrendSaclar.Remove(db.TrendSaclar.Find(id));
            db.SaveChanges();
            return View("TrendSacGor", db.TrendSaclar);
        }

        public ActionResult TrendSac()
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                return View("Index");

            }

            else
            {
                return View("AdminGiris");
            }
        }

        [HttpPost]
        public ActionResult TrendSac(string foto)
        {
            
            TrendSaclar sacekle = new TrendSaclar();

            if (Request.Files.Count > 0)
            {
                
               
                string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string TamYolYeri = "~/Resimler/TrendSac/" + DosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));
                sacekle.SacModelFoto = DosyaAdi + uzanti;
                sacekle.IsActive = true;


                //MemoryStream ms = new MemoryStream();

                //FileStream fs = new FileStream(Server.MapPath(TamYolYeri),FileMode.Open);
                //byte[] resim = new byte[fs.Length];
                //fs.Write(resim, 0, resim.Length);
                //sacekle.SacFoto = resim;


            }

            db.TrendSaclar.Add(sacekle);
            db.SaveChanges();

            return View("Index");
        }

        

       SalonClass ekle = new SalonClass();
       
        public ActionResult BerberKayitGor()
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                var sonuc = (from i in db.BerberSayfa select i).ToList();

                return View(sonuc);

            }

            else
            {
                return View("AdminGiris");
            }
            
        }

        public ActionResult Guncelle(int id)
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                return View("Guncelle", db.BerberSayfa.Find(id));

            }

            else
            {
                return View("AdminGiris");
            }
            
        }

        [HttpPost]
        public ActionResult Guncelle(BerberSayfa u)
        {

            var mevcut = db.BerberSayfa.Find(u.id);
            mevcut.IsActive = u.IsActive;
            db.SaveChanges();
            return View("BerberKayitGor",db.BerberSayfa);
        }


        MusteriClass mekle = new MusteriClass();
        public ActionResult MusteriKayitGor()
        {
            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                var sonuc = (from i in db.MüsteriKayit select i).ToList();

                return View(sonuc);

            }

            else
            {
                return View("AdminGiris");
            }
            
        }

        public ActionResult MusteriGuncelle(int id)
        {

            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                return View("MusteriGuncelle", db.MüsteriKayit.Find(id));

            }

            else
            {
                return View("AdminGiris");
            }
            
        }

        [HttpPost]
        public ActionResult MusteriGuncelle(MüsteriKayit u)
        {

            var mevcut = db.MüsteriKayit.Find(u.id);
            mevcut.IsActive = u.IsActive;
            db.SaveChanges();
            return View("MusteriKayitGor",db.MüsteriKayit);
        }

        public ActionResult OtomatikMail()
        {

            var gelen = (AdminGiris)Session["loginadmin"];


            if (gelen != null)
            {
                return View("OtomatikMail");

            }

            else
            {
                return View("AdminGiris");
            }
        }

        [HttpPost]
        public ActionResult OtomatikMail(string mesaj,string konu)
        {
            try
            {
                var musteriler = (from i in db.MüsteriKayit where i.IsActive == true select i).SingleOrDefault();
                MailMessage ePosta = new MailMessage();
                ePosta.From = new MailAddress("berberim@gmail.com");
                ePosta.To.Add(musteriler.MusteriMail);
                ePosta.Subject = konu;
                ePosta.Body = mesaj;
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential("berberim@gmail.com", "sifre");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                object userState = ePosta;
                OtomatikMail omail = new OtomatikMail();
                omail.MusteriAd = musteriler.MusteriAd;
                omail.MusteriId = musteriler.id;
                omail.MusteriMail = musteriler.MusteriMail;
                omail.Mesaj = mesaj;
                return View();

            }
            catch (Exception)
            {
                return View();
                
            }
            
           
        }

        

       
        
    }
}