using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Travel.Configurations;

namespace Travel.Services
{
    public class EmailService : IEmailService
    {
       
            private readonly SmtpClient _smtpClient;

            public EmailService()
            {
                _smtpClient = new SmtpClient
                {
                    Host = "journo024@gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("journo024@gmail.com", "ggsf jzun kiar iagb")
                };
            }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            
            var mailMessage = new MailMessage();
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true; // This is the key to send HTML content
            mailMessage.From = new MailAddress("journo024@gmail.com");



            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("journo024@gmail.com", "ggsf jzun kiar iagb");
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }

            
        }

    }


}

