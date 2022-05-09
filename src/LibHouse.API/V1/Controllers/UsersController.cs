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
using LibHouse.Infrastructure.Authentication.Login.Models;
using LibHouse.Infrastructure.Authentication.Login.Password;
using LibHouse.Infrastructure.Authentication.Register.SignIn;
using LibHouse.Infrastructure.Authentication.Register.SignUp;
using LibHouse.Infrastructure.Authentication.Token.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LibHouse.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : MainController
    {
        private readonly IUserSignIn _userSignIn;
        private readonly IUserSignUp _userSignUp;
        private readonly IUserPasswordReset _userPasswordReset;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly AuthenticationContext _authenticationContext;

        public UsersController(
            INotifier notifier, 
            ILoggedUser loggedUser, 
            IKLogger logger,
            IMapper mapper,
            IUserSignIn userSignIn,
            IUserSignUp userSignUp,
            IUserPasswordReset userPasswordReset,
            IUserRegistrationService userRegistrationService,
            AuthenticationContext authenticationContext) 
            : base(notifier, loggedUser, logger, mapper)
        {
            _userSignIn = userSignIn;
            _userSignUp = userSignUp;
            _userPasswordReset = userPasswordReset;
            _userRegistrationService = userRegistrationService;
            _authenticationContext = authenticationContext;
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
                Logger.Log(LogLevel.Error, $"Erro ao registrar o usuário: {userRegistration.Error}");

                return CustomResponseForPostEndpoint();
            }

            Result<SignUpConfirmationToken> userSignUp = await _userSignUp.SignUpUserAsync(user, registerUser.Password);

            if (userSignUp.Failure)
            {
                NotifyError("Registrar usuário", userSignUp.Error);

                Logger.Log(LogLevel.Error, $"Erro ao registrar o usuário: {userSignUp.Error}");

                return CustomResponseForPostEndpoint();
            }

            SignUpConfirmationToken confirmationToken = userSignUp.Value;

            Result sendConfirmationToken = await _userSignUp.SendConfirmationTokenToUserAsync(confirmationToken, user);

            if (sendConfirmationToken.Failure)
            {
                NotifyError("Enviar token de confirmação", sendConfirmationToken.Error);

                Logger.Log(LogLevel.Error, $"Erro ao enviar o token de confirmação de cadastro do usuário: {sendConfirmationToken.Error}");

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
                NotifyError("Confirmação do cadastro do usuário", userConfirmed.Error);

                Logger.Log(LogLevel.Error, $"Erro ao confirmar o cadastro do usuário: {userConfirmed.Error}");

                return CustomResponseForPatchEndpoint(userConfirmed);
            }

            SignUpConfirmationToken confirmationToken = new(confirmUserRegistration.ConfirmationToken, isEncoded: false);

            Result userConfirmationAccepted = await _userSignUp.AcceptUserConfirmationTokenAsync(confirmationToken, confirmUserRegistration.UserEmail);

            if (userConfirmationAccepted.Failure)
            {
                NotifyError("Aceitar confirmação do usuário", userConfirmationAccepted.Error);

                Logger.Log(LogLevel.Error, $"Erro ao confirmar o cadastro do usuário: {userConfirmationAccepted.Error}");

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
        public async Task<ActionResult<UserTokenViewModel>> LoginUserAsync(LoginUserViewModel loginUser)
        {
            if (ModelState.NotValid())
            {
                return CustomResponseFor(ModelState);
            }

            Result<AuthenticatedUser> userSignIn = await _userSignIn.SignInUserAsync(loginUser.Email, loginUser.Password);

            if (userSignIn.Failure)
            {
                NotifyError("Falha na autenticação do usuário", userSignIn.Error);

                Logger.Log(LogLevel.Warning, userSignIn.Error);

                return CustomResponseForPostEndpoint();
            }

            UserTokenViewModel userToken = Mapper.Map<UserTokenViewModel>(userSignIn.Value);

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
        public async Task<ActionResult<UserTokenViewModel>> RefreshTokenAsync(
            UserRefreshTokenViewModel userRefreshToken)
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

            AccessToken accessToken = new(userRefreshToken.AccessToken);

            string userEmail = LoggedUser.GetUserEmail();

            Result<AuthenticatedUser> userSignIn = await _userSignIn.SignInUserAsync(userEmail, accessToken, refreshToken);

            if (userSignIn.Failure)
            {
                Logger.Log(LogLevel.Error, userSignIn.Error);

                NotifyError("Refresh Token não pode ser utilizado", userSignIn.Error);

                return CustomResponseForPostEndpoint();
            }

            refreshToken.MarkAsUsed();

            await _authenticationContext.SaveChangesAsync();

            Logger.Log(LogLevel.Information, $"Usuário {userEmail} renovou o seu login com sucesso: {DateTime.UtcNow}");

            UserTokenViewModel userToken = Mapper.Map<UserTokenViewModel>(userSignIn.Value);

            return Ok(userToken);
        }

        /// <summary>
        /// Solicita a redefinição de senha para um usuário.
        /// </summary>
        /// <param name="requestUserPasswordReset">Objeto que possui os dados necessários para solicitar a redefinição de senha do usuário.</param>
        /// <returns>Em caso de sucesso, retorna um objeto vazio. Em caso de erro, retorna uma lista de notificações.</returns>
        /// <response code="204">A solicitação de redefinição de senha foi confirmada com sucesso.</response>
        /// <response code="400">Os dados enviados são inválidos ou houve uma falha na geração do token de redefinição de senha.</response>
        /// <response code="500">Erro ao processar a requisição no servidor.</response>
        [AllowAnonymous]
        [HttpPatch("request-password-reset", Name = "Request Password Reset")]
        public async Task<IActionResult> RequestPasswordResetAsync(
            [FromBody] RequestUserPasswordResetViewModel requestUserPasswordReset)
        {
            if (ModelState.NotValid())
            {
                return CustomResponseFor(ModelState);
            }

            Cpf userCpf = Cpf.CreateFromDocument(requestUserPasswordReset.Cpf);

            Result<User> userAccountExistence = await _userRegistrationService.CheckUserAccountExistence(userCpf);

            if (userAccountExistence.Failure)
            {
                NotifyError("Conta do usuário", userAccountExistence.Error);

                return CustomResponseForPatchEndpoint(userAccountExistence);
            }

            User user = userAccountExistence.Value;

            Result<PasswordResetToken> requestPasswordReset = await _userPasswordReset.RequestPasswordResetAsync(user.Email);

            if (requestPasswordReset.Failure)
            {
                NotifyError("Solicitar redefinição de senha", requestPasswordReset.Error);

                Logger.Log(LogLevel.Error, $"Falha ao criar o token de redefinição de senha para {user.Email}");

                return CustomResponseForPatchEndpoint(requestPasswordReset);
            }

            PasswordResetToken passwordResetToken = requestPasswordReset.Value;

            Result sendPasswordResetToken = await _userPasswordReset.SendPasswordResetTokenToUserAsync(passwordResetToken, user);

            if (sendPasswordResetToken.Failure)
            {
                NotifyError("Enviar solicitação de redefinição de senha", sendPasswordResetToken.Error);

                Logger.Log(LogLevel.Error, $"Falha ao enviar o token de redefinição de senha para {user.Email}");

                return CustomResponseForPatchEndpoint(sendPasswordResetToken);
            }

            Logger.Log(LogLevel.Information, $"Redefinição de senha solicitada para {user.Email}");

            return CustomResponseForPatchEndpoint(sendPasswordResetToken);
        }
    }
}