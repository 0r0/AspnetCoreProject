using AspnetCoreProject.Models;
using AspnetCoreProject.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _settings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            email.Body = new TextPart("Html")
            {
                Text = mailRequest.Body
             };
            using var smtp=new SmtpClient();
            smtp.Connect(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_settings.UserName, _settings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
