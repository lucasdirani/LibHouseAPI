using LibHouse.Business.Monads;
using System;
using System.Threading.Tasks;

namespace LibHouse.Business.Entities.Users
{
    public interface IUserRegistrationService
    {
        Task<Result> RegisterUserAsync(User user);
        Task<Result> ConfirmUserRegistrationAsync(Guid userId);
        Task<Result> ConfirmationExistingAccountAsync(Cpf cpf);
    }
}