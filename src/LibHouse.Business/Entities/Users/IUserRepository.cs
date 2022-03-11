using LibHouse.Business.Entities.Shared;
using System.Threading.Tasks;

namespace LibHouse.Business.Entities.Users
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}