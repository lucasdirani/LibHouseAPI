using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Context;
using LibHouse.Infrastructure.Authentication.Extensions.Common;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Authentication.Token.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Token.Generators
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IUserRepository _userRepository;
        private readonly TokenSettings _tokenSettings;

        public JwtTokenGenerator(
            UserManager<IdentityUser> userManager,
            AuthenticationContext authenticationContext,
            IUserRepository userRepository, 
            IOptions<TokenSettings> tokenSettings)
        {
            _userManager = userManager;
            _authenticationContext = authenticationContext;
            _userRepository = userRepository;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<UserToken> GenerateUserTokenAsync(string userEmail)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(userEmail);

            ClaimsIdentity identityClaims = await GenerateIdentityClaimsAsync(user);

            (SecurityToken securityToken, string accessToken) = GenerateAccessToken(identityClaims);

            string refreshToken = await GenerateAndStoreRefreshTokenAsync(user, securityToken.Id);

            return new UserToken(
                user: await GetAuthenticatedUserDataAsync(userEmail),
                accessToken: accessToken,
                expiresIn: TimeSpan.FromSeconds(_tokenSettings.ExpiresInSeconds).TotalSeconds,
                refreshToken: refreshToken,
                claims: identityClaims.Claims.Select(c => new UserClaim(c.Type, c.Value))
            );
        }

        private async Task<AuthenticatedUser> GetAuthenticatedUserDataAsync(string userEmail)
        {
            return await _userRepository.GetProjectionAsync(
                u => u.Email == userEmail, 
                u => new AuthenticatedUser(u.Id, u.Name, u.LastName, u.BirthDate, u.Gender, u.Email, u.UserType)
            );
        }

        private async Task<string> GenerateAndStoreRefreshTokenAsync(IdentityUser user, string accessTokenId)
        {
            IRefreshTokenGenerator refreshTokenGenerator = new JwtRefreshTokenGenerator(user);

            RefreshToken refreshToken = refreshTokenGenerator.GenerateRefreshToken(accessTokenId);

            await _authenticationContext.RefreshTokens.AddAsync(refreshToken);

            await _authenticationContext.SaveChangesAsync();

            return refreshToken.Token;
        }

        private (SecurityToken securityToken, string accessToken) GenerateAccessToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            var symmetricKey = new SymmetricSecurityKey(key);

            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            SecurityToken token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.ValidIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddSeconds(_tokenSettings.ExpiresInSeconds),
                SigningCredentials = signingCredentials,
            });

            return (token, tokenHandler.WriteToken(token));
        }

        private async Task<ClaimsIdentity> GenerateIdentityClaimsAsync(IdentityUser identityUser)
        {
            var claims = await _userManager.GetClaimsAsync(identityUser);

            var userRoles = await _userManager.GetRolesAsync(identityUser);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, identityUser.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnixEpochDate().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(type: "role", userRole));
            }

            var identityClaims = new ClaimsIdentity();

            identityClaims.AddClaims(claims);

            return identityClaims;
        }
    }
}