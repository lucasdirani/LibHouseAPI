using LibHouse.Business.Monads;
using System.Threading.Tasks;

namespace LibHouse.Business.Entities.Users
{
    public interface IUserRegistrationService
    {
        Task<Result> RegisterUserAsync(User user);
    }
}