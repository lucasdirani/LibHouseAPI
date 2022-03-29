using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Shared.Helpers.String;
using Microsoft.AspNetCore.Identity;
using System;

namespace LibHouse.Infrastructure.Authentication.Token.Generators
{
    public class JwtRefreshTokenGenerator : IRefreshTokenGenerator
    {
        const int RefreshTokenLength = 35;
        const int MonthsToExpire = 6;

        private readonly IdentityUser _identityUser;

        public JwtRefreshTokenGenerator(IdentityUser identityUser)
        {
            if (!identityUser.EmailConfirmed || identityUser.LockoutEnd is not null)
            {
                throw new ArgumentException("O usuário não está apto para obter um refresh token", nameof(identityUser));
            }

            _identityUser = identityUser;
        }

        public RefreshToken GenerateRefreshToken(string accessTokenId)
        {
            if (string.IsNullOrEmpty(accessTokenId))
            {
                throw new ArgumentNullException(nameof(accessTokenId), "O id do token é obrigatório");
            }

            return new RefreshToken(
                token: GenerateTokenSequence(RefreshTokenLength),
                jwtId: accessTokenId,
                isUsed: false,
                isRevoked: false,
                createdAt: DateTime.UtcNow,
                expiresIn: DateTime.UtcNow.AddMonths(MonthsToExpire),
                user: _identityUser
            );
        }

        private static string GenerateTokenSequence(int length)
        {
            return string.Concat(RandomStringGenerator.GenerateRandomString(length), Guid.NewGuid());
        }
    }
}