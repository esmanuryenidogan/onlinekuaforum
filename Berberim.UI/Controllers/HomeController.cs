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
            var musteriEmail = "";

            if (Session["musteri"] != null)
                musteriEmail = Session["musteri"].ToString();

            var data = new tabMenu
            {
                salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                musteri = _db.MUSTERİ.Where(i => i.EMAIL == musteriEmail).ToList(),
                randevu = _db.RANDEVU.Where(i => i.MUSTERIMAIL == musteriEmail).ToList(),
                musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
            };

            if (data.salon.Count > 5)
                ViewBag.tumSalonButtonShow = true;

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
                    salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                    kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                    trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                    musteri = _db.MUSTERİ.Where(i => i.EMAIL == sonuc.EMAIL).ToList(),
                    randevu = _db.RANDEVU.Where(i => i.MUSTERIMAIL == sonuc.EMAIL).ToList(),
                    musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
                };

                var maxDate = DateTime.MinValue;
                var randevuBilgi = "";
                if (data.randevu.Count > 0)
                {
                    foreach (var randevu in data.randevu)
                    {
                        maxDate = randevu.RANDEVUTARIH > maxDate ? randevu.RANDEVUTARIH : maxDate;
                        randevuBilgi = randevu.SALONAD + " " + randevu.RANDEVUSAAT + " ";
                    }

                    ViewBag.randevum = randevuBilgi + maxDate;
                }


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
                salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                musteri = _db.MUSTERİ.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                randevu = _db.RANDEVU.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
            };
            ViewBag.tumSalonButtonShow = true;
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
            var sonuc = (from i in _db.SALONSAYFA where i.SALONID == id select i).FirstOrDefault();
            var islemler = (from i in _db.ISLEM where i.SALONID == sonuc.SALONID select i).ToList();
            var personeller = (from i in _db.PERSONEL where i.SALONID == sonuc.SALONID select i).ToList();
            var musteriyorumlar = (from i in _db.YORUM where i.SALONID == sonuc.SALONID select i).ToList();
            var salonfotolar = (from i in _db.SALONFOTO where i.SALONID == sonuc.SALONID select i).ToList();
            var kesilensacmodeller = (from i in _db.BSACMODEL where i.SALONID == sonuc.SALONID select i).ToList();
            var yorumsay = (from i in _db.YORUM where i.SALONID == sonuc.SALONID select i.ID).Count();
            var salonlar = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).Take(6).ToList();
            var salonLogo = (from i in _db.SALONLOGO where i.SALONID == sonuc.SALONID select i).FirstOrDefault();
            ViewBag.yorumsay = yorumsay;

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
                Islemler = islemler,
                Personeller = personeller,
                SalonFotolar = salonfotolar,
                KesilenSacModeller = kesilensacmodeller,
                MusteriYorumlar = musteriyorumlar,
                Salonlar = salonlar,
                SalonLogo = salonLogo
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult SalonSayfa(string yorum, string rAd, string rTelno, string rtarih, string rSaat, string rEmail, string personeller, string rIslem, int id)
        {
            if (Session["musteri"] == null)
                return View("MusteriGiris");

            var gelen = Session["musteri"].ToString();

            var musteriId = (from i in _db.MUSTERİ where i.EMAIL == gelen select i.ID).FirstOrDefault();
            var berberbilgi = (from i in _db.SALONSAYFA where i.ID == id select i).FirstOrDefault();
            var musteribilgi = (from i in _db.MUSTERİ where i.ID == musteriId select i).FirstOrDefault();
            var kampanyaID = (from i in _db.KAMPANYA where i.SALONID == id select i.ID).FirstOrDefault();
            var kampanyabilgileri = (from i in _db.KAMPANYA where i.ID == kampanyaID select i).FirstOrDefault();


            if (rtarih != null)
            {
                string personel = Request["Personeller"];
                var islem = Convert.ToInt32(Request["islemler"]);
                var islemBilgisi = (from i in _db.ISLEM where i.ID == islem select i).FirstOrDefault();

                DateTime tarih = Convert.ToDateTime(rtarih);
                var simdi = DateTime.Now;
                if (tarih > simdi)
                {
                    var randevual = new RANDEVU();

                    randevual.MUSTERIAD = musteribilgi?.AD;
                    randevual.MUSTERITEL = musteribilgi?.TEL;
                    randevual.MUSTERIMAIL = musteribilgi?.EMAIL;
                    randevual.SALONAD = berberbilgi?.AD;
                    randevual.SALONTEL = berberbilgi?.TEL;
                    randevual.SALONMAIL = berberbilgi?.EMAIL;
                    randevual.ISLEMID = islem;
                    randevual.ISLEMADI = islemBilgisi.AD;
                    randevual.ISLEMFIYAT = islemBilgisi.FIYAT.ToString();
                    randevual.RANDEVUTARIH = tarih;
                    randevual.RANDEVUSAAT = rSaat;
                    randevual.PERSONEL = personel;
                    randevual.SALONID = id;
                    randevual.MUSTERIID = musteriId;
                    randevual.STATUS = Constants.RecordStatu.Active;




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
            var personeller = (from i in _db.PERSONEL where i.SALONID == sonuc.SALONID select i).ToList();
            var musteriyorumlar = (from i in _db.YORUM where i.SALONID == sonuc.SALONID select i).ToList();
            var salonfotolar = (from i in _db.SALONFOTO where i.SALONID == sonuc.SALONID select i).ToList();
            var kesilensacmodeller = (from i in _db.BSACMODEL where i.SALONID == sonuc.SALONID select i).ToList();
            var kampanya = (from i in _db.KAMPANYA select i).SingleOrDefault();
            var yorumsay = (from i in _db.YORUM where i.SALONID == sonuc.SALONID select i.ID).Count();
            var salonlar = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).Take(6).ToList();
            var salonLogo = (from i in _db.SALONLOGO where i.SALONID == sonuc.SALONID select i).FirstOrDefault();
            ViewBag.yorumsay = yorumsay;

            BerberDetayModel model = new BerberDetayModel
            {
                Berber = sonuc,
                Islemler = islemler,
                Personeller = personeller,
                SalonFotolar = salonfotolar,
                KesilenSacModeller = kesilensacmodeller,
                MusteriYorumlar = musteriyorumlar,
                Kampanya = kampanya,
                Salonlar = salonlar,
                SalonLogo = salonLogo
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
        public ActionResult KampanyaSatınAl(string rtarih, string rSaat, string rEmail, string rPersonel, string rIslem, int id)
        {
            if (Session["musteri"] == null)
                return View("MusteriGiris");

            var gelen = Session["musteri"].ToString();

            var musteriId = (from i in _db.MUSTERİ where i.EMAIL == gelen select i.ID).FirstOrDefault();
            var berberbilgi = (from i in _db.SALONSAYFA where i.ID == id select i).FirstOrDefault();
            var musteribilgi = (from i in _db.MUSTERİ where i.ID == musteriId select i).FirstOrDefault();
            var kampanyaID = (from i in _db.KAMPANYA where i.SALONID == id select i.ID).FirstOrDefault();
            var kampanyabilgileri = (from i in _db.KAMPANYA where i.ID == kampanyaID select i).FirstOrDefault();


            if (rtarih != null)
            {
                string personel = Request["Personeller"];
                var islem = Convert.ToInt32(Request["islemler"]);
                var islemBilgisi = (from i in _db.ISLEM where i.ID == islem select i).FirstOrDefault();

                DateTime tarih = Convert.ToDateTime(rtarih);
                var simdi = DateTime.Now;
                if (tarih > simdi)
                {
                    RANDEVU randevual = new RANDEVU();

                    randevual.MUSTERIAD = musteribilgi?.AD;
                    randevual.MUSTERITEL = musteribilgi?.TEL;
                    randevual.MUSTERIMAIL = musteribilgi?.EMAIL;
                    randevual.SALONAD = berberbilgi?.AD;
                    randevual.SALONTEL = berberbilgi?.TEL;
                    randevual.SALONMAIL = berberbilgi?.EMAIL;
                    randevual.ISLEMID = islem;
                    randevual.ISLEMADI = kampanyabilgileri.BASLIK;
                    randevual.ISLEMFIYAT = kampanyabilgileri.FIYAT.ToString();
                    randevual.RANDEVUTARIH = tarih;
                    randevual.RANDEVUSAAT = rSaat;
                    randevual.PERSONEL = personel;
                    randevual.SALONID = id;
                    randevual.MUSTERIID = musteriId;
                    randevual.STATUS = Constants.RecordStatu.Active;

                    _db.RANDEVU.Add(randevual);
                    _db.SaveChanges();

                    string body = "Randevu Bilgileriniz" + " " + rtarih + " " + rSaat;
                    var email = RandevuEmail.RandevuMail(body, "Randevu Bilgileri", "atakangmc@gmail.com", berberbilgi?.EMAIL, musteribilgi?.EMAIL);
                }
                var Indexİtem = Index();
                ViewData["Model"] = Indexİtem;
                var model = ViewData.Model;

                return View("Index", model);
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
            var sonuc = (from i in _db.SALONSAYFA where i.ID == id select i).SingleOrDefault();
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

        public ActionResult TumSalonlar()
        {
            var musteriEmail = "";

            if (Session["musteri"] != null)
                musteriEmail = Session["musteri"].ToString();

            var data = new tabMenu
            {
                salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteri = _db.MUSTERİ.Where(i => i.EMAIL == musteriEmail).ToList(),
                randevu = _db.RANDEVU.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
            };
            ViewBag.tumSalonButtonShow = false;

            return View("Index", data);

        }
        public ActionResult MusteriGuncelle(int ID)
        {
            var musteriEmail = "";

            if (Session["musteri"] != null)
                musteriEmail = Session["musteri"].ToString();

            var salonControl = (from i in _db.MUSTERİ where i.EMAIL == musteriEmail select i).FirstOrDefault();

            return View("MusteriGuncelle", _db.MUSTERİ.Find(ID));


        }

        [HttpPost]
        public ActionResult MusteriGuncelle(MUSTERI u)
        {
            var mevcut = _db.MUSTERİ.Find(u.ID);
            string salonadeski = mevcut?.AD;

            if (mevcut != null)
            {
                mevcut.AD = u.AD;
                mevcut.CİNSİYET = u.CİNSİYET;
                mevcut.TEL = u.TEL;
                mevcut.CİNSİYET = u.SIFRE;
                mevcut.SOYAD = u.SOYAD;
            }

            _db.SaveChanges();
            var musteriEmail = "";

            if (Session["musteri"] != null)
                musteriEmail = Session["musteri"].ToString();

            var data = new tabMenu
            {
                salon = _db.SALONSAYFA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                kampanya = _db.KAMPANYA.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                trendSac = _db.TRENDHAIRS.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList(),
                musteri = _db.MUSTERİ.Where(i => i.EMAIL == musteriEmail).ToList(),
                randevu = _db.RANDEVU.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList(),
                musteriYorum = _db.YORUM.Where(i => i.STATUS == Constants.RecordStatu.Active).ToList().Take(5).ToList()
            };
            ViewBag.tumSalonButtonShow = true;

            return View("Index", data);
        }


        public ActionResult SalonaMail(string mesaj)
        {
            //var email = RandevuEmail.SalonaEmail();
            return View("SalonSayfa");
        }
    }
}