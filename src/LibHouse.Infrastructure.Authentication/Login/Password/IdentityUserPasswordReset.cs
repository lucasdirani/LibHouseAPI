using Ardalis.GuardClauses;
using LibHouse.Business.Entities.Users;
using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Authentication.Extensions.Identity;
using LibHouse.Infrastructure.Authentication.Login.Password.Senders;
using LibHouse.Infrastructure.Authentication.Token.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Login.Password
{
    public class IdentityUserPasswordReset : IUserPasswordReset
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPasswordResetTokenSender _passwordResetTokenSender;

        public IdentityUserPasswordReset(
            UserManager<IdentityUser> userManager,
            IPasswordResetTokenSender passwordResetTokenSender)
        {
            _userManager = userManager;
            _passwordResetTokenSender = passwordResetTokenSender;
        }

        public async Task<Result> AcceptUserPasswordResetTokenAsync(
            PasswordResetToken passwordResetToken, 
            string userEmail, 
            string userNewPassword)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null)
            {
                return Result.Fail($"O usuário {userEmail} não foi encontrado");
            }

            IdentityResult passwordResetResult = await _userManager.ResetPasswordAsync(user, passwordResetToken.Value, userNewPassword);

            return passwordResetResult.Succeeded
                ? Result.Success()
                : Result.Fail(passwordResetResult.Errors.GetFirstErrorDescription());
        }

        public async Task<Result<PasswordResetToken>> RequestPasswordResetAsync(string userEmail)
        {
            Guard.Against.NullOrEmpty(userEmail, nameof(userEmail), "O valor do e-mail é obrigatório");

            IdentityUser user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null)
            {
                return Result.Fail<PasswordResetToken>($"O usuário {userEmail} não foi encontrado");
            }

            string passwordResetTokenValue = await _userManager.GeneratePasswordResetTokenAsync(user);

            PasswordResetToken passwordResetToken = new(passwordResetTokenValue);

            return Result.Success(passwordResetToken);
        }

        public async Task<Result> SendPasswordResetTokenToUserAsync(PasswordResetToken passwordResetToken, User user)
        {
            bool passwordResetTokenSent = await _passwordResetTokenSender.SendPasswordResetTokenToUserAsync(passwordResetToken, user);

            return passwordResetTokenSent ? Result.Success() : Result.Fail($"Falha ao enviar o token de redefinição de senha para {user.Email}");
        }
    }
}