﻿using LibHouse.Business.Monads;
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

            int tokenExpirationInSeconds = 1;

            var tokenGenerator = SetupRefreshTokenValidatorTests.SetupTokenGenerator(userWhoOwnsTheToken, LibHouseContext, AuthenticationContext, tokenExpirationInSeconds);

            TokenValidationParameters tokenValidationParams = SetupRefreshTokenValidatorTests.SetupTokenValidationParameters();

            UserToken userToken = await tokenGenerator.GenerateUserTokenAsync(userWhoOwnsTheToken.Email);

            ClaimsPrincipal accessTokenClaims = _tokenHandler.ValidateToken(userToken.AccessToken, tokenValidationParams, out var validatedToken);

            RefreshToken refreshToken = SetupRefreshTokenValidatorTests.SetupRefreshToken(validatedToken, userWhoOwnsTheToken);

            await AwaitForAccessTokenExpire(userToken.ExpiresIn);

            Result refreshTokenValidation = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, validatedToken, accessTokenClaims);

            Assert.True(refreshTokenValidation.IsSuccess);
        }

        [Fact]
        public async Task CheckIfRefreshTokenCanBeUsedWithAccessToken_ValidRefreshTokenWithNonExpiredAccessToken_ReturnsFailure()
        {
            IdentityUser userWhoOwnsTheToken = new() { Id = Guid.NewGuid().ToString(), Email = "lucas.dirani@gmail.com", EmailConfirmed = true };

            int tokenExpirationInSeconds = 600;

            var tokenGenerator = SetupRefreshTokenValidatorTests.SetupTokenGenerator(userWhoOwnsTheToken, LibHouseContext, AuthenticationContext, tokenExpirationInSeconds);

            TokenValidationParameters tokenValidationParams = SetupRefreshTokenValidatorTests.SetupTokenValidationParameters();

            UserToken userToken = await tokenGenerator.GenerateUserTokenAsync(userWhoOwnsTheToken.Email);

            ClaimsPrincipal accessTokenClaims = _tokenHandler.ValidateToken(userToken.AccessToken, tokenValidationParams, out var validatedToken);

            RefreshToken refreshToken = SetupRefreshTokenValidatorTests.SetupRefreshToken(validatedToken, userWhoOwnsTheToken);

            Result refreshTokenValidation = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, validatedToken, accessTokenClaims);

            Assert.True(refreshTokenValidation.Failure);
        }

        [Fact]
        public async Task CheckIfRefreshTokenCanBeUsedWithAccessToken_UsedRefreshToken_ReturnsFailure()
        {
            IdentityUser userWhoOwnsTheToken = new() { Id = Guid.NewGuid().ToString(), Email = "lucas.dirani@gmail.com", EmailConfirmed = true };

            int tokenExpirationInSeconds = 1;

            var tokenGenerator = SetupRefreshTokenValidatorTests.SetupTokenGenerator(userWhoOwnsTheToken, LibHouseContext, AuthenticationContext, tokenExpirationInSeconds);

            TokenValidationParameters tokenValidationParams = SetupRefreshTokenValidatorTests.SetupTokenValidationParameters();

            UserToken userToken = await tokenGenerator.GenerateUserTokenAsync(userWhoOwnsTheToken.Email);

            ClaimsPrincipal accessTokenClaims = _tokenHandler.ValidateToken(userToken.AccessToken, tokenValidationParams, out var validatedToken);

            RefreshToken refreshToken = SetupRefreshTokenValidatorTests.SetupRefreshToken(validatedToken, userWhoOwnsTheToken, isUsed: true);

            await AwaitForAccessTokenExpire(userToken.ExpiresIn);

            Result refreshTokenValidation = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, validatedToken, accessTokenClaims);

            Assert.True(refreshTokenValidation.Failure);
        }

        [Fact]
        public async Task CheckIfRefreshTokenCanBeUsedWithAccessToken_RevokedRefreshToken_ReturnsFailure()
        {
            IdentityUser userWhoOwnsTheToken = new() { Id = Guid.NewGuid().ToString(), Email = "lucas.dirani@gmail.com", EmailConfirmed = true };

            int tokenExpirationInSeconds = 1;

            var tokenGenerator = SetupRefreshTokenValidatorTests.SetupTokenGenerator(userWhoOwnsTheToken, LibHouseContext, AuthenticationContext, tokenExpirationInSeconds);

            TokenValidationParameters tokenValidationParams = SetupRefreshTokenValidatorTests.SetupTokenValidationParameters();

            UserToken userToken = await tokenGenerator.GenerateUserTokenAsync(userWhoOwnsTheToken.Email);

            ClaimsPrincipal accessTokenClaims = _tokenHandler.ValidateToken(userToken.AccessToken, tokenValidationParams, out var validatedToken);

            RefreshToken refreshToken = SetupRefreshTokenValidatorTests.SetupRefreshToken(validatedToken, userWhoOwnsTheToken, isRevoked: true);

            await AwaitForAccessTokenExpire(userToken.ExpiresIn);

            Result refreshTokenValidation = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, validatedToken, accessTokenClaims);

            Assert.True(refreshTokenValidation.Failure);
        }

        [Fact]
        public async Task CheckIfRefreshTokenCanBeUsedWithAccessToken_RefreshTokenAndAccessTokenDontMatch_ReturnsFailure()
        {
            IdentityUser userWhoOwnsTheToken = new() { Id = Guid.NewGuid().ToString(), Email = "lucas.dirani@gmail.com", EmailConfirmed = true };

            int tokenExpirationInSeconds = 1;

            var tokenGenerator = SetupRefreshTokenValidatorTests.SetupTokenGenerator(userWhoOwnsTheToken, LibHouseContext, AuthenticationContext, tokenExpirationInSeconds);

            TokenValidationParameters tokenValidationParams = SetupRefreshTokenValidatorTests.SetupTokenValidationParameters();

            UserToken userToken = await tokenGenerator.GenerateUserTokenAsync(userWhoOwnsTheToken.Email);

            ClaimsPrincipal accessTokenClaims = _tokenHandler.ValidateToken(userToken.AccessToken, tokenValidationParams, out var validatedToken);

            RefreshToken refreshToken = SetupRefreshTokenValidatorTests.SetupRefreshToken(validatedToken, userWhoOwnsTheToken, jwtId: Guid.NewGuid().ToString());

            await AwaitForAccessTokenExpire(userToken.ExpiresIn);

            Result refreshTokenValidation = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, validatedToken, accessTokenClaims);

            Assert.True(refreshTokenValidation.Failure);
        }

        [Fact]
        public async Task CheckIfRefreshTokenCanBeUsedWithAccessToken_ExpiredRefreshToken_ReturnsFailure()
        {
            IdentityUser userWhoOwnsTheToken = new() { Id = Guid.NewGuid().ToString(), Email = "lucas.dirani@gmail.com", EmailConfirmed = true };

            int tokenExpirationInSeconds = 1;

            var tokenGenerator = SetupRefreshTokenValidatorTests.SetupTokenGenerator(userWhoOwnsTheToken, LibHouseContext, AuthenticationContext, tokenExpirationInSeconds);

            TokenValidationParameters tokenValidationParams = SetupRefreshTokenValidatorTests.SetupTokenValidationParameters();

            UserToken userToken = await tokenGenerator.GenerateUserTokenAsync(userWhoOwnsTheToken.Email);

            ClaimsPrincipal accessTokenClaims = _tokenHandler.ValidateToken(userToken.AccessToken, tokenValidationParams, out var validatedToken);

            DateTime refreshTokenCreatedAt = DateTime.UtcNow.AddDays(-30);

            DateTime refreshTokenExpiresIn = DateTime.UtcNow.AddDays(-1);

            RefreshToken refreshToken = SetupRefreshTokenValidatorTests.SetupRefreshToken(validatedToken, userWhoOwnsTheToken, refreshTokenCreatedAt, refreshTokenExpiresIn);

            await AwaitForAccessTokenExpire(userToken.ExpiresIn);

            Result refreshTokenValidation = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, validatedToken, accessTokenClaims);

            Assert.True(refreshTokenValidation.Failure);
        }

        private static async Task AwaitForAccessTokenExpire(double tokenExpiresIn)
        {
            await Task.Delay(TimeSpan.FromSeconds(tokenExpiresIn));
        }
    }
}