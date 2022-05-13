using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Token.Models;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Login.Password
{
    public interface IUserPasswordReset
    {
        Task<Result<PasswordResetToken>> RequestPasswordResetAsync(string userEmail);
        Task<Result> SendPasswordResetTokenToUserAsync(PasswordResetToken passwordResetToken, User user);
        Task<Result> AcceptUserPasswordResetTokenAsync(PasswordResetToken passwordResetToken, string userEmail, string userNewPassword);
    }
}