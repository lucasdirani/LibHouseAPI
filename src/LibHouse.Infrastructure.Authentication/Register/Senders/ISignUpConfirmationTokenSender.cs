using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Token.Models;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register.Senders
{
    public interface ISignUpConfirmationTokenSender
    {
        Task<bool> SendSignUpConfirmationTokenToUserAsync(SignUpConfirmationToken confirmationToken, User user);
    }
}