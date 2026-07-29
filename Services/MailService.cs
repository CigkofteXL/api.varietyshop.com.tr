using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace api.varietyshop.com.tr.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(
                _config["EmailSettings:SenderName"],
                _config["EmailSettings:SenderEmail"]));

            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            // Mail içeriğini HTML olarak ayarlıyoruz ki fiyakalı tasarımlar yapabilelim
            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                // VDS ve Local fark etmeksizin güvenli bağlantı kurar
                await smtp.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:Password"]);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // VDS'te port kapalıysa veya şifre yanlışsa buraya düşer
                Console.WriteLine($"Siber Posta Hatası: {ex.Message}");
                throw; // İstersen hatayı yutabilirsin ama API'de görmek için fırlatıyoruz
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}