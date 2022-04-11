using AutoMapper;
using KissLog;
using LibHouse.API.Attributes.Authorization;
using LibHouse.API.BaseControllers;
using LibHouse.API.Extensions.ModelState;
using LibHouse.API.V1.ViewModels.Users;
using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Business.Notifiers;
using LibHouse.Infrastructure.Authentication.Context;
using LibHouse.Infrastructure.Authentication.Login.Interfaces;
using LibHouse.Infrastructure.Authentication.Register.SignIn;
using LibHouse.Infrastructure.Authentication.Register.SignUp;
using LibHouse.Infrastructure.Authentication.Token.Generators;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Authentication.Token.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibHouse.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : MainController
    {
        private readonly IUserSignIn _userSignIn;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        private readonly IUserSignUp _userSignUp;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly AuthenticationContext _authenticationContext;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public UsersController(
            INotifier notifier, 
            ILoggedUser loggedUser, 
            IKLogger logger,
            IMapper mapper,
            ITokenGenerator tokenGenerator,
            IRefreshTokenValidator refreshTokenValidator,
            IUserSignIn userSignIn,
            IUserSignUp userSignUp,
            IUserRegistrationService userRegistrationService,
            AuthenticationContext authenticationContext,
            TokenValidationParameters tokenValidationParameters) 
            : base(notifier, loggedUser, logger, mapper)
        {
            _tokenGenerator = tokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
            _userSignIn = userSignIn;
            _userSignUp = userSignUp;
            _userRegistrationService = userRegistrationService;
            _authenticationContext = authenticationContext;
            _tokenValidationParameters = tokenValidationParameters;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Cadastra um novo usuário na plataforma, enviando um e-mail automático de confirmação.
        /// </summary>
        /// <param name="registerUser">Objeto que possui os dados necessários para cadastrar o usuário.</param>
        /// <returns>Em caso de sucesso, retorna um objeto vazio. Em caso de erro, retorna uma lista de notificações.</returns>
        /// <response code="200">O usuário foi registrado com sucesso.</response>
        /// <response code="400">Os dados enviados são inválidos, o usuário já está cadastrado ou houve uma falha para registrá-lo.</response>
        /// <response code="500">Erro ao processar a requisição no servidor.</response>
        [AllowAnonymous]
        [HttpPost("new-account", Name = "New Account")]
        public async Task<ActionResult> RegisterUserAsync(RegisterUserViewModel registerUser)
        {
            if (ModelState.NotValid())
            {
                return CustomResponseFor(ModelState);
            }

            User user = Mapper.Map<User>(registerUser);

            Result userRegistration = await _userRegistrationService.RegisterUserAsync(user);

            if (userRegistration.Failure)
            {
                return CustomResponseForPostEndpoint();
            }

            Result<SignUpConfirmationToken> userSignUp = await _userSignUp.SignUpUserAsync(user, registerUser.Password);

            if (userSignUp.Failure)
            {
                NotifyError("Registrar usuário", userSignUp.Error);

                return CustomResponseForPostEndpoint();
            }

            SignUpConfirmationToken confirmationToken = userSignUp.Value;

            Result sendConfirmationToken = await _userSignUp.SendConfirmationTokenToUserAsync(confirmationToken, user);

            if (sendConfirmationToken.Failure)
            {
                NotifyError("Enviar token de confirmação", sendConfirmationToken.Error);

                return CustomResponseForPostEndpoint();
            }

            return Ok();
        }

        /// <summary>
        /// Confirma o cadastro de um novo usuário na plataforma.
        /// </summary>
        /// <param name="confirmUserRegistration">Objeto que possui os dados necessários para confirmar o cadastro do usuário.</param>
        /// <returns>Em caso de sucesso, retorna um objeto vazio. Em caso de erro, retorna uma lista de notificações.</returns>
        /// <response code="204">O cadastro do usuário foi confirmado com sucesso.</response>
        /// <response code="400">Os dados enviados são inválidos ou o token de confirmação expirou.</response>
        /// <response code="500">Erro ao processar a requisição no servidor.</response>
        [AllowAnonymous]
        [HttpPatch("confirm-registration", Name = "Confirm User Registration")]
        public async Task<ActionResult> ConfirmUserRegistrationAsync(
            [FromBody] ConfirmUserRegistrationViewModel confirmUserRegistration)
        {
            if (ModelState.NotValid())
            {
                return CustomResponseFor(ModelState);
            }

            Result userConfirmed = await _userRegistrationService.ConfirmUserRegistrationAsync(confirmUserRegistration.UserId);

            if (userConfirmed.Failure)
            {
                return CustomResponseForPatchEndpoint(userConfirmed);
            }

            SignUpConfirmationToken confirmationToken = new(confirmUserRegistration.ConfirmationToken, isEncoded: false);

            Result userConfirmationAccepted = await _userSignUp.AcceptUserConfirmationTokenAsync(confirmationToken, confirmUserRegistration.UserEmail);

            if (userConfirmationAccepted.Failure)
            {
                NotifyError("Aceitar confirmação do usuário", userConfirmationAccepted.Error);

                return CustomResponseForPatchEndpoint(userConfirmationAccepted);
            }

            return CustomResponseForPatchEndpoint(userConfirmationAccepted);
        }

        /// <summary>
        /// Realiza o login de um usuário na plataforma, gerando um token de acesso.
        /// </summary>
        /// <param name="loginUser">Objeto que possui os dados necessários para confirmar o login do usuário.</param>
        /// <returns>Em caso de sucesso, retorna um objeto com os dados do usuário e do token. Em caso de erro, retorna uma lista de notificações.</returns>
        /// <response code="200">O login do usuário foi confirmado com sucesso.</response>
        /// <response code="400">Os dados enviados são inválidos ou houve uma falha na autenticação do usuário.</response>
        /// <response code="500">Erro ao processar a requisição no servidor.</response>
        [AllowAnonymous]
        [HttpPost("login", Name = "User Login")]
        public async Task<ActionResult<UserToken>> LoginUserAsync(LoginUserViewModel loginUser)
        {
            if (ModelState.NotValid())
            {
                return CustomResponseFor(ModelState);
            }

            Result userSignIn = await _userSignIn.SignInUserAsync(loginUser.Email, loginUser.Password);

            if (userSignIn.Failure)
            {
                NotifyError("Falha na autenticação do usuário", userSignIn.Error);

                Logger.Log(LogLevel.Warning, userSignIn.Error);

                return CustomResponseForPostEndpoint();
            }

            UserToken userToken = await _tokenGenerator.GenerateUserTokenAsync(loginUser.Email);

            Logger.Log(LogLevel.Information, $"Usuário {loginUser.Email} realizou login com sucesso: {DateTime.UtcNow}");

            return Ok(userToken);
        }

        /// <summary>
        /// Renova um access token expirado a partir de um refresh token.
        /// </summary>
        /// <param name="userRefreshToken">Objeto que possui os dados necessários para renovar o access token.</param>
        /// <returns>Em caso de sucesso, retorna um objeto com os dados do usuário e do novo token. Em caso de erro, retorna uma lista de notificações.</returns>
        /// <response code="200">O token do usuário foi renovado com sucesso.</response>
        /// <response code="400">Os dados enviados são inválidos ou houve uma falha na validação do refresh token.</response>
        /// <response code="500">Erro ao processar a requisição no servidor.</response>
        [Authorize("User")]
        [HttpPost("refresh-token", Name = "Refresh Token")]
        public async Task<ActionResult<UserToken>> RefreshTokenAsync(UserRefreshTokenViewModel userRefreshToken)
        {
            if (ModelState.NotValid())
            {
                return CustomResponseFor(ModelState);
            }

            RefreshToken refreshToken = await _authenticationContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == userRefreshToken.RefreshToken);

            if (refreshToken is null)
            {
                NotifyError("Refresh Token não encontrado", $"O refresh token {userRefreshToken.RefreshToken} não foi localizado");

                return CustomResponseForPostEndpoint();
            }

            ClaimsPrincipal accessTokenClaims = _jwtSecurityTokenHandler.ValidateToken(userRefreshToken.AccessToken, _tokenValidationParameters, out var accessToken);

            Result refreshTokenCanBeUsed = _refreshTokenValidator.CheckIfRefreshTokenCanBeUsedWithAccessToken(refreshToken, accessToken, accessTokenClaims);

            if (refreshTokenCanBeUsed.Failure)
            {
                Logger.Log(LogLevel.Error, refreshTokenCanBeUsed.Error);

                NotifyError("Refresh Token não pode ser utilizado", refreshTokenCanBeUsed.Error);

                return CustomResponseForPostEndpoint();
            }

            refreshToken.MarkAsUsed();

            await _authenticationContext.SaveChangesAsync();

            string userEmail = LoggedUser.GetUserEmail();

            UserToken userToken = await _tokenGenerator.GenerateUserTokenAsync(userEmail);

            Logger.Log(LogLevel.Information, $"Usuário {userEmail} renovou o seu login com sucesso: {DateTime.UtcNow}");

            return Ok(userToken);
        }
    }
}