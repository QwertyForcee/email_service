using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.Entities;

namespace WepApp.Api.EmailManager
{
    public class EmailSender
    {
        private readonly string appEmail = "maxim.webmy@gmail.com";
        private readonly string password = "cegthgfhjkm100!";
        public void Send(ICronJob job, string filename)
        {
            MailAddress from = new MailAddress(this.appEmail, "Maxim");
            MailAddress to = new MailAddress(job.Email);
            MailMessage message = new MailMessage(from, to);

            message.Subject = job.Name;
            message.Body = job.Description;
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(filename));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(appEmail, this.password);
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
    }
}
