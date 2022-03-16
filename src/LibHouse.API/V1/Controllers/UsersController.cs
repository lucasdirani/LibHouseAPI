using AutoMapper;
using KissLog;
using LibHouse.API.BaseControllers;
using LibHouse.API.Extensions.ModelState;
using LibHouse.API.V1.ViewModels.Users;
using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Business.Notifiers;
using LibHouse.Infrastructure.Authentication.Register;
using LibHouse.Infrastructure.Authentication.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibHouse.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : MainController
    {
        private readonly IUserSignUp _userSignUp;
        private readonly IUserRegistrationService _userRegistrationService;

        public UsersController(
            INotifier notifier, 
            ILoggedUser loggedUser, 
            IKLogger logger,
            IMapper mapper,
            IUserSignUp userSignUp,
            IUserRegistrationService userRegistrationService) 
            : base(notifier, loggedUser, logger, mapper)
        {
            _userSignUp = userSignUp;
            _userRegistrationService = userRegistrationService;
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
    }
}