using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Authentication.Token.Validations;
using LibHouse.UnitTests.Configuration;
using LibHouse.UnitTests.Setup.Suite.Infrastructure.Authentication.Token.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace LibHouse.UnitTests.Suite.Infrastructure.Authentication.Token.Validations
{
    public class RefreshTokenValidatorTests : BaseUnitTest
    {
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public RefreshTokenValidatorTests()
        {
            _refreshTokenValidator = new RefreshTokenValidator();
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [Fact]
        public async Task CheckIfRefreshTokenCanBeUsedWithAccessToken_ValidRefreshTokenWithExpiredAccessToken_ReturnsSuccess()
        {
            IdentityUser userWhoOwnsTheToken = new() { Id = Guid.NewGuid().ToString(), Email = "lucas.dirani@gmail.com", EmailConfirmed = true };

            var tokenGenerator = SetupRefreshTokenValidatorTests.SetupTokenGeneratorForValidRefreshTokenWithExpiredAccessToken(userWhoOwnsTheToken, LibHouseContext, AuthenticationContext);

            TokenValidationParameters tokenValidationParams = SetupRefreshTokenValidatorTests.SetupTokenValidationParameters();

            UserToken userToken = await tokenGenerator.GenerateUserTokenAsync(userWhoOwnsTheToken.Email);

            ClaimsPrincipal accessTokenClaims = _tokenHandler.ValidateToken(userToken.AccessToken, tokenValidationParams, out var validatedToken);

            RefreshToken refreshToken = SetupRefreshTokenValidatorTests.SetupRefreshTokenForValidRefreshTokenWithExpiredAccessToken(validatedToken, userWhoOwnsTheToken);

            await AwaitForAccessTokenExpire(userToken.ExpiresIn);

            Result refreshTokenValidation = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, validatedToken, accessTokenClaims);

            Assert.True(refreshTokenValidation.IsSuccess);
        }

        private static async Task AwaitForAccessTokenExpire(double tokenExpiresIn)
        {
            await Task.Delay(TimeSpan.FromSeconds(tokenExpiresIn));
        }
    }
}