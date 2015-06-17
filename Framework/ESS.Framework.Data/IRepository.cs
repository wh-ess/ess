#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace ESS.Framework.Data
{
    public interface IRepository<TEntity, in TKey> : IDisposable where TEntity : class
    {
        Task<bool> Add(TKey id, TEntity entity);
        Task<bool> Update(TKey id, TEntity entity);

        Task<bool> Delete(TKey id);
        Task<bool> Delete(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DeleteAll();

        Task<TEntity> Get(TKey id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAllPaged(int page, int pageSize);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> First(Expression<Func<TEntity, bool>> predicate);
        Task<int> Count();
        Task<int> Count(Expression<Func<TEntity, bool>> criteria);
    }
}