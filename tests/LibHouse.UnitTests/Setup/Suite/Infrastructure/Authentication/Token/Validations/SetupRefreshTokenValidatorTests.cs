﻿using LibHouse.Data.Context;
using LibHouse.Data.Repositories.Users;
using LibHouse.Infrastructure.Authentication.Context;
using LibHouse.Infrastructure.Authentication.Token.Generators;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Authentication.Token.Settings;
using LibHouse.UnitTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LibHouse.UnitTests.Setup.Suite.Infrastructure.Authentication.Token.Validations
{
    internal static class SetupRefreshTokenValidatorTests
    {
        private static readonly string _tokenKey = Guid.NewGuid().ToString();
        private static readonly string _tokenIssuer = "LibHouse";
        private static readonly string _tokenValidIn = "https://localhost";

        internal static TokenValidationParameters SetupTokenValidationParameters()
        {
            return new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _tokenIssuer,
                ValidAudience = _tokenValidIn,
            };
        }

        internal static RefreshToken SetupRefreshToken(
            SecurityToken validatedToken,
            IdentityUser userWhoOwnsTheToken,
            DateTime? createdAt = null,
            DateTime? expiresIn = null,
            string jwtId = null,
            bool isUsed = false,
            bool isRevoked = false)
        {
            return new(
                token: Guid.NewGuid().ToString(), 
                jwtId: jwtId ?? validatedToken.Id, 
                isUsed: isUsed, 
                isRevoked: isRevoked, 
                createdAt: createdAt ?? DateTime.UtcNow,
                expiresIn: expiresIn ?? DateTime.UtcNow.AddDays(3), 
                user: userWhoOwnsTheToken);
        }

        internal static ITokenGenerator SetupTokenGenerator(
            IdentityUser userWhoOwnsTheToken,
            LibHouseContext libHouseContext,
            AuthenticationContext authenticationContext,
            int tokenExpirationInSeconds)
        {
            Mock<UserManager<IdentityUser>> userManager = MockHelper.CreateMockForUserManager();
            userManager.Setup(u => u.FindByEmailAsync(userWhoOwnsTheToken.Email)).ReturnsAsync(userWhoOwnsTheToken);
            userManager.Setup(u => u.GetClaimsAsync(userWhoOwnsTheToken)).ReturnsAsync(new List<Claim>());
            userManager.Setup(u => u.GetRolesAsync(userWhoOwnsTheToken)).ReturnsAsync(new List<string>());

            Mock<IOptions<TokenSettings>> tokenSettings = new();
            tokenSettings.Setup(t => t.Value).Returns(new TokenSettings() 
            { 
                Secret = _tokenKey, 
                ExpiresInSeconds = tokenExpirationInSeconds, 
                Issuer = _tokenIssuer, 
                ValidIn = _tokenValidIn
            });

            return new JwtTokenGenerator(
                userManager.Object, 
                authenticationContext, 
                new UserRepository(libHouseContext), 
                tokenSettings.Object
            );
        }
    }
}