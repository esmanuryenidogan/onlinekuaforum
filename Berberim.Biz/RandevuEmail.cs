using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Berberim.Data.Models;

namespace Berberim.Biz
{
    public class RandevuEmail
    {
        readonly OnlineKuaforumDbContext _db;
        public RandevuEmail()
        {
            _db = new OnlineKuaforumDbContext();
        }
        public static async Task<bool> RandevuMail(string body, string subject, string from, string to, string cc)
        {

            SmtpClient client = new SmtpClient("smtp.mailtrap.io");         
            client.Credentials = new NetworkCredential("38d96fd3370aa2", "591628a8bef258");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("atakangmc@gmail.com");
            mailMessage.To.Add(to);
            mailMessage.CC.Add(cc);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            client.Send(mailMessage);

            return true;
        }

        public static async Task<bool> SalonaEmail(string body, string subject, string from, string to, string cc)
        {

            SmtpClient client = new SmtpClient("smtp.mailtrap.io");
            client.Credentials = new NetworkCredential("38d96fd3370aa2", "591628a8bef258");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("atakangmc@gmail.com");
            mailMessage.To.Add(to);
            mailMessage.CC.Add(cc);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            client.Send(mailMessage);

            return true;
        }
    }
}

