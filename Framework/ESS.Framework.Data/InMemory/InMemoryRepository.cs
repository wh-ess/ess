﻿#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ESS.Framework.Common.Extensions;

#endregion

namespace ESS.Framework.Data.InMemory
{
    public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private ConcurrentDictionary<TKey, TEntity> _store = new ConcurrentDictionary<TKey, TEntity>();

        public void Add(TKey id, TEntity entity)
        {
            _store.AddOrUpdate(id, entity, (k, v) => entity);
        }

        public void Update(TKey id, TEntity entity)
        {
            _store.AddOrUpdate(id, entity, (k, v) => entity);
        }

        public void Delete(TKey id)
        {
            _store.Remove(id);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
        }

        public TEntity Get(TKey id)
        {
            TEntity result = null;
            _store.TryGetValue(id, out result);
            return result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _store.Values;
        }

        public IEnumerable<TEntity> GetAllPaged(int page, int pageSize)
        {
            return _store.Values.Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _store.Values.Where(predicate.Compile());
        }

        public IEnumerable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate)
                .Single();
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate)
                .First();
        }

        public int Count()
        {
            return _store.Count;
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return Find(criteria)
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