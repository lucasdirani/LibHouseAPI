using LibHouse.Business.Monads;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register
{
    public class IdentityUserSignIn : IUserSignIn
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityUserSignIn(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<Result> SignInUserAsync(string userEmail, string userPassword)
        {
            SignInResult loginResult = await _signInManager.PasswordSignInAsync(userEmail, userPassword, isPersistent: true, lockoutOnFailure: true);

            if (loginResult.Succeeded)
            {
                return Result.Success();
            }

            if (loginResult.IsLockedOut)
            {
                return Result.Fail("O usuário está bloqueado.");
            }

            return Result.Fail("As credenciais são inválidas");
        }
    }
}