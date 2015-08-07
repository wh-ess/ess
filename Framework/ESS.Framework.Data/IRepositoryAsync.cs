#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace ESS.Framework.Data
{
    public interface IRepositoryAsync<TEntity, in TKey> : IDisposable where TEntity : class
    {
        Task<bool> AddAsync(TKey id, TEntity entity);
        Task<bool> UpdateAsync(TKey id, TEntity entity);

        Task<bool> DeleteAsync(TKey id);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DeleteAllAsync();

        Task<TEntity> GetAsync(TKey id);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> GetAllPagedAsync(int page, int pageSize);
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> FindPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria);
    }
}