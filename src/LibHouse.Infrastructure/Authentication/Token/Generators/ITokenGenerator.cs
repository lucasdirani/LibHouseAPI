using LibHouse.Infrastructure.Authentication.Token.Models;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Token.Generators
{
    public interface ITokenGenerator
    {
        Task<UserToken> GenerateUserTokenAsync(string userEmail);
    }
}