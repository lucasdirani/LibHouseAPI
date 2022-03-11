using LibHouse.Business.Entities.Users;
using System;
using System.Threading.Tasks;

namespace LibHouse.Data.Transactions
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        Task<bool> CommitAsync();
        IUserRepository UserRepository { get; }
    }
}