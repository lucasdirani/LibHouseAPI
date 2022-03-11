using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Token
{
    public interface ITokenGenerator
    {
        Task<UserToken> GenerateUserTokenAsync(string userEmail);
    }
}