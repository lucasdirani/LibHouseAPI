using LibHouse.Business.Entities.Shared;
using LibHouse.Business.Projections.Users;
using System.Threading.Tasks;

namespace LibHouse.Business.Entities.Users
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<ConsolidatedUser> GetConsolidatedUserByEmailAsync(string email);
        Task<bool> CheckIfUserCpfIsNotRegisteredAsync(Cpf cpf);
        Task<bool> CheckIfUserEmailIsNotRegisteredAsync(string email);
    }
}