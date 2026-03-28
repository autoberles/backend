using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace autoberles_backend.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public virtual async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["Email:SmtpHost"])
            {
                Port = int.Parse(_config["Email:SmtpPort"]!),
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Email:From"]!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}