using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Token;

namespace LibHouse.Infrastructure.Authentication.Register.Messages
{
    internal static class UserRegistrationMessageBuilder
    {
        internal static string BuildMessageForSendConfirmationTokenToUser(
            User user,
            SignUpConfirmationToken confirmationToken,
            string confirmEmailAddress)
        {
            return $"{user.Name}, seja bem-vindo(a) ao LibHouse.\n\n Se você solicitou o cadastro " +
                $"na plataforma, confirme o seu e-mail clicando neste link: {confirmEmailAddress}?" +
                $"confirmationToken={confirmationToken.EncodedValue}&userEmail={user.Email}&userId={user.Id}";
        }
    }
}