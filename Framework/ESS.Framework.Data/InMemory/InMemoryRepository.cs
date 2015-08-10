#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace ESS.Framework.Data.InMemory
{
    public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private Dictionary<TKey, TEntity> _store = new Dictionary<TKey, TEntity>();

        public bool Add(TKey id, TEntity entity)
        {
            _store.Add(id, entity);
            return true;
        }

        public bool Update(TKey id, TEntity entity)
        {
            if (_store.ContainsKey(id))
            {
                _store[id] = entity;
            }
            return true;
        }

        public bool Delete(TKey id)
        {
            return _store.Remove(id);
        }

        public bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return false;
        }

        public bool DeleteAll()
        {
            _store.Clear();
            return true;
        }

        public TEntity Get(TKey id)
        {
                   TEntity result;
                   _store.TryGetValue(id, out result);
                   return result;
        }

        public IQueryable<TEntity> GetAll()
        {

            return _store.Values.AsQueryable();
        }

        public IQueryable<TEntity> GetAllPaged(int page, int pageSize)
        {
            return _store.Values.AsQueryable().Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _store.Values.AsQueryable().Where(predicate);
        }

        public IQueryable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate)
                .SingleOrDefault();

        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate)
                .FirstOrDefault();

        }

        public int Count()
        {
            return _store.Count;
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return  Find(criteria)
                .Count();

        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            _store = null;
        }

        #endregion
    }
}