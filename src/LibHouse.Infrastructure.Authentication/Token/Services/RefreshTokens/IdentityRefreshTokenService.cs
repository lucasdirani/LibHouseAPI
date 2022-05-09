using Ardalis.GuardClauses;
using LibHouse.Infrastructure.Authentication.Context;
using LibHouse.Infrastructure.Authentication.Token.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Token.Services.RefreshTokens
{
    public class IdentityRefreshTokenService : IRefreshTokenService
    {
        private readonly AuthenticationContext _authenticationContext;

        public IdentityRefreshTokenService(AuthenticationContext authenticationContext)
        {
            _authenticationContext = authenticationContext;
        }

        public async Task<RefreshToken> GetRefreshTokenByValueAsync(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value), "O valor do token é obrigatório.");

            return await _authenticationContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == value);
        }

        public async Task MarkRefreshTokenAsUsedAsync(RefreshToken refreshToken)
        {
            Guard.Against.Null(refreshToken, nameof(refreshToken), "O token é obrigatório.");

            refreshToken.MarkAsUsed();

            await _authenticationContext.SaveChangesAsync();
        }
    }
}