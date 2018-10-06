using Berberim.Biz;
using Berberim.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Berberim.UI.Controllers
{
    public class ContactController : Controller
    {
        OnlineKuaforumDbContext _db = new OnlineKuaforumDbContext();
        // GET: Contact
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

        public async Task<ActionResult> ContactEmail()
        {
            var body = "";
            var message = new MailMessage();
            message.To.Add(new MailAddress("atakan.gemici@ofisim.com"));  // replace with valid value 
            message.From = new MailAddress("atakangmc@gmail.com");  // replace with valid value
            message.Subject = "Your email subject";
            message.Body = "<h1>atakan<h1>";
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "38d96fd3370aa2",  // replace with valid value
                    Password = "591628a8bef258"  // replace with valid value
                }; 

                smtp.Credentials = credential;
                smtp.Host = "smtp.mailtrap.io";
                smtp.Port = 465;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return View("Index");
            }
        }
    }
}