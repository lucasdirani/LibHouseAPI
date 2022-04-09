using LibHouse.Business.Monads;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Authentication.Register.SignIn
{
    public interface IUserSignIn
    {
        Task<Result> SignInUserAsync(string userEmail, string userPassword);
    }
}