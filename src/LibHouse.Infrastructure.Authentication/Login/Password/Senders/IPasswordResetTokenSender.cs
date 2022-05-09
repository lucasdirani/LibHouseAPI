using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Token.Models;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Login.Password.Senders
{
    public interface IPasswordResetTokenSender
    {
        Task<bool> SendPasswordResetTokenToUserAsync(PasswordResetToken passwordResetToken, User user);
    }
}