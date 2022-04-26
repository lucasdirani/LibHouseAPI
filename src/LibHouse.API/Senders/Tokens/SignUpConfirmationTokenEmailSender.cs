using LibHouse.API.Settings.Website;
using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Register.Senders;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Email.Models;
using LibHouse.Infrastructure.Email.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LibHouse.API.Senders.Tokens
{
    public class SignUpConfirmationTokenEmailSender : ISignUpConfirmationTokenSender
    {
        private readonly LibHouseWebsiteSettings _libHouseWebsiteSettings;
        private readonly IMailService _mailService;

        public SignUpConfirmationTokenEmailSender(
            IOptions<LibHouseWebsiteSettings> libHouseWebsiteSettings,
            IMailService mailService)
        {
            _mailService = mailService;
            _libHouseWebsiteSettings = libHouseWebsiteSettings.Value;
        }

        public async Task<bool> SendSignUpConfirmationTokenToUserAsync(
            SignUpConfirmationToken confirmationToken,
            User user)
        {
            string confirmEmailAddress = _libHouseWebsiteSettings.ConfirmEmailAddress;

            string confirmationTokenMessage = BuildMessageForSendConfirmationTokenToUser(user, confirmationToken, confirmEmailAddress);

            var mailRequest = new MailRequest(toEmail: user.Email, subject: "Confirme o seu e-mail", body: confirmationTokenMessage);

            return await _mailService.SendEmailAsync(mailRequest);
        }

        private static string BuildMessageForSendConfirmationTokenToUser(
            User user,
            SignUpConfirmationToken confirmationToken,
            string confirmEmailAddress)
        {
            return $"{user.Name}, seja bem-vindo(a) ao LibHouse. Se você solicitou o cadastro " +
                $"na plataforma, confirme o seu e-mail clicando neste link: {confirmEmailAddress}/" +
                $"{user.Email}/{user.Id}/{confirmationToken.EncodedValue}";
        }
    }
}