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
        readonly BerberimEntities _db = new BerberimEntities();
        public ActionResult Index()
        {
            var data = new tabMenu
            {
                berber = _db.BerberSayfa.Where(i => i.IsActive == true).ToList(),
                kampanyalar = _db.Kampanyalar.Where(i => i.IsActive == true).ToList(),
                trendSac = _db.TrendSaclar.Where(i => i.IsActive == true).ToList()
            };
            return View(data);
        }

        public ActionResult MusteriKayit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MusteriKayit(string ad, string soyad, string adres, string mail, string tel, string dogumtarihi, string kuladi, string sifre)
        {
            var berber = _db.BerberSayfa.ToList();
            var kullaniciInfo = _db.MüsteriKayit.FirstOrDefault(i => i.MusteriKullaniciAdi == kuladi && i.MusteriKullaniciSifre == sifre);

            if (kullaniciInfo?.MusteriKullaniciAdi == kuladi || kullaniciInfo?.MusteriKullaniciSifre == sifre)
            {
                ViewBag.uyarı = "Kullanıcı Adı veya Şifre Mevcut !";
                return View();
            }
            Session["musterikuladi"] = kuladi;
            MüsteriKayit mekle = new MüsteriKayit
            {
                YetkiID = Constants.StatuID.Customer,
                IsActive = true,
                MusteriAd = ad,
                MusteriSoyad = soyad,
                MusteriAdres = adres,
                MusteriMail = mail,
                MusteriTel = tel,
                MusteriDogumTarihi = dogumtarihi,
                MusteriKullaniciAdi = kuladi,
                MusteriKullaniciSifre = sifre
            };
            _db.MüsteriKayit.Add(mekle);
            _db.SaveChanges();

            return View("Index", berber);
        }

        public ActionResult MusteriGiris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MusteriGiris(string kuladi, string sifre)
        {
            var sonuc = (from i in _db.MüsteriKayit where i.IsActive == true && i.MusteriKullaniciAdi == kuladi && i.MusteriKullaniciSifre == sifre select i).FirstOrDefault();
            if (sonuc != null)
            {
                Session["musteriadsoyad"] = sonuc.MusteriAd + " " + sonuc.MusteriSoyad;
                Session["musterikuladi"] = kuladi;
                return View("Index");
            }
            ViewBag.mesaj = "Kullanıcıadı veya şifre hatalı!";
            return View();
        }

        public ActionResult CıkısYap()
        {
            var berber = (from i in _db.BerberSayfa where i.IsActive == true select i).ToList();
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

            var sonuc = (from i in _db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var islemler = (from i in _db.Islemler where i.BerberId == sonuc.id select i).ToList();
            var personeller = (from i in _db.Personel where i.BerberId == sonuc.id select i).ToList();
            var musteriyorumlar = (from i in _db.MusteriYorumlari where i.BerberId == id select i).ToList();
            var salonfotolar = (from i in _db.SalonFotolar where i.BerberId == id select i).ToList();
            var kesilensacmodeller = (from i in _db.KesilenSacModeller where i.BerberId == id select i).ToList();
            var yorumsay = (from i in _db.MusteriYorumlari where i.BerberId == sonuc.id select i.id).Count();
            ViewBag.yorumsay = yorumsay;

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
                islemler = islemler,
                Personeller = personeller,
                SalonFotolar = salonfotolar,
                KesilenSacModeller = kesilensacmodeller,
                MusteriYorumlar = musteriyorumlar
            };
            //model.islemler = sonuc.Islemler.ToList(); Sorguya gerek kalmaz bağlantılı olduğu için

            return View(model);
        }

        [HttpPost]
        public ActionResult SalonSayfa(string text, int id)
        {
            var gelen = Session["musterikuladi"].ToString();

            var musteriId = (from i in _db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();
            var berberbilgi = (from i in _db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var musteribilgi = (from i in _db.MüsteriKayit where i.id == musteriId select i).SingleOrDefault();

            if (gelen != null)
            {
                MusteriYorumlari myorum = new MusteriYorumlari
                {
                    MüsteriYorumu = text,
                    MusteriId = musteriId,
                    BerberId = berberbilgi.id,
                    SalonAd = berberbilgi.SalonAd,
                    MüsteriAd = musteribilgi.MusteriAd,
                    MüsteriSoyad = musteribilgi.MusteriSoyad,
                    IsActive = true
                };
                _db.MusteriYorumlari.Add(myorum);
                _db.SaveChanges();

                return View("Index");
            }
            return View("MusteriGiris");
        }

        public ActionResult TrendSacVitrin()
        {
            var trendsac = (from i in _db.TrendSaclar select i).ToList();
            return View(trendsac);
        }

        public ActionResult KampanyaVitrin()
        {
            var kampanya = (from i in _db.Kampanyalar select i).ToList();
            return View(kampanya);
        }

        public ActionResult KampanyaSatınAl(int id)
        {
            var gelen = Session["musterikuladi"];
            if (gelen == null)
            {
                return View("MusteriGiris");
            }
            var sonuc = (from i in _db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var personeller = (from i in _db.Personel where i.BerberId == sonuc.id select i).ToList();

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
        [HttpPost]
        public ActionResult KampanyaSatınAl(int id, string r_ad, string r_telno, string r_email, string r_personel)
        {
            var gelen = Session["musterikuladi"].ToString();
            var musteriID = (from i in _db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();
            var kampanyaID = (from i in _db.Kampanyalar where i.BerberId == id select i.id).SingleOrDefault();
            var berberbilgi = (from i in _db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var musteribilgi = (from i in _db.MüsteriKayit where i.id == musteriID select i).SingleOrDefault();
            var kampanyabilgileri = (from i in _db.Kampanyalar where i.id == kampanyaID select i).SingleOrDefault();

            string personel = Request["Personeller"];
            DateTime tarih = Convert.ToDateTime(Request["r_tarih"]);
            TimeSpan saat = TimeSpan.Parse(Request["r_saat"]);

            var simdi = DateTime.Now;
            if (tarih > simdi || tarih == simdi)
            {
                Randevular ral = new Randevular
                {
                    BerberId = id,
                    MusteriId = musteriID,
                    SalonAd = berberbilgi.SalonAd,
                    SalonTel = berberbilgi.SalonTel,
                    SalonMail = berberbilgi.SalonMail,
                    MüsteriAd = musteribilgi.MusteriAd,
                    MüsteriTel = musteribilgi.MusteriTel,
                    MusteriMail = musteribilgi.MusteriMail,
                    RandevuTarihi = tarih,
                    RandevuSaati = saat,
                    PersonelAdSoyad = personel,
                    IslemAd = kampanyabilgileri.KampanyaBaslik,
                    IslemFiyat = kampanyabilgileri.KampanyaFiyat,
                    KoltukSayisi = berberbilgi.KoltukSayisi,
                    IsActive = true
                };
                _db.Randevular.Add(ral);
                _db.SaveChanges();

                return View("Index");
            }

            var gelenmodel = Session["gelenmodel"];
            ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
            return View(gelenmodel);
        }

        public ActionResult RandevuAl(int id)
        {

            var gelen = Session["musterikuladi"];
            if (gelen == null)
            {
                return View("MusteriGiris");
            }
            var sonuc = (from i in _db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var islemler = (from i in _db.Islemler where i.BerberId == sonuc.id select i).ToList();
            var personeller = (from i in _db.Personel where i.BerberId == sonuc.id select i).ToList();

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
                islemler = islemler,
                Personeller = personeller
            };

            List<string> randevuSaat = new List<string>
            {
                "10:00",
                "11:00",
                "12:00",
                "13:00",
                "14:00",
                "15:00",
                "16:00",
                "17:00",
                "18:00",
                "19:00",
                "20:00"
            };

            model.RandevuSaat = randevuSaat;

            Session["model"] = model;

            return View("RandevuAl", model);
        }

        [HttpPost]
        public ActionResult RandevuAl(string rAd, string rTelno, string rEmail, string rPersonel, string rIslem, int id)
        {
            var gelen = Session["musterikuladi"].ToString();
            var musteriId = (from i in _db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();
            var berberbilgi = (from i in _db.BerberSayfa where i.id == id select i).SingleOrDefault();
            var musteribilgi = (from i in _db.MüsteriKayit where i.id == musteriId select i).SingleOrDefault();

            string personel = Request["Personeller"];
            string islem = Request["islemler"];

            DateTime tarih = Convert.ToDateTime(Request["r_tarih"]);
            TimeSpan saat = TimeSpan.Parse(Request["r_saat"]);
            var simdi = DateTime.Now;
            if (tarih > simdi)
            {
                Randevular randevual = new Randevular
                {
                    MüsteriAd = musteribilgi?.MusteriAd,
                    MüsteriTel = musteribilgi?.MusteriTel,
                    MusteriMail = musteribilgi?.MusteriMail,
                    SalonAd = berberbilgi?.SalonAd,
                    SalonTel = berberbilgi?.SalonTel,
                    SalonMail = berberbilgi?.SalonMail,
                    KoltukSayisi = berberbilgi?.KoltukSayisi,
                    RandevuTarihi = tarih,
                    RandevuSaati = saat,
                    PersonelAdSoyad = personel,
                    IslemAd = islem,
                    BerberId = id,
                    MusteriId = musteriId,
                    IsActive = true
                };
                _db.Randevular.Add(randevual);
                _db.SaveChanges();

                return View("Index");
            }
                var model = Session["model"];
                ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
                return View(model);
        }

        public ActionResult MusteriRandevuGoruntule()
        {
            var gelen = Session["musterikuladi"].ToString();
            var musterikontrol = (from i in _db.MüsteriKayit where i.MusteriKullaniciAdi == gelen select i.id).SingleOrDefault();
            var musterirandevu = (from i in _db.Randevular where i.MusteriId == musterikontrol select i).ToList();
            return View(musterirandevu);
        }

        [HttpPost]
        public ActionResult MailYolla()
        {
            return View();
        }
    }
}