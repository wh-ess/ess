using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Serializing;
using StackExchange.Redis;

namespace ESS.Framework.Data.Redis
{

    public class RedisRepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey>
        where TEntity : class
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDatabase _redis;
        private readonly string _key = typeof(TEntity).FullName;
        public RedisRepositoryAsync()
        {
            _jsonSerializer = ObjectContainer.Resolve<IJsonSerializer>();
            _redis = ObjectContainer.Resolve<IDatabase>();
        }


        public async Task<bool> AddAsync(TKey id, TEntity entity)
        {
            return await _redis.HashSetAsync(_key, id.ToString(), _jsonSerializer.Serialize(entity));
        }

        public async Task<bool> UpdateAsync(TKey id, TEntity entity)
        {
            return await _redis.HashSetAsync(_key, id.ToString(), _jsonSerializer.Serialize(entity));
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            return await _redis.HashDeleteAsync(_key, id.ToString());
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAllAsync()
        {
            return await _redis.KeyDeleteAsync(_key);
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return _jsonSerializer.Deserialize<TEntity>(await _redis.HashGetAsync(_key, id.ToString()));
        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            RedisValue[] result = { };
            if (_redis.KeyExists(_key))
            {
                result = await _redis.HashValuesAsync(_key);
            }
            return result.Select(r => _jsonSerializer.Deserialize<TEntity>(r)).AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetAllPagedAsync(int page, int pageSize)
        {
            return (await GetAllAsync())
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await GetAllAsync()).Where(predicate);
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
            return (int)await _redis.HashLengthAsync(_key);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return (await FindAsync(criteria)).Count();
        }


        public void Dispose()
        {

        }
    }
}
