#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace ESS.Framework.Data
{
    public interface IRepository<TEntity, in TKey> : IDisposable where TEntity : class
    {
        bool Add(TKey id, TEntity entity);
        bool Update(TKey id, TEntity entity);

        bool Delete(TKey id);
        bool Delete(Expression<Func<TEntity, bool>> predicate);
        bool DeleteAll();

        TEntity Get(TKey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllPaged(int page, int pageSize);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        int Count();
        int Count(Expression<Func<TEntity, bool>> criteria);
    }
}