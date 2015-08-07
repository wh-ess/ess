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
    public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private ConcurrentDictionary<TKey, TEntity> _store = new ConcurrentDictionary<TKey, TEntity>();

        public async Task<bool> Add(TKey id, TEntity entity)
        {
            return await Task.Run(() =>
            {
                _store.AddOrUpdate(id, entity, (k, v) => entity);
                return true;
            });
        }

        public async Task<bool> Update(TKey id, TEntity entity)
        {
            return await Task.Run(() =>
            {
                _store.AddOrUpdate(id, entity, (k, v) => entity);
                return true;
            });
        }

        public async Task<bool> Delete(TKey id)
        {
            return await Task.Run(() =>
               {
                   _store.Remove(id);
                   return true;
               });
        }

        public async Task<bool> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() =>
               {
                   return false;
               });
        }

        public async Task<bool> DeleteAll()
        {
            return await Task.Run(() =>
               {
                   _store.Clear();
                   return true;
               });
        }

        public async Task<TEntity> Get(TKey id)
        {
            return await Task.Run(() =>
               {
                   TEntity result;
                   _store.TryGetValue(id, out result);
                   return result;
               });
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            
            return await Task.Run(() => _store.Values);
        }

        public async Task<IEnumerable<TEntity>> GetAllPaged(int page, int pageSize)
        {
            if (!_store.Any())
            {
                return null;
            }
            return await Task.Run(() => _store.Values.Skip((page - 1) * pageSize)
                .Take(pageSize));
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            if (!_store.Any())
            {
                return _store.Values;
            }
            return await Task.Run(() => _store.Values.Where(predicate.Compile()));
        }

        public async Task<IEnumerable<TEntity>> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return (await Find(predicate))
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

        }

        public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate)
        {
            return (await Find(predicate))
                .SingleOrDefault();

        }

        public async Task<TEntity> First(Expression<Func<TEntity, bool>> predicate)
        {
            return (await Find(predicate))
                .FirstOrDefault();

        }

        public async Task<int> Count()
        {
            return await Task.Run(() => _store.Count);
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> criteria)
        {
            return (await Find(criteria))
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