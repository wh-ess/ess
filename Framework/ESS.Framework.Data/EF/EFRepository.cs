#region

using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace ESS.Framework.Data.EF
{
    public class EfRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public  bool Add(TKey id, TEntity entity)
        {
            _dbSet.Add(entity);
            return  _dbContext.SaveChanges() == 1;
        }

        public  bool Update(TKey id, TEntity entity)
        {
            _dbSet.Attach(entity);
            return  _dbContext.SaveChanges()==1;
        }

        public  bool Delete(TKey id)
        {
            return false;
        }

        public  bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = Find(predicate);
            _dbSet.RemoveRange(entities);
            return  _dbContext.SaveChanges() == entities.Count();

        }

        public  bool DeleteAll()
        {
            var count = _dbSet.Count();
            _dbSet.RemoveRange(_dbSet);
            return  _dbContext.SaveChanges() == count;
        }
        public  TEntity Get(TKey id)
        {
            throw new NotImplementedException();
        }

        public  IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public  IQueryable<TEntity> GetAllPaged(int page, int pageSize)
        {
            return _dbSet.Skip((page - 1)*pageSize).Take(pageSize);
        }

        public  IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public  IQueryable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return ( Find(predicate)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public  TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return  _dbSet.SingleOrDefault(predicate);
        }

        public  TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return  _dbSet.FirstOrDefault(predicate);
        }

        public  int Count()
        {
            return  _dbSet.Count();
        }

        public  int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return  _dbSet.Count(predicate);
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