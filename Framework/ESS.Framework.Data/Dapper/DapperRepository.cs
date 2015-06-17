#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        public async Task<bool> Add(TKey id, TEntity entity)
        {
            return await _conn.InsertAsync(entity)>0;
        }

        public async Task<bool> Update(TKey id, TEntity entity)
        {
            return await _conn.UpdateAsync(entity);
        }

        public async Task<bool> Delete(TKey id)
        {
            return false;
        }

        public async Task<bool> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return false;
        }

        public async Task<bool> DeleteAll()
        {
            return await _conn.DeleteAllAsync<TEntity>();
        }
        public async Task<TEntity> Get(TKey id)
        {
            return await _conn.GetAsync<TEntity>(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllPaged(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> First(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Count()
        {
            return 0;
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> criteria)
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