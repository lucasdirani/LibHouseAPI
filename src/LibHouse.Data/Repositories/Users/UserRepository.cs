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

        public async Task<bool> CheckIfExistingAccountAsync(Cpf cpf)
        {
            bool accountExist = await _dbSet.AnyAsync(u => u.CPF.Value == cpf.Value);
            bool accountActive = await _dbSet.AnyAsync(u => u.Active == true);

            if (accountActive && accountExist) return true;

            return false;
        }

        public async Task<bool> CheckIfUserCpfIsNotRegisteredAsync(Cpf cpf)
        {
            return !await _dbSet.AnyAsync(u => u.CPF.Value == cpf.Value);
        }

        public async Task<bool> CheckIfUserEmailIsNotRegisteredAsync(string email)
        {
            return !await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}