using LibHouse.Data.Transactions;
using LibHouse.Infrastructure.Authentication.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Token
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenSettings _tokenSettings;

        public JwtTokenGenerator(
            UserManager<IdentityUser> userManager,
            IUnitOfWork unitOfWork, 
            IOptions<TokenSettings> tokenSettings)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<UserToken> GenerateUserTokenAsync(string userEmail)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(userEmail);

            ClaimsIdentity identityClaims = await GenerateIdentityClaimsAsync(user);

            byte[] key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            var tokenHandler = new JwtSecurityTokenHandler();

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            SecurityToken token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.ValidIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_tokenSettings.ExpiresInHours),
                SigningCredentials = signingCredentials,
            });

            return new UserToken(
                user: await _unitOfWork.UserRepository.GetUserByEmailAsync(userEmail),
                accessToken: tokenHandler.WriteToken(token),
                expiresIn: TimeSpan.FromHours(_tokenSettings.ExpiresInHours).TotalSeconds,
                claims: identityClaims.Claims.Select(c => new UserClaim(c.Type, c.Value))
            );
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