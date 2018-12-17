using Berberim.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Berberim.Biz;
using Berberim.Data.Models;

namespace Berberim.UI.Areas.Berber.Controllers
{
    public class HomeController : Controller
    {
        OnlineKuaforumDbContext db = new OnlineKuaforumDbContext();
        // GET: Berber/Home
        public ActionResult Index()
        {
            //Method yapılacak.
            var gelenSalon = (SALON)Session["berberkuladi"];
            var salon = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salon;

            var gelen = Session["berberkuladi"];
            return View(gelen != null ? "Index" : "BerberGiris");
        }

        public ActionResult BerberGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BerberGiris(string email, string sifre)
        {
            var sonuc = (from i in db.SALON where i.EMAIL == email && i.SIFRE == sifre select i).FirstOrDefault();


            if (sonuc != null)
            {

                if (sonuc.STATUS == Constants.RecordStatu.Active)
                {
                    Session["berberkuladi"] = sonuc;
                    var gelenSalon = (SALON)Session["berberkuladi"];
                    var salon = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
                    ViewBag.salonControl = salon;
                    return View("Index");
                }
                ViewBag.mesaj = "Kullanıcınız aktif durumda olmadığından giriş yapamamaktasınız, kullanıcınızı aktif hale getirmek için bizimle iletişime geçebilirsiniz.";
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Adı veya Şifre Hatalı !";
            }
            return View();
        }

        public ActionResult BerberCikisYap()
        {
            Session.Abandon();
            return View("BerberGiris");
        }

        public ActionResult BerberSalonEkle()
        {
            var gelen = (SALON)Session["berberkuladi"];
            ViewBag.email = gelen.EMAIL;
            return View("BerberSalonEkle");
        }

        [HttpPost]
        public ActionResult BerberSalonEkle(string salonad, string koltuk, string salonmail, string salonadres, string salontel, string vitrinyazi, string foto, string personeladsoyad, string personelfoto, string salonhakkinda, string il, string ilce)
        {
            var gelen = (SALON)Session["berberkuladi"];
            var salon = (from i in db.SALONSAYFA where i.SALONID == gelen.ID select i).FirstOrDefault();

            //if (salon != null)
            //{
            //    ViewBag.mesaj = "Size Tanımlı Bir Salon Mevcut !";
            //    return View();
            //}

            if (salon == null)
            {
                var berberekle = new SALONSAYFA
                {
                    AD = salonad,
                    KOLTUKSAY = Convert.ToInt32(koltuk),
                    ADRES = salonadres,
                    TEL = salontel,
                    HAKKINDA = salonhakkinda,
                    EMAIL = gelen.EMAIL,
                    VITRINYAZI = vitrinyazi,
                    SALONID = gelen.ID,
                    IL = il,
                    ILCE = ilce,
                    STATUS = Constants.RecordStatu.Active
                };

                string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                var httpPostedFileBase = Request.Files[0];
                if (httpPostedFileBase != null)
                {
                    string uzanti = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                    string tamYolYeri = "~/Resimler/SalonVitrinFoto/" + dosyaAdi + uzanti;
                    httpPostedFileBase.SaveAs(Server.MapPath(tamYolYeri));
                    berberekle.VITRINFOTO = dosyaAdi + uzanti;
                }
                db.SALONSAYFA.Add(berberekle);
                db.SaveChanges();

                var berberSalon = BerberDuzenle();
                ViewData["Model"] = berberSalon;
                var model = ViewData.Model;

                return View("BerberDuzenle", model);
            }
            return View();
        }

        public ActionResult BerberDuzenle()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen == null)
            {
                return View("BerberGiris");
            }
            var gelenSalon = (SALON)Session["berberkuladi"];
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;

            if (salonControl != null)
            {
                var salon = (from i in db.SALONSAYFA where i.EMAIL == gelen.EMAIL select i).ToList();
                return View(salon);
            }
            else
            {
                return View("Index");
            }

        }
        SALONSAYFA ssil = new SALONSAYFA();
        public ActionResult BerberSil(int ID)
        {
            db.SALONSAYFA.Remove(db.SALONSAYFA.Find(ID));
            db.SaveChanges();
            return View("Index", db.SALONSAYFA);
        }

        public ActionResult Guncelle(int ID)
        {
            var gelen = (SALON)Session["berberkuladi"];
            var gelenSalon = (SALON)Session["berberkuladi"];
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;

            if (gelen != null)
            {
                return View("Guncelle", db.SALONSAYFA.Find(ID));
            }
            return View("BerberGiris");
        }

        [HttpPost]
        public ActionResult Guncelle(SALONSAYFA u)
        {
            var mevcut = db.SALONSAYFA.Find(u.ID);
            string salonadeski = mevcut?.AD;
            //string fotos = mevcut?.VITRINFOTO;
            //if (Request.Files.Count != 0)
            //{
            //    string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
            //    var httpPostedFileBase = Request.Files[0];
            //    if (httpPostedFileBase != null)
            //    {
            //        string uzanti = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
            //        string tamYolYeri = "~/Resimler/SalonVitrinFoto/" + dosyaAdi + uzanti;
            //        httpPostedFileBase.SaveAs(Server.MapPath(tamYolYeri));
            //        if (mevcut != null) mevcut.VITRINFOTO = dosyaAdi + uzanti;
            //    }
            //}
            //else
            //{
            //    if (mevcut != null) mevcut.VITRINFOTO = fotos;
            //}
            if (mevcut != null)
            {
                mevcut.AD = u.AD;
                mevcut.IL = u.IL;
                mevcut.ILCE = u.ILCE;
                mevcut.TEL = u.TEL;
                mevcut.EMAIL = u.EMAIL;
                mevcut.KOLTUKSAY = u.KOLTUKSAY;
                mevcut.HAKKINDA = u.HAKKINDA;
                mevcut.VITRINYAZI = u.VITRINYAZI;
            }
            var adminislem = db.ADMIN.FirstOrDefault(s => s.AD == salonadeski);
            if (adminislem != null) adminislem.AD = u.AD;

            db.SaveChanges();
            return View("Index", db.SALONSAYFA);
        }

        public ActionResult SalonFotograflarGor()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen == null)
            {
                return View("BerberGiris");
            }

            var gelenSalon = (SALON)Session["berberkuladi"];
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;

            if (salonControl != null)
            {
                var salon = (from i in db.SALONSAYFA where i.SALONID == gelen.ID select i).FirstOrDefault();

                if (salon != null)
                {
                    var salonfoto = (from i in db.SALONFOTO where i.SALONID == salon.ID select i).ToList();
                    return View(salonfoto);
                }

                return View("SalonFotograflarGor");
            }
            else
            {
                return View("Index");
            }

        }

        public ActionResult SalonFotograflarSil(int ID)
        {
            var salonfotosil = new SALONFOTO();
            db.SALONFOTO.Remove(db.SALONFOTO.Find(ID));
            db.SaveChanges();
            return View("Index", db.SALONFOTO);
        }

        public ActionResult SalonFotografEkle()
        {
            var gelen = (SALON)Session["berberkuladi"];

            if (gelen != null)
            {
                return View();
            }
            return View("BerberGiris");
        }

        [HttpPost]
        public ActionResult SalonFotografEkle(string foto)
        {
            SALONFOTO salonfotoekle = new SALONFOTO();

            var gelen = (SALON)Session["berberkuladi"];
            if (gelen != null)
            {
                var salonId = (from i in db.SALONSAYFA where i.SALONID == gelen.ID select i).FirstOrDefault();

                if (Request.Files.Count > 0)
                {
                    string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    var httpPostedFileBase = Request.Files[0];
                    if (httpPostedFileBase != null)
                    {
                        string uzanti = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                        string tamYolYeri = "~/Resimler/SalonFotograflar/" + dosyaAdi + uzanti;
                        httpPostedFileBase.SaveAs(Server.MapPath(tamYolYeri));
                        salonfotoekle.FOTO = dosyaAdi + uzanti;
                    }
                    salonfotoekle.SALONID = salonId.ID;
                    salonfotoekle.STATUS = Constants.RecordStatu.Active;
                }
            }
            db.SALONFOTO.Add(salonfotoekle);
            db.SaveChanges();

            var salonFotolar = SalonFotograflarGor();

            ViewData["Model"] = salonFotolar;
            var model = ViewData.Model;

            return View("SalonFotograflarGor", model);
        }

        public ActionResult KesilenSaclarGor()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen == null)
            {
                return View("BerberGiris");
            }
            var gelenSalon = (SALON)Session["berberkuladi"];
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;


            if (salonControl != null)
            {
                var salon = (from i in db.SALONSAYFA where i.SALONID == gelen.ID select i.ID).FirstOrDefault();
                var kesilenmodeller = (from i in db.BSACMODEL where i.SALONID == salon select i).ToList();

                return View(kesilenmodeller);
            }
            else
            {
                return View("Index");
            }

        }

        public ActionResult KesilenSacSil(int ID)
        {
            BSACMODEL sacfotosil = new BSACMODEL();
            db.BSACMODEL.Remove(db.BSACMODEL.Find(ID));
            db.SaveChanges();
            return View("Index", db.BSACMODEL);
        }

        public ActionResult KesilenSacFotoEkle()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen != null)
            {
                return View();
            }

            return View("BerberGiris");
        }

        [HttpPost]
        public ActionResult KesilenSacFotoEkle(string foto)
        {
            var kesilensacfotoekle = new BSACMODEL();
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen != null)
            {
                var salonId = (from i in db.SALONSAYFA where i.SALONID == gelen.ID select i.ID).SingleOrDefault();

                if (Request.Files.Count > 0)
                {
                    string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    var httpPostedFileBase = Request.Files[0];
                    if (httpPostedFileBase != null)
                    {
                        string uzanti = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                        string tamYolYeri = "~/Resimler/KesilenSaclar/" + dosyaAdi + uzanti;
                        httpPostedFileBase.SaveAs(Server.MapPath(tamYolYeri));
                        kesilensacfotoekle.FOTO = dosyaAdi + uzanti;
                    }
                    kesilensacfotoekle.SALONID = salonId;
                    kesilensacfotoekle.STATUS = Constants.RecordStatu.Active;
                }
            }
            db.BSACMODEL.Add(kesilensacfotoekle);
            db.SaveChanges();

            var kesilenSaclar = KesilenSaclarGor();
            ViewData["Model"] = kesilenSaclar;
            var model = ViewData.Model;

            return View("KesilenSaclarGor", model);
        }

        public ActionResult PersonelDuzenle()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen == null)
            {
                return View("BerberGiris");
            }

            var gelenSalon = (SALON)Session["berberkuladi"];
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;


            if (salonControl != null)
            {
                var personel = (from i in db.PERSONEL where i.SALONID == gelen.ID select i).ToList();

                return View(personel);
            }
            else
            {
                return View("Index");
            }

        }

        public ActionResult PersonelEkle()
        {
            var gelen = (SALON)Session["berberkuladi"];

            if (gelen != null)
            {
                return View();
            }
            return View("BerberGiris");
        }
        [HttpPost]
        public ActionResult PersonelEkle(string personeladsoyad, string unvan, string cinsiyet, string foto)
        {
            var gelen = (SALON)Session["berberkuladi"];

            var pekle = new PERSONEL();
            if (gelen != null)
            {
                pekle.ADSOYAD = personeladsoyad;
                pekle.STATUS = Constants.RecordStatu.Active;
                pekle.SALONID = gelen.ID;
                if (foto != null && Request.Files.Count > 0)
                {
                    string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    var httpPostedFileBase = Request.Files[0];
                    if (httpPostedFileBase != null)
                    {
                        string uzanti = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                        string tamYolYeri = "~/Common/Personeller/" + dosyaAdi + uzanti;
                        httpPostedFileBase.SaveAs(Server.MapPath(tamYolYeri));
                        pekle.FOTO = dosyaAdi + uzanti;
                    }

                }
                pekle.UNVAN = unvan;
                pekle.CINSIYET = cinsiyet;
                db.PERSONEL.Add(pekle);
                db.SaveChanges();

                var personelDuzenle = PersonelDuzenle();

                ViewData["Model"] = personelDuzenle;
                var model = ViewData.Model;

                return View("PersonelDuzenle", model);
            }
            return View();
        }

        public ActionResult PersonelSil(int ID)
        {
            var bsil = new PERSONEL();
            db.PERSONEL.Remove(db.PERSONEL.Find(ID));
            db.SaveChanges();
            return View("Index", db.PERSONEL);
        }

        public ActionResult Berberİslem()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen == null)
            {
                return View("BerberGiris");
            }
            var gelenSalon = (SALON)Session["berberkuladi"];
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;


            if (salonControl != null)
            {
                var islemler = (from i in db.ISLEM where i.SALONID == gelen.ID select i).ToList();

                return View(islemler);
            }
            else
            {
                return View("Index");
            }

        }

        public ActionResult BerberİslemEkle()
        {
            var gelen = (SALON)Session["berberkuladi"];

            if (gelen != null)
            {
                return View();
            }

            return View("BerberGiris");
        }

        [HttpPost]
        public ActionResult BerberİslemEkle(string islemad, int islemfiyat)
        {
            var gelen = (SALON)Session["berberkuladi"];

            var islemekle = new ISLEM();
            if (gelen != null)
            {
                islemekle.STATUS = Constants.RecordStatu.Active;
                islemekle.AD = islemad;
                islemekle.FIYAT = islemfiyat;
                islemekle.SALONID = gelen.ID;
                db.ISLEM.Add(islemekle);
                db.SaveChanges();

                var islemler = Berberİslem();

                ViewData["Model"] = islemler;
                var model = ViewData.Model;

                return View("Berberİslem", model);
            }
            return View();
        }

        public ActionResult İslemSil(int ID)
        {
            var bsil = new ISLEM();
            db.ISLEM.Remove(db.ISLEM.Find(ID));
            db.SaveChanges();
            return View("Index", db.ISLEM);
        }

        public ActionResult Kampanyalar()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen == null)
            {
                return View("BerberGiris");
            }
            var gelenSalon = (SALON)Session["berberkuladi"];
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelenSalon.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;

            if (salonControl != null)
            {
                var kampanyalar = (from i in db.KAMPANYA where i.SALONID == gelen.ID select i).ToList();
                return View(kampanyalar);
            }
            else
            {

                return View("Index");
            }

        }

        public ActionResult KampanyaSil(int ID)
        {
            var bsil = new KAMPANYA();
            db.KAMPANYA.Remove(db.KAMPANYA.Find(ID));
            db.SaveChanges();
            return View("Index", db.KAMPANYA);
        }

        public ActionResult KampanyaEkle()
        {
            var gelen = (SALON)Session["berberkuladi"];

            if (gelen != null)
            {
                return View();
            }
            return View("BerberGiris");
        }

        [HttpPost]
        public ActionResult KampanyaEkle(string baslik, string icerik, int fiyat, DateTime tarih)
        {
            var gelen = (SALON)Session["berberkuladi"];

            KAMPANYA kekle = new KAMPANYA();
            if (gelen != null)
            {
                kekle.STATUS = Constants.RecordStatu.Active;
                kekle.BASLIK = baslik;
                kekle.ICERIK = icerik;
                kekle.FIYAT = fiyat;
                kekle.SALONID = gelen.ID;
                kekle.SALONAD = gelen.ADSOYAD;
                kekle.SONGUN = tarih;
                db.KAMPANYA.Add(kekle);
                db.SaveChanges();

                var kampanyalar = Kampanyalar();
                ViewData["Model"] = kampanyalar;
                var model = ViewData.Model;

                return View("Kampanyalar", model);
            }
            ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
            return View();
        }

        public ActionResult BerberRandevular()
        {
            var gelen = (SALON)Session["berberkuladi"];
            if (gelen == null)
            {
                return View("BerberGiris");
            }
            var salonControl = (from i in db.SALONSAYFA where i.SALONID == gelen.ID select i).FirstOrDefault();
            ViewBag.salonControl = salonControl;


            if (salonControl != null)
            {
                var randevukontrol = (from i in db.RANDEVU where i.SALONID == gelen.ID select i).ToList();
                return View(randevukontrol);
            }
            return View("Index");
        }

        public ActionResult BerberKayıt()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BerberKayıt(string email, string sifre)
        {
            var sonuc = (from i in db.SALON where i.EMAIL == email && i.SIFRE == sifre select i).FirstOrDefault();

            SALON berberkayit = new SALON();

            if (sonuc == null)
            {

                berberkayit.EMAIL = email;
                berberkayit.SIFRE = sifre;
                berberkayit.CREATEDATE = DateTime.UtcNow;
                berberkayit.STATUS = 2;

                db.SALON.Add(berberkayit);
                db.SaveChanges();

                return View("BerberGiris");
            }
            else
                return View("BerberKayıt");

        }
    }
}