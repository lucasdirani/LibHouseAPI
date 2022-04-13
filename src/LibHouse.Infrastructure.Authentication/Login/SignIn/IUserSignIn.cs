using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Login.Models;
using LibHouse.Infrastructure.Authentication.Token.Models;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register.SignIn
{
    public interface IUserSignIn
    {
        Task<Result<AuthenticatedUser>> SignInUserAsync(string userEmail, string userPassword);
        Task<Result<AuthenticatedUser>> SignInUserAsync(string userEmail, AccessToken accessToken, RefreshToken refreshToken);
    }
}