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
        public static async Task<bool> RandevuMail(string body,string subject,string from,string to,string cc)
        {
           
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));   
            message.From = new MailAddress(from);
            message.CC.Add(new MailAddress(cc));
            message.Subject = subject;
            message.Body = body;
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

                return true;
            }
        }
    }
}
