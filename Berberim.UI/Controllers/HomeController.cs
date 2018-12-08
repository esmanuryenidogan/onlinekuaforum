using Berberim.Biz;
using Berberim.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Berberim.Data.Models;
using Berberim.UI.Models;

namespace Berberim.UI.Controllers
{
    public class HomeController : Controller
    {
        OnlineKuaforumDbContext _db = new OnlineKuaforumDbContext();

        public ActionResult Index()
        {
            var data = new tabMenu
            {
                salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteri = _db.MUSTERİ.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                randevu = _db.RANDEVU.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
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
            var kullaniciInfo = _db.MUSTERİ.FirstOrDefault(i => i.EMAIL == m.EMAIL);

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
                CİNSİYET = m.CİNSİYET,
                STATUS = Constants.RecordStatu.Active,
                TEL = m.TEL,
                FOTO = m.FOTO,
            };
            _db.MUSTERİ.Add(mekle);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult MusteriGiris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MusteriGiris(string mail, string sifre)
        {
            var sonuc = (from i in _db.MUSTERİ where i.STATUS == Constants.RecordStatu.Active && i.EMAIL == mail && i.SIFRE == sifre select i).FirstOrDefault();
            if (sonuc != null)
            {
                var data = new tabMenu
                {
                    salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                    kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                    trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                    musteri = _db.MUSTERİ.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                    randevu = _db.RANDEVU.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                    musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
                };

                Session["musteriadsoyad"] = sonuc.AD + " " + sonuc.SOYAD;
                Session["musteri"] = sonuc.EMAIL;
                return View("Index", data);
            }
            ViewBag.mesaj = "Kullanıcı Adı veya Şifre Hatalı!";
            return View();
        }

        public ActionResult CıkısYap()
        {
            Session["musteri"] = null;
            var data = new tabMenu
            {
                salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteri = _db.MUSTERİ.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                randevu = _db.RANDEVU.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
            };
            return View("Index", data);
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
                var sendMail = new MAIL()
                {
                    FROM = email,
                    TO = Constants.ContactMail,
                    MAILADRESS = message,
                    SUBJECT = "İletişim Formu",
                    //ISSEND = Constants.SendMail.Succes
                };
                _db.MAIL.Add(sendMail);
                _db.SaveChanges();
                return View($"Mailiniz iletildi.Teşekkürler!");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult SalonSayfa(int id)
        {
            var sonuc = _db.SALONSAYFA.FirstOrDefault(a => a.SALONID == id);
            var islemler = (from i in _db.ISLEM where i.SALONID == sonuc.SALONID select i).ToList();
            var personeller = (from i in _db.PERSONEL where i.SALONID == sonuc.SALONID select i).ToList();
            var musteriyorumlar = (from i in _db.YORUM where i.SALONID == id select i).ToList();
            var salonfotolar = (from i in _db.SALONFOTO where i.SALONID == id select i).ToList();
            var kesilensacmodeller = (from i in _db.BSACMODEL where i.SALONID == id select i).ToList();
            var yorumsay = (from i in _db.YORUM where i.SALONID == sonuc.SALONID select i.ID).Count();
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
        public ActionResult SalonSayfa(string yorum, string rAd, string rTelno, string rtarih, string rSaat, string rEmail, string rPersonel, string rIslem, int id)
        {
            if (Session["musteri"] == null)
                return View("MusteriGiris");

            var gelen = Session["musteri"].ToString();

            var musteriId = (from i in _db.MUSTERİ where i.EMAIL == gelen select i.ID).FirstOrDefault();
            var berberbilgi = (from i in _db.SALON where i.ID == id select i).FirstOrDefault();
            var musteribilgi = (from i in _db.MUSTERİ where i.ID == musteriId select i).FirstOrDefault();

            if (rtarih != null)
            {
                string personel = Request["Personeller"];
                string islem = Request["islemler"];

                DateTime tarih = Convert.ToDateTime(rtarih);
                TimeSpan saat = TimeSpan.Parse(rSaat);
                var simdi = DateTime.Now;
                if (tarih > simdi)
                {
                    var randevual = new RANDEVU
                    {
                        MUSTERIAD = musteribilgi?.AD,
                        MUSTERITEL = musteribilgi?.TEL,
                        MUSTERIMAIL = musteribilgi?.EMAIL,
                        SALONAD = berberbilgi?.ADSOYAD,
                        SALONTEL = berberbilgi?.TELEFON,
                        SALONMAIL = berberbilgi?.EMAIL,
                        //KOLTUKSAY = berberbilgi?.KOLTUKSAY.ToString(),
                        RANDEVUTARIH = tarih,
                        //RANDEVUSAAT = saat.ToString(),
                        PERSONEL = personel,
                        SALONID = id,
                        MUSTERIID = musteriId,
                        STATUS = Constants.RecordStatu.Active
                    };
                    _db.RANDEVU.Add(randevual);
                    _db.SaveChanges();

                    string body = "Randevu Bilgileriniz" + " " + rtarih + " " + rSaat;
                    var email = RandevuEmail.RandevuMail(body, "Randevu Bilgileri", "atakangmc@gmail.com", berberbilgi?.EMAIL, musteribilgi?.EMAIL);
                }
            }


            if (yorum != null && gelen != null)
            {
                var myorum = new YORUM()
                {
                    MYORUM = yorum,
                    MUSTERIID = musteriId,
                    SALONID = id,
                    MUSTERIAD = musteribilgi?.AD,
                    MUSTERISOYAD = musteribilgi?.SOYAD,
                    STATUS = Constants.RecordStatu.Active,
                    CİNSİYET = musteribilgi.CİNSİYET
                };
                _db.YORUM.Add(myorum);
                _db.SaveChanges();

            }
            var model = SalonSayfa(id);



            return View("SalonSayfa");
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

            var sonuc = (from i in _db.SALONSAYFA where i.SALONID == id select i).SingleOrDefault();
            var islemler = (from i in _db.ISLEM where i.SALONID == sonuc.ID select i).ToList();
            var personeller = (from i in _db.PERSONEL where i.SALONID == sonuc.ID select i).ToList();
            var musteriyorumlar = (from i in _db.YORUM where i.SALONID == sonuc.ID select i).ToList();
            var salonfotolar = (from i in _db.SALONFOTO where i.SALONID == sonuc.ID select i).ToList();
            var kesilensacmodeller = (from i in _db.BSACMODEL where i.SALONID == sonuc.ID select i).ToList();
            var kampanya = (from i in _db.KAMPANYA select i).ToList();
            var yorumsay = (from i in _db.YORUM where i.SALONID == sonuc.ID select i.ID).Count();
            ViewBag.yorumsay = yorumsay;

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
                Islemler = islemler,
                Personeller = personeller,
                SalonFotolar = salonfotolar,
                KesilenSacModeller = kesilensacmodeller,
                MusteriYorumlar = musteriyorumlar,
                Kampanya = kampanya
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
            ViewBag.kampanyaGelen = true;
            return View("SalonSayfa", model);
        }
        [HttpPost]
        public ActionResult KampanyaSatınAl(int id, string r_ad, string r_telno, string r_email, string r_personel)
        {
            var gelen = Session["musteri"].ToString();
            var musteriID = (from i in _db.MUSTERİ where i.EMAIL == gelen select i.ID).FirstOrDefault();
            var kampanyaID = (from i in _db.KAMPANYA where i.SALONID == id select i.ID).FirstOrDefault();
            var berberbilgi = (from i in _db.SALONSAYFA where i.ID == id select i).FirstOrDefault();
            var musteribilgi = (from i in _db.MUSTERİ where i.ID == musteriID select i).FirstOrDefault();
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
                    SALONAD = berberbilgi?.AD,
                    SALONTEL = "",
                    SALONMAIL = berberbilgi?.EMAIL,
                    MUSTERIAD = musteribilgi?.AD,
                    MUSTERITEL = musteribilgi?.TEL,
                    MUSTERIMAIL = musteribilgi?.EMAIL,
                    RANDEVUTARIH = tarih,
                    //RANDEVUSAAT = saat,
                    PERSONEL = personel,
                    //KOLTUKSAY = berberbilgi.KOLTUKSAY.ToString(),
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

        public ActionResult MusteriRandevuGoruntule()
        {
            var gelen = Session["musteri"].ToString();
            var musterikontrol = (from i in _db.MUSTERİ where i.EMAIL == gelen select i.ID).SingleOrDefault();
            var musterirandevu = (from i in _db.RANDEVU where i.MUSTERIID == musterikontrol select i).ToList();
            return View(musterirandevu);
        }

        [HttpPost]
        public ActionResult MailYolla()
        {
            return View();
        }

        public ActionResult İletisimSayfasi()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}