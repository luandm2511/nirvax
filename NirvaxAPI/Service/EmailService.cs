using System.Net.Mail;
using System.Net;
using DataAccess.IRepository;

namespace WebAPI.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"]))
            {
                smtpClient.Port = int.Parse(_config["EmailSettings:SmtpPort"]);
                smtpClient.Credentials = new NetworkCredential(_config["EmailSettings:SmtpUser"], _config["EmailSettings:SmtpPass"]);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:SmtpUser"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
