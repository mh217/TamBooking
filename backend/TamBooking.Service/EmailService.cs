using Microsoft.Extensions.Options;
using System.Net.Mail;
using TamBooking.Common;
using TamBooking.Model.DomainModels;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IJwtService _jwtService;

        public EmailService(IOptions<EmailSettings> emailSettings, IJwtService jwtService)
        {
            _emailSettings = emailSettings.Value;
            _jwtService = jwtService;
        }

        public async Task SendEmailAsync(EmailModel emailModel)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = emailModel.Subject,
                Body = emailModel.Body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(emailModel.ToEmail);

            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);

            smtpClient.EnableSsl = _emailSettings.EnableSsl;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}