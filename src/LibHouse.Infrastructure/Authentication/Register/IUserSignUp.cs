using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register
{
    public interface IUserSignUp
    {
        Task<Result<SignUpConfirmationToken>> SignUpUserAsync(User user, string password);
        Task<Result> SendConfirmationTokenToUserAsync(SignUpConfirmationToken confirmationToken, User user);
    }
}