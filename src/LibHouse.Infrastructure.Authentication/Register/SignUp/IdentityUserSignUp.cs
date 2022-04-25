using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Extensions.Identity;
using LibHouse.Infrastructure.Authentication.Extensions.Users;
using LibHouse.Infrastructure.Authentication.Register.Senders;
using LibHouse.Infrastructure.Authentication.Roles;
using LibHouse.Infrastructure.Authentication.Token.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register.SignUp
{
    public class IdentityUserSignUp : IUserSignUp
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISignUpConfirmationTokenSender _signUpConfirmationTokenSender;

        public IdentityUserSignUp(
            UserManager<IdentityUser> userManager,
            ISignUpConfirmationTokenSender signUpConfirmationTokenSender)
        {
            _userManager = userManager;
            _signUpConfirmationTokenSender = signUpConfirmationTokenSender;
        }

        public async Task<Result> AcceptUserConfirmationTokenAsync(
            SignUpConfirmationToken confirmationToken,
            string userEmail)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(userEmail);
             
            if (user is null)
            {
                return Result.Fail("O endereço de e-mail do usuário não foi localizado.");
            }

            IdentityResult userEmailConfirmed = await _userManager.ConfirmEmailAsync(user, confirmationToken.Value);

            return userEmailConfirmed.Succeeded 
                ? Result.Success() 
                : Result.Fail(userEmailConfirmed.Errors.GetFirstErrorDescription());
        }

        public async Task<Result> SendConfirmationTokenToUserAsync(
            SignUpConfirmationToken confirmationToken, 
            User user)
        {
            bool confirmationTokenSent = await _signUpConfirmationTokenSender.SendSignUpConfirmationTokenToUserAsync(confirmationToken, user);

            return confirmationTokenSent ? Result.Success() : Result.Fail($"Falha ao enviar o token de confirmação para {user.Email}");
        }

        public async Task<Result<SignUpConfirmationToken>> SignUpUserAsync(User user, string password)
        {
            IdentityUser identityUser = user.AsNewIdentityUser();

            IdentityResult userCreation = await _userManager.CreateAsync(identityUser, password);

            if (!userCreation.Succeeded)
            {
                string userCreationError = userCreation.Errors.GetFirstErrorDescription();

                return Result.Fail<SignUpConfirmationToken>(userCreationError);
            }

            await _userManager.AddToRolesAsync(identityUser, new[] { LibHouseUserRole.User, user.GetUserTypeRole() });

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

            var signUpConfirmationToken = new SignUpConfirmationToken(token);

            return Result.Success(signUpConfirmationToken);
        }
    }
}