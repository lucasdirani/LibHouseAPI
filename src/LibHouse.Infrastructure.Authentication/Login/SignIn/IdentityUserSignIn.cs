using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Login.Models;
using LibHouse.Infrastructure.Authentication.Token.Generators.AccessTokens;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Authentication.Token.Validations.RefreshTokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register.SignIn
{
    public class IdentityUserSignIn : IUserSignIn
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        private readonly IUserRepository _userRepository;

        public IdentityUserSignIn(
            SignInManager<IdentityUser> signInManager,
            TokenValidationParameters tokenValidationParameters,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenValidator refreshTokenValidator,
            IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _tokenValidationParameters = tokenValidationParameters;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
            _userRepository = userRepository;
        }

        public async Task<Result<AuthenticatedUser>> SignInUserAsync(string userEmail, string userPassword)
        {
            SignInResult loginResult = await _signInManager.PasswordSignInAsync(userEmail, userPassword, isPersistent: true, lockoutOnFailure: true);

            if (loginResult.IsLockedOut)
            {
                return Result.Fail<AuthenticatedUser>("O usuário está bloqueado.");
            }

            if (!loginResult.Succeeded)
            {
                return Result.Fail<AuthenticatedUser>("As credenciais são inválidas");
            }

            return Result.Success(new AuthenticatedUser(
                profile: await GetAuthenticatedUserProfileAsync(userEmail),
                accessToken: await _accessTokenGenerator.GenerateAccessTokenAsync(userEmail)
            ));
        }

        public async Task<Result<AuthenticatedUser>> SignInUserAsync(
            string userEmail,
            AccessToken accessToken, 
            RefreshToken refreshToken)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            ClaimsPrincipal accessTokenClaims = jwtSecurityTokenHandler.ValidateToken(accessToken.Value, _tokenValidationParameters, out var securityToken);

            Result refreshTokenCanBeUsed = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, securityToken, accessTokenClaims);

            if (refreshTokenCanBeUsed.Failure)
            {
                return Result.Fail<AuthenticatedUser>(refreshTokenCanBeUsed.Error);
            }

            return Result.Success(new AuthenticatedUser(
                profile: await GetAuthenticatedUserProfileAsync(userEmail),
                accessToken: await _accessTokenGenerator.GenerateAccessTokenAsync(userEmail)
            ));
        }

        private async Task<AuthenticatedUserProfile> GetAuthenticatedUserProfileAsync(string userEmail)
        {
            return await _userRepository.GetProjectionAsync(
                u => u.Email == userEmail,
                u => new AuthenticatedUserProfile(u.Id, u.Name, u.LastName, u.BirthDate, u.Gender, u.Email, u.UserType)
            );
        }
    }
}