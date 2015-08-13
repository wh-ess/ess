#region

using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace ESS.Framework.Data.EF
{
    public class EfRepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public EfRepositoryAsync(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<bool> AddAsync(TKey id, TEntity entity)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync() == 1;
        }

        public async Task<bool> UpdateAsync(TKey id, TEntity entity)
        {
            _dbSet.Attach(entity);
            return await _dbContext.SaveChangesAsync()==1;
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            return false;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = FindAsync(predicate).Result;
            _dbSet.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() == entities.Count();

        }

        public async Task<bool> DeleteAllAsync()
        {
            var count = _dbSet.Count();
            _dbSet.RemoveRange(_dbSet);
            return await _dbContext.SaveChangesAsync() == count;
        }
        public async Task<TEntity> GetAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return _dbSet;
        }

        public async Task<IQueryable<TEntity>> GetAllPagedAsync(int page, int pageSize)
        {
            return _dbSet.Skip((page - 1)*pageSize).Take(pageSize);
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<IQueryable<TEntity>> FindPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return (await FindAsync(predicate)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
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
            _dbContext.Dispose();
        }
        #endregion
    }
}