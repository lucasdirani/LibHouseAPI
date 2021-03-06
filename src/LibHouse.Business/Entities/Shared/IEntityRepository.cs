using LibHouse.Business.Monads;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibHouse.Business.Entities.Shared
{
    public interface IEntityRepository<T> where T : Entity
    {
        Task AddAsync(T entity);
        void Remove(T entity);
        void Update(T entity);
        Task<Maybe<T>> GetByIdAsync(Guid id);
        Task<Maybe<T>> FirstAsync(Expression<Func<T, bool>> expression);
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAsync(
            Expression<Func<T, bool>> expression = null,
            int? skip = null,
            int? take = null);
    }
}