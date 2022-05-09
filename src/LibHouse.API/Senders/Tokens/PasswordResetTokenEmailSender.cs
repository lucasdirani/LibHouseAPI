using LibHouse.API.Settings.Website;
using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Login.Password.Senders;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Email.Models;
using LibHouse.Infrastructure.Email.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LibHouse.API.Senders.Tokens
{
    public class PasswordResetTokenEmailSender : IPasswordResetTokenSender
    {
        private readonly LibHouseWebsiteSettings _libHouseWebsiteSettings;
        private readonly IMailService _mailService;

        public PasswordResetTokenEmailSender(
            IOptions<LibHouseWebsiteSettings> libHouseWebsiteSettings,
            IMailService mailService)
        {
            _mailService = mailService;
            _libHouseWebsiteSettings = libHouseWebsiteSettings.Value;
        }

        public async Task<bool> SendPasswordResetTokenToUserAsync(PasswordResetToken passwordResetToken, User user)
        {
            string requestPasswordResetAddress = _libHouseWebsiteSettings.RequestPasswordResetAddress;

            string passwordResetTokenMessage = BuildMessageForSendPasswordResetTokenToUser(user, passwordResetToken, requestPasswordResetAddress);

            var mailRequest = new MailRequest(toEmail: user.Email, subject: "Redefinição de senha", body: passwordResetTokenMessage);

            return await _mailService.SendEmailAsync(mailRequest);
        }

        private static string BuildMessageForSendPasswordResetTokenToUser(
            User user,
            PasswordResetToken passwordResetToken,
            string requestPasswordResetAddress)
        {
            return $"{user.Name}, se você solicitou a sua redefinição de senha na plataforma LibHouse, " +
                $"clique neste link para concluir o processo: {requestPasswordResetAddress}/" +
                $"{user.Email}/{user.Id}/{passwordResetToken.EncodedValue}";
        }
    }
}