using LibHouse.Infrastructure.Authentication.Token.Models;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Token.Services.RefreshTokens
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetRefreshTokenByValueAsync(string value);
        Task MarkRefreshTokenAsUsedAsync(RefreshToken refreshToken);
    }
}