using Berberim.Biz;
using Berberim.Data;
using Berberim.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Berberim.UI.Controllers
{
    public class HomeController : Controller
    {
        BerberimEntities db = new BerberimEntities();
        public ActionResult Index()
        {

            var berber = (from i in db.BerberSayfa where i.IsActive == true select i).ToList();

            return View(berber);
        }



        public ActionResult MusteriKayit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MusteriKayit(string ad, string soyad, string adres, string mail, string tel, string dogumtarihi, string kuladi, string sifre)
        {

            var berber = (from i in db.BerberSayfa select i).ToList();

            var kullanıcıadı = (from i in db.MüsteriKayit where i.MusteriKullaniciAdi == kuladi select i.MusteriKullaniciAdi).SingleOrDefault();

            var kullanıcısifre = (from i in db.MüsteriKayit where i.MusteriKullaniciSifre == sifre select i.MusteriKullaniciSifre).SingleOrDefault();


            if (kullanıcıadı == kuladi || kullanıcısifre == sifre)
            {


                ViewBag.uyarı = "Kullanıcı Adı veya Şifre Mevcut !";
                return View();


            }

            else
            {

                Session["musterikuladi"] = kuladi;
                MüsteriKayit mekle = new MüsteriKayit();
                mekle.YetkiID = 3;
                mekle.IsActive = true;
                mekle.MusteriAd = ad;
                mekle.MusteriSoyad = soyad;
                mekle.MusteriAdres = adres;
                mekle.MusteriMail = mail;
                mekle.MusteriTel = tel;
                mekle.MusteriDogumTarihi = dogumtarihi;
                mekle.MusteriKullaniciAdi = kuladi;
                mekle.MusteriKullaniciSifre = sifre;
                db.MüsteriKayit.Add(mekle);
                db.SaveChanges();
                return View("Index", berber);

            }




        }


        public ActionResult MusteriGiris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MusteriGiris(string kuladi, string sifre)
        {


            var sonuc = (from i in db.MüsteriKayit where i.IsActive == true && i.MusteriKullaniciAdi == kuladi && i.MusteriKullaniciSifre == sifre select i).SingleOrDefault();



            if (sonuc != null)
            {
                var berber = (from i in db.BerberSayfa where i.IsActive == true select i).ToList();
                Session["musteriadsoyad"] = sonuc.MusteriAd + " " + sonuc.MusteriSoyad;
                Session["musterikuladi"] = kuladi;
                return View("Index", berber);

            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Adı veya Şifre Hatalı !";
                return View();
            }
        }

        public ActionResult CıkısYap()
        {
            var berber = (from i in db.BerberSayfa where i.IsActive == true select i).ToList();

            Session["musterikuladi"] = null;
            return View("Index", berber);


        }



        public ActionResult IletisimMail()
        {

            return View();
        }
        [HttpPost]
        public ActionResult IletisimMail(string name, string email, string message)
        {
            try
            {
                SmtpClient client = new SmtpClient("mail.gmail.com");
                MailMessage msg = new MailMessage("esmanur.yndgn@gmail.com", email);
                msg.IsBodyHtml = true;
                msg.Body = message;
                NetworkCredential sifre = new NetworkCredential("esmanur.yndgn@gmail.com", "esmanur1234");
                client.Credentials = sifre;
                client.Send(msg);
                return View("Mailiniz iletildi.");
            }
            catch (Exception)
            {

                return View();
            }


        }



        public ActionResult SalonSayfa(int id)
        {

            var gelen = Session["musterikuladi"];


            var sonuc = (from i in db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var islemler = (from i in db.Islemler where i.BerberId == sonuc.id select i).ToList();
            var personeller = (from i in db.Personel where i.BerberId == sonuc.id select i).ToList();
            var musteriyorumlar = (from i in db.MusteriYorumlari where i.BerberId == id select i).ToList();
            var salonfotolar = (from i in db.SalonFotolar where i.BerberId == id select i).ToList();
            var kesilensacmodeller = (from i in db.KesilenSacModeller where i.BerberId == id select i).ToList();
            var yorumsay = (from i in db.MusteriYorumlari where i.BerberId == sonuc.id select i.id).Count();
            ViewBag.yorumsay = yorumsay;



            BerberDetayModel model = new BerberDetayModel();

            model.Berber = sonuc;
            model.islemler = islemler;
            model.Personeller = personeller;
            model.SalonFotolar = salonfotolar;
            model.KesilenSacModeller = kesilensacmodeller;
            model.MusteriYorumlar = musteriyorumlar;


            //model.islemler = sonuc.Islemler.ToList(); Sorguya gerek kalmaz bağlantılı olduğu için

            return View(model);

        }

        [HttpPost]
        public ActionResult SalonSayfa(string text, int id)
        {
            var gelen = Session["musterikuladi"];

            var musteriID = (from i in db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();
            var berberbilgi = (from i in db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var musteribilgi = (from i in db.MüsteriKayit where i.id == musteriID select i).SingleOrDefault();

            if (gelen != null)
            {
                MusteriYorumlari myorum = new MusteriYorumlari();

                myorum.MüsteriYorumu = text;
                myorum.MusteriId = musteriID;
                myorum.BerberId = berberbilgi.id;
                myorum.SalonAd = berberbilgi.SalonAd;
                myorum.MüsteriAd = musteribilgi.MusteriAd;
                myorum.MüsteriSoyad = musteribilgi.MusteriSoyad;
                myorum.IsActive = true;


                db.MusteriYorumlari.Add(myorum);
                db.SaveChanges();

                var berber = (from i in db.BerberSayfa where i.IsActive == true select i).ToList();

                return View("Index", berber);
            }

            else
            {
                return View("MusteriGiris");
            }
        }

        public ActionResult TrendSacVitrin()
        {


            var trendsac = (from i in db.TrendSaclar select i).ToList();

            return View(trendsac);

        }

        public ActionResult KampanyaVitrin()
        {
            var kampanya = (from i in db.Kampanyalar select i).ToList();
            return View(kampanya);
        }

        public ActionResult KampanyaSatınAl(int id)
        {
            var gelen = Session["musterikuladi"];
            if (gelen == null)
            {
                return View("MusteriGiris");
            }
            else
            {

                var sonuc = (from i in db.BerberSayfa where i.id == id select i).SingleOrDefault();
                var personeller = (from i in db.Personel where i.BerberId == sonuc.id select i).ToList();

                BerberDetayModel model = new BerberDetayModel();

                model.Berber = sonuc;
                model.Personeller = personeller;

                List<string> RandevuSaat = new List<string>();
                RandevuSaat.Add("10:00");
                RandevuSaat.Add("11:00");
                RandevuSaat.Add("12:00");
                RandevuSaat.Add("13:00");
                RandevuSaat.Add("14:00");
                RandevuSaat.Add("15:00");
                RandevuSaat.Add("16:00");
                RandevuSaat.Add("17:00");
                RandevuSaat.Add("18:00");
                RandevuSaat.Add("19:00");
                RandevuSaat.Add("20:00");
                model.RandevuSaat = RandevuSaat;

                Session["gelenmodel"] = model;

                return View(model);



            }
            //var secilenp = Request.Form["personel"];
            //var gelens = Request.Form["saat"];

            //TimeSpan ts = Convert.ToDateTime(gelens).TimeOfDay;    
            //ral.RandevuTarihi = r_tarih;
            //ral.RandevuSaati = ts;
            //ral.PersonelAdSoyad = secilenp;
        }
        [HttpPost]
        public ActionResult KampanyaSatınAl(int id, string r_ad, string r_telno, string r_email, string r_personel)
        {
            var gelen = Session["musterikuladi"].ToString();

            var musteriID = (from i in db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();



            var kampanyaID = (from i in db.Kampanyalar where i.BerberId == id select i.id).SingleOrDefault();

            var berberbilgi = (from i in db.BerberSayfa where i.id == id select i).SingleOrDefault();

            var musteribilgi = (from i in db.MüsteriKayit where i.id == musteriID select i).SingleOrDefault();

            var kampanyabilgileri = (from i in db.Kampanyalar where i.id == kampanyaID select i).SingleOrDefault();

            string personel = Request["Personeller"];

            DateTime tarih = Convert.ToDateTime(Request["r_tarih"]);

            TimeSpan saat = TimeSpan.Parse(Request["r_saat"]);



            var simdi = DateTime.Now;

            if (tarih > simdi || tarih == simdi)
            {
                Randevular ral = new Randevular();
                ral.BerberId = id;
                ral.MusteriId = musteriID;
                ral.SalonAd = berberbilgi.SalonAd;
                ral.SalonTel = berberbilgi.SalonTel;
                ral.SalonMail = berberbilgi.SalonMail;

                ral.MüsteriAd = musteribilgi.MusteriAd;
                ral.MüsteriTel = musteribilgi.MusteriTel;
                ral.MusteriMail = musteribilgi.MusteriMail;

                ral.RandevuTarihi = tarih;
                ral.RandevuSaati = saat;
                ral.PersonelAdSoyad = personel;

                ral.IslemAd = kampanyabilgileri.KampanyaBaslik;
                ral.IslemFiyat = kampanyabilgileri.KampanyaFiyat;
                ral.KoltukSayisi = berberbilgi.KoltukSayisi;
                ral.IsActive = true;
                db.Randevular.Add(ral);
                db.SaveChanges();


                var berber = (from i in db.BerberSayfa select i).ToList();

                return View("Index", berber);


            }

            else
            {
                var gelenmodel = Session["gelenmodel"];
                ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
                return View(gelenmodel);
            }

        }

        public ActionResult RandevuAl(int id)
        {

            var gelen = Session["musterikuladi"];
            if (gelen == null)
            {
                return View("MusteriGiris");
            }

            else
            {
                var sonuc = (from i in db.BerberSayfa where i.id == id select i).SingleOrDefault();
                var islemler = (from i in db.Islemler where i.BerberId == sonuc.id select i).ToList();
                var personeller = (from i in db.Personel where i.BerberId == sonuc.id select i).ToList();

                BerberDetayModel model = new BerberDetayModel();

                model.Berber = sonuc;
                model.islemler = islemler;
                model.Personeller = personeller;

                List<string> RandevuSaat = new List<string>();
                RandevuSaat.Add("10:00");
                RandevuSaat.Add("11:00");
                RandevuSaat.Add("12:00");
                RandevuSaat.Add("13:00");
                RandevuSaat.Add("14:00");
                RandevuSaat.Add("15:00");
                RandevuSaat.Add("16:00");
                RandevuSaat.Add("17:00");
                RandevuSaat.Add("18:00");
                RandevuSaat.Add("19:00");
                RandevuSaat.Add("20:00");

                model.RandevuSaat = RandevuSaat;

                Session["model"] = model;

                return View("RandevuAl", model);
            }
        }

        [HttpPost]
        public ActionResult RandevuAl(string r_ad, string r_telno, string r_email, string r_personel, string r_islem, int id)
        {

            var gelen = Session["musterikuladi"];

            var musteriID = (from i in db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();

            var berberbilgi = (from i in db.BerberSayfa where i.id == id select i).SingleOrDefault();

            var musteribilgi = (from i in db.MüsteriKayit where i.id == musteriID select i).SingleOrDefault();

            string personel = Request["Personeller"];

            string islem = Request["islemler"];

            DateTime tarih = Convert.ToDateTime(Request["r_tarih"]);

            TimeSpan saat = TimeSpan.Parse(Request["r_saat"]);

            var simdi = DateTime.Now;


            if (tarih > simdi)
            {

                Randevular randevual = new Randevular();
                randevual.MüsteriAd = musteribilgi.MusteriAd;
                randevual.MüsteriTel = musteribilgi.MusteriTel;
                randevual.MusteriMail = musteribilgi.MusteriMail;

                randevual.SalonAd = berberbilgi.SalonAd;
                randevual.SalonTel = berberbilgi.SalonTel;
                randevual.SalonMail = berberbilgi.SalonMail;
                randevual.KoltukSayisi = berberbilgi.KoltukSayisi;

                randevual.RandevuTarihi = tarih;
                randevual.RandevuSaati = saat;
                randevual.PersonelAdSoyad = personel;
                randevual.IslemAd = islem;


                randevual.BerberId = id;
                randevual.MusteriId = musteriID;
                randevual.IsActive = true;

                db.Randevular.Add(randevual);
                db.SaveChanges();

                var berber = (from i in db.BerberSayfa select i).ToList();



                return View("Index", berber);

            }

            else
            {
                var model = Session["model"];
                ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
                return View(model);
            }


        }


        public ActionResult MusteriRandevuGoruntule()
        {
            var gelen = Session["musterikuladi"];

            var musterikontrol = (from i in db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();

            var musterirandevu = (from i in db.Randevular where i.MusteriId == musterikontrol select i).ToList();


            return View(musterirandevu);

        }

        [HttpPost]
        public ActionResult MailYolla()
        {


            return View();

        }





    }
}