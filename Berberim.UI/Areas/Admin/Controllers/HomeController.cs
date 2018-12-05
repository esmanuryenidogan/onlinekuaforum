using Berberim.Biz;
using Berberim.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Berberim.Data.Models;

namespace Berberim.UI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly OnlineKuaforumDbContext _db = new OnlineKuaforumDbContext();
        private SalonClass _ekle = new SalonClass();

        // GET: Admin/Home
        public ActionResult AdminGiris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminGiris(string email, string sifre)
        {
            var sonuc = (from i in _db.ADMIN where i.STATUS == Constants.RecordStatu.Active && i.EMAIL.Equals(email) && i.SİFRE.Equals(sifre)select i).FirstOrDefault();
            if (sonuc != null)
            {
                Session.Add("loginadmin",sonuc) ;
                return View("Index");
            }
            ViewBag.girismesaj = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        public ActionResult Index()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            if (gelen != null) return View(gelen);
            return View("AdminGiris");
        }

        public ActionResult BerberKayitGor()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            if (gelen == null) return View("AdminGiris");
            var sonuc = _db.SALONSAYFA.ToList();
            return View(sonuc);
        }

        public ActionResult Guncelle(int id)
        {
            var mevcut = _db.SALONSAYFA.Find(id);
            var newStatu = 2;
            switch (mevcut?.STATUS)
            {
                case 1:
                    newStatu = 2;
                    break;
                case 2:
                    newStatu = 1;
                    break;
                case 3:
                    newStatu = 2;
                    break;
            }
            
            if (mevcut != null) mevcut.STATUS = newStatu;
            _db.SaveChanges();
            return RedirectToAction("BerberKayitGor");
        }

        public ActionResult AdminEkle()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return View(gelen != null ? "AdminEkle" : "AdminGiris");
        }

        [HttpPost]
        public ActionResult AdminEkle(string adminad, string email, string sifre)
        {
            var gelen = (ADMIN)Session["loginadmin"];
            if (gelen != null)
            {
                var adminekle = new ADMIN
                {
                    AD = adminad,
                    EMAIL = email,
                    SİFRE = sifre,
                    STATUS = Constants.RecordStatu.Active
                };
                _db.ADMIN.Add(adminekle);
                _db.SaveChanges();

                return View("Index");
            }
            return View("AdminGiris");
        }

        public ActionResult BerberKayıt()
        {
            var salon = _db.SALON.Where(i => i.STATUS == Constants.RecordStatu.Passive).ToList();
            return View(salon);
        }

        [HttpPost]
        public ActionResult BerberKayıt(int id)
        {
            var updSalon = _db.SALON.FirstOrDefault(a => a.ID == id);
            if (updSalon != null)
            {
                updSalon.STATUS = Constants.RecordStatu.Active;
                updSalon.SIFRE = CreatePassword(6);
            }

            _db.SaveChanges();
            return RedirectToAction("BerberKayıt");
        }

        public string CreatePassword(int size)
        {
            char[] cr = "0123456789ABCDEFGHIJKLMNOPQRSTUCWXYZ".ToCharArray();
            string result = string.Empty;
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                result += cr[r.Next(0, cr.Length - 1)].ToString();
            }

            return result;
        }

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
                var trendsac = (from i in _db.TRENDHAIRS select i).ToList();
                return View(trendsac);
            }
            return View("AdminGiris");
        }

        public ActionResult TrendSacSil(int id)
        {
            var trendsacsil = new TRENDHAIR();
            _db.TRENDHAIRS.Remove(_db.TRENDHAIRS.Find(id));
            _db.SaveChanges();
            return View("TrendSacGor", _db.TRENDHAIRS);
        }

        public ActionResult TrendSac()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return View(gelen != null ? "Index" : "AdminGiris");
        }

        [HttpPost]
        public ActionResult TrendSac(string foto)
        {
            var sacekle = new TRENDHAIR();
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

            _db.TRENDHAIRS.Add(sacekle);
            _db.SaveChanges();

            return View("Index");
        }

        private MusteriClass mekle = new MusteriClass();
        public ActionResult MusteriKayitGor()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            if (gelen == null) return View("AdminGiris");
            var sonuc = (from i in _db.MUSTERİ select i).ToList();
            return View(sonuc);
        }

        public ActionResult MusteriGuncelle(int id)
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return gelen != null ? View("MusteriGuncelle", _db.MUSTERİ.Find(id)) : View("AdminGiris");
        }

        [HttpPost]
        public ActionResult MusteriGuncelle(MUSTERI u)
        {
            var mevcut = _db.MUSTERİ.Find(u.ID);
            if (mevcut != null) mevcut.STATUS = u.STATUS;
            _db.SaveChanges();
            return View("MusteriKayitGor", _db.MUSTERİ);
        }

        public ActionResult OtomatikMail()
        {
            var gelen = (ADMIN)Session["loginadmin"];
            return View(gelen != null ? "OtomatikMail" : "AdminGiris");
        }

        [HttpPost]
        public ActionResult OtomatikMail(string mesaj, string konu)
        {
            try
            {
                var musteriler = (from i in _db.MUSTERİ where i.STATUS == Constants.RecordStatu.Active select i).FirstOrDefault();
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
                    MAILADRESS = mesaj,
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