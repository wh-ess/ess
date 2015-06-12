#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

#endregion

namespace ESS.Framework.Data.Dapper
{
    public class DapperRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private IDbConnection _conn;
        public DapperRepository(string connString)
        {
            _conn  = new SqlConnection(connString);
        }
        public bool Add(TKey id, TEntity entity)
        {
            return _conn.Insert(entity)>0;
        }

        public bool Update(TKey id, TEntity entity)
        {
            return _conn.Update(entity);
        }

        public bool Delete(TKey id)
        {
            return false;
        }

        public bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return false;
        }

        public bool DeleteAll()
        {
            return _conn.DeleteAll<TEntity>();
        }
        public TEntity Get(TKey id)
        {
            return _conn.Get<TEntity>(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAllPaged(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return 0;
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _conn.Close();
            _conn.Dispose();
            _conn = null;
        }
        #endregion
    }
}