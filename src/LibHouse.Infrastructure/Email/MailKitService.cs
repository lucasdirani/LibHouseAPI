using LibHouse.Business.Monads;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Email
{
    public class MailKitService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailKitService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<Result> SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                MimeMessage email = BuildMessageFromMailRequest(mailRequest);

                using var smtp = new SmtpClient();

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                await smtp.SendAsync(email);

                smtp.Disconnect(true);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Falha ao enviar a mensagem: {ex.Message}");
            }
        }

        private MimeMessage BuildMessageFromMailRequest(MailRequest mailRequest)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = mailRequest.Subject,
            };

            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

            var builder = new BodyBuilder
            {
                HtmlBody = mailRequest.Body
            };

            email.Body = builder.ToMessageBody();

            return email;
        }
    }
}