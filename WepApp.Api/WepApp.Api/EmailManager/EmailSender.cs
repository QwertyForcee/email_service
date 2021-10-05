using Microsoft.Extensions.Options;
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
    public class EmailSender:IEmailSender,IDisposable
    {
        private EmailManagerConfig _config;
        private SmtpClient _smtp;
        public EmailSender(IOptions<EmailManagerConfig> config)
        {
            this._config = config.Value;
            _smtp = new SmtpClient(_config.Host, _config.Port);
            _smtp.Credentials = new NetworkCredential(_config.AppEmail, _config.Password);
            _smtp.EnableSsl = true;
        }

        public void Send(ICronJob job, string filename)
        {
            MailAddress from = new MailAddress(_config.AppEmail, "Maxim");
            MailAddress to = new MailAddress(job.Email);
            MailMessage message = new MailMessage(from, to);

            message.Subject = job.Name;
            message.Body = job.Description;
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(filename));

            _smtp.Send(message);
        }
        public void Dispose()
        {
            this._smtp?.Dispose();
        }
    }
}
