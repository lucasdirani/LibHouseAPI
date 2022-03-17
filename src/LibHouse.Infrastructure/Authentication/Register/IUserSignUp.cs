using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Token;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register
{
    public interface IUserSignUp
    {
        Task<Result<SignUpConfirmationToken>> SignUpUserAsync(User user, string password);
        Task<Result> SendConfirmationTokenToUserAsync(SignUpConfirmationToken confirmationToken, User user);
        Task<Result> AcceptUserConfirmationTokenAsync(SignUpConfirmationToken confirmationToken, string userEmail);
    }
}