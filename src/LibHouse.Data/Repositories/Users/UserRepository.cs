using LibHouse.Business.Entities.Users;
using LibHouse.Data.Context;
using LibHouse.Data.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibHouse.Data.Repositories.Users
{
    public class UserRepository : EntityTypeRepository<User>, IUserRepository
    {
        public UserRepository(LibHouseContext context) 
            : base(context)
        {

        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}