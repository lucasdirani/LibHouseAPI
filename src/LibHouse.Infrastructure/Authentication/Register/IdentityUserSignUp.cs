using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Extensions.Identity;
using LibHouse.Infrastructure.Authentication.Extensions.Users;
using LibHouse.Infrastructure.Authentication.Register.Messages;
using LibHouse.Infrastructure.Authentication.Roles;
using LibHouse.Infrastructure.Authentication.Token.Models;
using LibHouse.Infrastructure.Email;
using LibHouse.Infrastructure.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register
{
    public class IdentityUserSignUp : IUserSignUp
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LibHouseWebsiteSettings _libHouseWebsiteSettings;
        private readonly IMailService _mailService;

        public IdentityUserSignUp(
            UserManager<IdentityUser> userManager,
            IOptions<LibHouseWebsiteSettings> libHouseWebsiteSettings,
            IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
            _libHouseWebsiteSettings = libHouseWebsiteSettings.Value;
        }

        public async Task<Result> AcceptUserConfirmationTokenAsync(
            SignUpConfirmationToken confirmationToken,
            string userEmail)
        {
            Maybe<IdentityUser> findUserByEmail = await _userManager.FindByEmailAsync(userEmail);

            if (findUserByEmail.HasNoValue)
            {
                return Result.Fail("O endereço de e-mail do usuário não foi localizado.");
            }

            IdentityUser user = findUserByEmail.Value;

            IdentityResult userEmailConfirmed = await _userManager.ConfirmEmailAsync(user, confirmationToken.Value);

            return userEmailConfirmed.Succeeded 
                ? Result.Success() 
                : Result.Fail(userEmailConfirmed.Errors.GetFirstErrorDescription());
        }

        public async Task<Result> SendConfirmationTokenToUserAsync(
            SignUpConfirmationToken confirmationToken, 
            User user)
        {
            string confirmEmailAddress = _libHouseWebsiteSettings.ConfirmEmailAddress;

            string confirmationTokenMessage = UserRegistrationMessageBuilder.BuildMessageForSendConfirmationTokenToUser(user, confirmationToken, confirmEmailAddress);

            var mailRequest = new MailRequest(toEmail: user.Email, subject: "Confirme o seu e-mail", body: confirmationTokenMessage);

            return await _mailService.SendEmailAsync(mailRequest);
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