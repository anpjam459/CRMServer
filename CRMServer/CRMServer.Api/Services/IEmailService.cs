using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CRMServer.Shared.Common;
using System.Net;
using CRMServer.Shared.Models;

namespace CRMServer.Api.Services
{
    public interface IEmailService
    {
        UserManagerResponse SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserManagerResponse SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_configuration["EmailHost:Email"]);
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = message;

                using (var client = new SmtpClient())
                {
                    client.Port = _configuration["EmailHost:Port"].ToInt();
                    client.Host = _configuration["EmailHost:ServerMail"];
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_configuration["EmailHost:Email"], _configuration["EmailHost:Password"]);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mail); 
                }

                return new UserManagerResponse
                {
                    IsSusscess = true,
                    Message = "Email send successful"
                };
            }
            catch(Exception ex)
            {
                return new UserManagerResponse
                {
                    IsSusscess = false,
                    Message = "Can't send email",
                    Errors = new string[] { ex.Message }
                };
            }
        }
    }
}
