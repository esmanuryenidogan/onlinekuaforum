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
                salon = _db.SALON.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList()
                //musteriYorumlar = _db.MusteriYorumlari.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
            };
            return View(data);
        }

        public ActionResult MusteriKayit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MusteriKayit(MUSTERI m)
        {
            var kullaniciInfo = _db.MUSTERI.FirstOrDefault(i => i.EMAIL == m.EMAIL);

            if (kullaniciInfo?.EMAIL != null)
            {
                ViewBag.uyarı = "Bu Email ile Kullanıcı Mevcut !";
                return View();
            }
            Session["musteri"] = kullaniciInfo;
            MUSTERI mekle = new MUSTERI
            {
                AD = m.AD,
                SOYAD = m.SOYAD,
                EMAIL = m.EMAIL,
                ADRES = m.ADRES,
                SIFRE = m.SIFRE,
                STATUS = Constants.RecordStatu.Active,
                DOGUMTARIHI = m.DOGUMTARIHI,
                TEL = m.TEL,
                FOTO = m.FOTO,
            };
            _db.MUSTERI.Add(mekle);
            _db.SaveChanges();

            return View("Index");
        }

        public ActionResult MusteriGiris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MusteriGiris(string mail, string sifre)
        {
            var sonuc = (from i in _db.MUSTERI where i.STATUS == Constants.RecordStatu.Active && i.EMAIL == mail && i.SIFRE == sifre select i).FirstOrDefault();
            if (sonuc != null)
            {
                Session["musteriadsoyad"] = sonuc.AD + " " + sonuc.SOYAD;
                Session["musteri"] = sonuc.EMAIL;
                return View("Index");
            }
            ViewBag.mesaj = "Kullanıcı Adı veya Şifre Hatalı!";
            return View();
        }

        public ActionResult CıkısYap()
        {
            Session["musteri"] = null;
            return View("Index");
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
                //todo:Mail gönderimi kodu eklenecek.
                SmtpClient client = new SmtpClient("mail.gmail.com");
                MailMessage msg = new MailMessage("esmanur.yndgn@gmail.com", email)
                {
                    IsBodyHtml = true,
                    Body = message
                };
                NetworkCredential sifre = new NetworkCredential("esmanur.yndgn@gmail.com", "esmanur1234");
                client.Credentials = sifre;
                client.Send(msg);
                var sendMail=new MAIL()
                {
                    FROM = email,
                    TO = Constants.ContactMail,
                    MAIL1 = message,
                    SUBJECT = "İletişim Formu",
                    ISSEND = Constants.SendMail.Succes
                };
                _db.MAIL.Add(sendMail);
                _db.SaveChanges();
                return View("Mailiniz iletildi.Teşekkürler!");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult SalonSayfa(int id)
        {
            var gelen = Session["musteri"];
            var sonuc = (from i in _db.SALON where i.ID == id select i).SingleOrDefault();
            var islemler = (from i in _db.ISLEM where i.SALONID == sonuc.ID select i).ToList();
            var personeller = (from i in _db.PERSONEL where i.SALONID == sonuc.ID select i).ToList();
            var musteriyorumlar = (from i in _db.YORUM where i.SALONID == id select i).ToList();
            var salonfotolar = (from i in _db.SALONFOTO where i.SALONID == id select i).ToList();
            var kesilensacmodeller = (from i in _db.BSACMODEL where i.SALONID == id select i).ToList();
            var yorumsay = (from i in _db.YORUM where i.SALONID == sonuc.ID select i.ID).Count();
            ViewBag.yorumsay = yorumsay;

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
                Islemler = islemler,
                Personeller = personeller,
                SalonFotolar = salonfotolar,
                KesilenSacModeller = kesilensacmodeller,
                MusteriYorumlar = musteriyorumlar
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult SalonSayfa(string text, int id)
        {
            var gelen = Session["musteri"].ToString();

            var musteriId = (from i in _db.MUSTERI where i.EMAIL == gelen select i.ID).FirstOrDefault();
            var berberbilgi = (from i in _db.SALON where i.ID == id select i).FirstOrDefault();
            var musteribilgi = (from i in _db.MUSTERI where i.ID == musteriId select i).FirstOrDefault();

            if (gelen != null)
            {
                var myorum = new YORUM()
                {
                    YORUM1 = text,
                    MUSTERIID = musteriId,
                    SALONID = berberbilgi?.ID,
                    MUSTERIAD = musteribilgi?.AD,
                    MUSTERISOYAD = musteribilgi?.SOYAD,
                    STATUS = Constants.RecordStatu.Active
                };
                _db.YORUM.Add(myorum);
                _db.SaveChanges();

                return View("Index");
            }
            return View("MusteriGiris");
        }

        public ActionResult TrendSacVitrin()
        {
            var trendsac = (from i in _db.TRENDHAIRS select i).ToList();
            return View(trendsac);
        }

        public ActionResult KampanyaVitrin()
        {
            var kampanya = (from i in _db.KAMPANYA select i).ToList();
            return View(kampanya);
        }

        public ActionResult KampanyaSatınAl(int id)
        {
            var gelen = Session["musteri"];
            if (gelen == null)
            {
                return View("MusteriGiris");
            }
            var sonuc = (from i in _db.SALON where i.ID == id select i).SingleOrDefault();
            var personeller = (from i in _db.PERSONEL where i.SALONID == sonuc.ID select i).ToList();

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
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
            Session["gelenmodel"] = model;
            return View(model);
        }
        [HttpPost]
        public ActionResult KampanyaSatınAl(int id, string r_ad, string r_telno, string r_email, string r_personel)
        {
            var gelen = Session["musteri"].ToString();
            var musteriID = (from i in _db.MUSTERI where i.EMAIL == gelen select i.ID).FirstOrDefault();
            var kampanyaID = (from i in _db.KAMPANYA where i.SALONID == id select i.ID).FirstOrDefault();
            var berberbilgi = (from i in _db.SALON where i.ID == id select i).FirstOrDefault();
            var musteribilgi = (from i in _db.MUSTERI where i.ID == musteriID select i).FirstOrDefault();
            var kampanyabilgileri = (from i in _db.KAMPANYA where i.ID == kampanyaID select i).FirstOrDefault();

            string personel = Request["Personeller"];
            DateTime tarih = Convert.ToDateTime(Request["r_tarih"]);
            TimeSpan saat = TimeSpan.Parse(Request["r_saat"]);

            var simdi = DateTime.Now;
            if (tarih > simdi || tarih == simdi)
            {
                var ral = new RANDEVU()
                {
                    SALONID = id,
                    MUSTERIID = musteriID,
                    SALONAD = berberbilgi?.SALONADI,
                    SALONTEL = "",
                    SALONMAIL = berberbilgi?.EMAIL,
                    MUSTERIAD = musteribilgi?.AD,
                    MUSTERITEL = musteribilgi?.TEL,
                    MUSTERIMAIL = musteribilgi?.EMAIL,
                    RANDEVUTARIH = tarih,
                    RANDEVUSAAT = saat.ToString(),
                    PERSONEL = personel,
                    KOLTUKSAY = berberbilgi?.KOLTUKSAYI,
                    STATUS = Constants.RecordStatu.Active
                };
                _db.RANDEVU.Add(ral);
                _db.SaveChanges();

                return View("Index");
            }

            var gelenmodel = Session["gelenmodel"];
            ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
            return View(gelenmodel);
        }

        public ActionResult RandevuAl(int id)
        {

            var gelen = Session["musteri"];
            if (gelen == null)
            {
                return View("MusteriGiris");
            }
            var sonuc = (from i in _db.SALON where i.ID == id select i).SingleOrDefault();
            var islemler = (from i in _db.ISLEM where i.SALONID == sonuc.ID select i).ToList();
            var personeller = (from i in _db.PERSONEL where i.SALONID == sonuc.ID select i).ToList();

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
                Islemler = islemler,
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
            var gelen = Session["musteri"].ToString();
            var musteriId = (from i in _db.MUSTERI where i.EMAIL == gelen select i.ID).SingleOrDefault();
            var berberbilgi = (from i in _db.SALONSAYFA where i.SALONID == id select i).SingleOrDefault();
            var musteribilgi = (from i in _db.MUSTERI where i.ID == musteriId select i).SingleOrDefault();

            string personel = Request["Personeller"];
            string islem = Request["islemler"];

            DateTime tarih = Convert.ToDateTime(Request["r_tarih"]);
            TimeSpan saat = TimeSpan.Parse(Request["r_saat"]);
            var simdi = DateTime.Now;
            if (tarih > simdi)
            {
                 var randevual = new RANDEVU
                 {
                    MUSTERIAD = musteribilgi?.AD,
                    MUSTERITEL = musteribilgi?.TEL,
                    MUSTERIMAIL = musteribilgi?.EMAIL,
                    SALONAD = berberbilgi?.AD,
                    SALONTEL = berberbilgi?.TEL,
                    SALONMAIL = berberbilgi?.EMAIL,
                    KOLTUKSAY = berberbilgi?.KOLTUKSAY.ToString(),
                    RANDEVUTARIH = tarih,
                    RANDEVUSAAT = saat.ToString(),
                    PERSONEL = personel,
                    SALONID = id,
                    MUSTERIID = musteriId,
                    STATUS = Constants.RecordStatu.Active
                };
                _db.RANDEVU.Add(randevual);
                _db.SaveChanges();

                return View("Index");
            }
                var model = Session["model"];
                ViewBag.tarih = "Geçmiş Tarih Seçilemez !";
                return View(model);
        }

        public ActionResult MusteriRandevuGoruntule()
        {
            var gelen = Session["musteri"].ToString();
            var musterikontrol = (from i in _db.MUSTERI where i.EMAIL == gelen select i.ID).SingleOrDefault();
            var musterirandevu = (from i in _db.RANDEVU where i.MUSTERIID == musterikontrol select i).ToList();
            return View(musterirandevu);
        }

        [HttpPost]
        public ActionResult MailYolla()
        {
            return View();
        }
    }
}