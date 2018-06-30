using Berberim.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Berberim.UI.Areas.Berber.Controllers
{
    public class HomeController : Controller
    {
        BerberimEntities db = new BerberimEntities();
        // GET: Berber/Home
        public ActionResult Index()
        {

            var gelen = (AdminIslem)Session["berberkuladi"];


            if (gelen != null)
            {
                return View("Index");

            }

            else
            {
                return View("BerberGiris");
            }


        }


        public ActionResult BerberGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BerberGiris(string kuladi, string sifre)
        {
            var sonuc = (from i in db.AdminIslem where i.IsActive == true && i.YetkiID == 2 && i.BerberKulAdi == kuladi && i.BerberSifre == sifre select i).SingleOrDefault();

            //string gelen = Request.Form["kuladi"];
            //Session.Add("kuladi", gelen);

            Session["berberkuladi"] = sonuc;
            Session["berberinkulad"] = kuladi;

            if (sonuc != null)
            {
                Session["salonAd"] = sonuc.SalonAd;
                return View("Index");
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Adı veya Şifre Hatalı !";
                return View();
            }

        }

        public ActionResult BerberCikisYap()
        {


            Session.Abandon();
            return View("BerberGiris");
        }


        public ActionResult BerberSalonEkle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];


            if (gelen != null)
            {
                return View();

            }

            else
            {
                return View("BerberGiris");
            }

        }

        [HttpPost]
        public ActionResult BerberSalonEkle(string salonad, string koltuk, string salonmail, string salonadres, string salontel, string vitrinyazi, string foto, string personeladsoyad, string personelfoto, string salonhakkinda, string ililce)
        {

            var gelen = (AdminIslem)Session["berberkuladi"];

            var adminid = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

            var berberisim = (from i in db.BerberSayfa where i.SalonID == adminid select i.SalonAd).SingleOrDefault();

            var berbersalon = (from i in db.BerberSayfa where i.SalonID == adminid select i).SingleOrDefault();

            if (berbersalon != null)
            {
                ViewBag.mesaj = "Size Tanımlı Bir Salon Mevcut !";
                return View();
            }


            if (berbersalon == null)
            {
                BerberSayfa berberekle = new BerberSayfa();


                berberekle.SalonAd = salonad;
                berberekle.KoltukSayisi = koltuk;
                berberekle.SalonAdres = salonadres;
                berberekle.SalonTel = salontel;
                berberekle.SalonHakkinda = salonhakkinda;
                berberekle.SalonMail = salonmail;
                berberekle.VitrinYazi = vitrinyazi;
                berberekle.SalonID = adminid;
                berberekle.iLveiLce = ililce;
                berberekle.YetkiID = 2;
                berberekle.IsActive = true;




                string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string TamYolYeri = "~/Resimler/SalonVitrinFoto/" + DosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));
                berberekle.VitrinFoto = DosyaAdi + uzanti;



                db.BerberSayfa.Add(berberekle);
                db.SaveChanges();

                return View();

            }

            else
            {

                return View();
            }
        }



        public ActionResult BerberDuzenle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {


                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

                var salon = (from i in db.BerberSayfa where i.SalonID == sonuc select i).ToList();

                return View(salon);
            }

            else
            {
                return View("BerberGiris");
            }


        }

        BerberSayfa bsil = new BerberSayfa();
        public ActionResult BerberSil(int id)
        {

            db.BerberSayfa.Remove(db.BerberSayfa.Find(id));
            db.SaveChanges();
            return View("Index", db.BerberSayfa);

        }

        public ActionResult Guncelle(int id)
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {
                return View("Guncelle", db.BerberSayfa.Find(id));
            }

            else
            {
                return View("BerberGiris");
            }

        }

        [HttpPost]
        public ActionResult Guncelle(BerberSayfa u)
        {

            var mevcut = db.BerberSayfa.Find(u.id);
            string salonadeski = mevcut.SalonAd;
            string fotos = mevcut.VitrinFoto;
            if (Request.Files.Count != 0)
            {
                string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string TamYolYeri = "~/Resimler/SalonVitrinFoto/" + DosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));
                mevcut.VitrinFoto = DosyaAdi + uzanti;
            }
            else
            {
                mevcut.VitrinFoto = fotos;
            }

            mevcut.SalonAd = u.SalonAd;
            mevcut.iLveiLce = u.iLveiLce;
            mevcut.SalonAdres = u.SalonAdres;
            mevcut.SalonTel = u.SalonTel;
            mevcut.SalonMail = u.SalonMail;
            mevcut.KoltukSayisi = u.KoltukSayisi;
            mevcut.SalonHakkinda = u.SalonHakkinda;
            mevcut.VitrinYazi = u.VitrinYazi;




            var adminislem = db.AdminIslem.Where(s => s.SalonAd == salonadeski).SingleOrDefault();
            adminislem.SalonAd = u.SalonAd;

            db.SaveChanges();
            return View("Index", db.BerberSayfa);
        }

        public ActionResult SalonFotograflarGor()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {
                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

                var salon = (from i in db.BerberSayfa where i.SalonID == sonuc select i.id).SingleOrDefault();

                var salonfoto = (from i in db.SalonFotolar where i.BerberId == salon select i).ToList();


                return View(salonfoto);


            }

            else
            {
                return View("BerberGiris");
            }

        }

        public ActionResult SalonFotograflarSil(int id)
        {
            SalonFotolar salonfotosil = new SalonFotolar();
            db.SalonFotolar.Remove(db.SalonFotolar.Find(id));
            db.SaveChanges();
            return View("Index", db.SalonFotolar);
        }

        public ActionResult SalonFotografEkle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {
                return View();
            }

            else
            {
                return View("BerberGiris");
            }

        }

        [HttpPost]
        public ActionResult SalonFotografEkle(string foto)
        {

            SalonFotolar salonfotoekle = new SalonFotolar();

            var gelen = (AdminIslem)Session["berberkuladi"];



            if (gelen != null)
            {
                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();
                var salonID = (from i in db.BerberSayfa where i.SalonID == sonuc select i.id).SingleOrDefault();
                var salonad = (from i in db.BerberSayfa where i.SalonID == sonuc select i.SalonAd).SingleOrDefault();


                if (Request.Files.Count > 0)
                {


                    string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string TamYolYeri = "~/Resimler/SalonFotograflar/" + DosyaAdi + uzanti;
                    Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));
                    salonfotoekle.SalonFotograf = DosyaAdi + uzanti;
                    salonfotoekle.BerberId = salonID;
                    salonfotoekle.IsActive = true;
                    salonfotoekle.SalonAd = salonad;

                }
            }

            db.SalonFotolar.Add(salonfotoekle);
            db.SaveChanges();

            return View("Index");
        }

        public ActionResult KesilenSaclarGor()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {
                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

                var salon = (from i in db.BerberSayfa where i.SalonID == sonuc select i.id).SingleOrDefault();

                var kesilenmodeller = (from i in db.KesilenSacModeller where i.BerberId == salon select i).ToList();


                return View(kesilenmodeller);
            }

            else
            {
                return View("BerberGiris");
            }

        }

        public ActionResult KesilenSacSil(int id)
        {
            KesilenSacModeller sacfotosil = new KesilenSacModeller();
            db.KesilenSacModeller.Remove(db.KesilenSacModeller.Find(id));
            db.SaveChanges();
            return View("Index", db.KesilenSacModeller);
        }

        public ActionResult KesilenSacFotoEkle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {
                return View();
            }

            else
            {
                return View("BerberGiris");
            }

        }

        [HttpPost]
        public ActionResult KesilenSacFotoEkle(string foto)
        {

            KesilenSacModeller kesilensacfotoekle = new KesilenSacModeller();

            var gelen = (AdminIslem)Session["berberkuladi"];



            if (gelen != null)
            {
                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();
                var salonID = (from i in db.BerberSayfa where i.SalonID == sonuc select i.id).SingleOrDefault();
                var salonad = (from i in db.BerberSayfa where i.SalonID == sonuc select i.SalonAd).SingleOrDefault();


                if (Request.Files.Count > 0)
                {


                    string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string TamYolYeri = "~/Resimler/KesilenSaclar/" + DosyaAdi + uzanti;
                    Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));
                    kesilensacfotoekle.KesilenSacFotolar = DosyaAdi + uzanti;
                    kesilensacfotoekle.BerberId = salonID;
                    kesilensacfotoekle.IsActive = true;
                    kesilensacfotoekle.SalonAd = salonad;

                }
            }

            db.KesilenSacModeller.Add(kesilensacfotoekle);
            db.SaveChanges();

            return View("Index");
        }

        public ActionResult PersonelDuzenle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {
                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

                var salon = (from i in db.BerberSayfa where i.SalonID == sonuc select i.id).SingleOrDefault();

                var personel = (from i in db.Personel where i.BerberId == salon select i).ToList();


                return View(personel);
            }

            else
            {
                return View("BerberGiris");
            }


        }

        public ActionResult PersonelEkle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {
                return View();
            }

            else
            {
                return View("BerberGiris");
            }

        }
        [HttpPost]
        public ActionResult PersonelEkle(string personeladsoyad)
        {

            var gelen = (AdminIslem)Session["berberkuladi"];

            Personel pekle = new Personel();

            if (gelen != null)
            {

                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.SalonAd).SingleOrDefault();
                var salonad = (from i in db.BerberSayfa where i.SalonAd == sonuc select i.SalonAd).SingleOrDefault();
                var id = (from i in db.BerberSayfa where i.SalonAd == sonuc select i.id).SingleOrDefault();



                pekle.PersonelAdSoyad = personeladsoyad;
                pekle.IsActive = true;
                pekle.BerberId = id;
                pekle.SalonAd = salonad;

                db.Personel.Add(pekle);
                db.SaveChanges();

                return View("Index");

            }

            else
            {
                return View();
            }
        }

        public ActionResult PersonelSil(int id)
        {
            Personel bsil = new Personel();
            db.Personel.Remove(db.Personel.Find(id));
            db.SaveChanges();
            return View("Index", db.Personel);

        }



        public ActionResult Berberİslem()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {


                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

                var salon = (from i in db.BerberSayfa where i.SalonID == sonuc select i.id).SingleOrDefault();

                var islemler = (from i in db.Islemler where i.BerberId == salon select i).ToList();

                return View(islemler);
            }

            else
            {
                return View("BerberGiris");
            }

        }

        public ActionResult BerberİslemEkle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {

                return View();
            }

            else
            {
                return View("BerberGiris");
            }

        }

        [HttpPost]
        public ActionResult BerberİslemEkle(string islemad, decimal islemfiyat)
        {

            var gelen = (AdminIslem)Session["berberkuladi"];

            Islemler islemekle = new Islemler();

            if (gelen != null)
            {
                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.SalonAd).SingleOrDefault();
                var salon = (from i in db.BerberSayfa where i.SalonAd == sonuc select i).SingleOrDefault();
                var id = (from i in db.BerberSayfa where i.SalonAd == sonuc select i.id).SingleOrDefault();



                islemekle.IsActive = true;
                islemekle.IslemAd = islemad;
                islemekle.IslemFiyat = islemfiyat;
                islemekle.BerberId = id;
                islemekle.SalonAd = salon.SalonAd;

                db.Islemler.Add(islemekle);
                db.SaveChanges();

                return View("Index");

            }

            else
            {
                return View();
            }
        }

        public ActionResult İslemSil(int id)
        {
            Islemler bsil = new Islemler();
            db.Islemler.Remove(db.Islemler.Find(id));
            db.SaveChanges();
            return View("Index", db.Islemler);

        }

        public ActionResult Kampanyalar()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {

                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

                var salon = (from i in db.BerberSayfa where i.SalonID == sonuc select i.id).SingleOrDefault();

                var kampanyalar = (from i in db.Kampanyalar where i.BerberId == salon select i).ToList();

                return View(kampanyalar);
            }

            else
            {
                return View("BerberGiris");
            }


        }

        public ActionResult KampanyaSil(int id)
        {
            Kampanyalar bsil = new Kampanyalar();
            db.Kampanyalar.Remove(db.Kampanyalar.Find(id));
            db.SaveChanges();
            return View("Index", db.Kampanyalar);

        }


        public ActionResult KampanyaEkle()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {

                return View();
            }

            else
            {
                return View("BerberGiris");
            }

        }

        [HttpPost]
        public ActionResult KampanyaEkle(string baslik, string icerik, decimal fiyat, DateTime tarih)
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            Kampanyalar kekle = new Kampanyalar();

            if (gelen != null)
            {
                var sonuc = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.SalonAd).SingleOrDefault();
                var salonad = (from i in db.BerberSayfa where i.SalonAd == sonuc select i.SalonAd).SingleOrDefault();
                var id = (from i in db.BerberSayfa where i.SalonAd == sonuc select i.id).SingleOrDefault();

                var simdi = DateTime.Now;

                kekle.IsActive = true;
                kekle.KampanyaBaslik = baslik;
                kekle.KampanyaIcerik = icerik;
                kekle.KampanyaFiyat = fiyat;
                kekle.BerberId = id;
                kekle.SalonAd = salonad;
                kekle.KampanyaSonGun = tarih;
                db.Kampanyalar.Add(kekle);
                db.SaveChanges();

                return View("Index");

            }

            else
            {
                ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
                return View();
            }
        }

        public ActionResult BerberRandevular()
        {
            var gelen = (AdminIslem)Session["berberkuladi"];

            if (gelen != null)
            {

                var salonkontrol = (from i in db.AdminIslem where i.BerberKulAdi == gelen.BerberKulAdi select i.id).SingleOrDefault();

                var salonid = (from i in db.BerberSayfa where i.SalonID == salonkontrol select i.id).SingleOrDefault();

                var randevukontrol = (from i in db.Randevular where i.BerberId == salonid select i).ToList();


                return View(randevukontrol);
            }

            else
            {
                return View("BerberGiris");
            }




        }



    }
}