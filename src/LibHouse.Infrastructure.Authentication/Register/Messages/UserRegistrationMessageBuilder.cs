using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Token.Models;

namespace LibHouse.Infrastructure.Authentication.Register.Messages
{
    internal static class UserRegistrationMessageBuilder
    {
        internal static string BuildMessageForSendConfirmationTokenToUser(
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