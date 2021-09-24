using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace APICarsGQL.Data
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            var sender = new SmtpSender(new SmtpClient("smtp.ethereal.email")
            {
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential(config.GetSection("Mailing:Email").Value, config.GetSection("Mailing:Password").Value)
            });

            Email.DefaultSender = sender;
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string content)
        {
            await Email.From(_config.GetSection("Mailing:Email").Value).To(email).Subject(subject).
                Body(content, isHtml: true).SendAsync();
        }
    }
}
