#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ESS.Framework.Common.Extensions;

#endregion

namespace ESS.Framework.Data.InMemory
{
    public class InMemoryRepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey> where TEntity : class
    {
        private ConcurrentDictionary<TKey, TEntity> _store = new ConcurrentDictionary<TKey, TEntity>();

        public async Task<bool> AddAsync(TKey id, TEntity entity)
        {
            return await Task.Run(() =>
            {
                _store.AddOrUpdate(id, entity, (k, v) => entity);
                return true;
            });
        }

        public async Task<bool> UpdateAsync(TKey id, TEntity entity)
        {
            return await Task.Run(() =>
            {
                _store.AddOrUpdate(id, entity, (k, v) => entity);
                return true;
            });
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            return await Task.Run(() =>
               {
                   _store.Remove(id);
                   return true;
               });
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() =>
               {
                   return false;
               });
        }

        public async Task<bool> DeleteAllAsync()
        {
            return await Task.Run(() =>
               {
                   _store.Clear();
                   return true;
               });
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await Task.Run(() =>
               {
                   TEntity result;
                   _store.TryGetValue(id, out result);
                   return result;
               });
        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            
            return await Task.Run(() => _store.Values.AsQueryable());
        }

        public async Task<IQueryable<TEntity>> GetAllPagedAsync(int page, int pageSize)
        {
            if (!_store.Any())
            {
                return null;
            }
            return await Task.Run(() => _store.Values.AsQueryable().Skip((page - 1) * pageSize)
                .Take(pageSize));
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (!_store.Any())
            {
                return _store.Values.AsQueryable();
            }
            return await Task.Run(() => _store.Values.AsQueryable().Where(predicate));
        }

        public async Task<IQueryable<TEntity>> FindPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return (await FindAsync(predicate))
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await FindAsync(predicate))
                .SingleOrDefault();

        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await FindAsync(predicate))
                .FirstOrDefault();

        }

        public async Task<int> CountAsync()
        {
            return await Task.Run(() => _store.Count);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return (await FindAsync(criteria))
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