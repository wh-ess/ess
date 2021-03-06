﻿#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace ESS.Framework.Data.Dapper
{
    public class DapperRepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey> where TEntity : class
    {
        private IDbConnection _conn;
        public DapperRepositoryAsync(string connString)
        {
            _conn  = new SqlConnection(connString);
        }
        public async Task<bool> AddAsync(TKey id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(TKey id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            return false;
        }

        public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        

        public async Task<bool> DeleteAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<TEntity> GetAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<TEntity>> GetAllPagedAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<TEntity>> FindPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountAsync()
        {
            return 0;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria)
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