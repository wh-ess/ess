using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Serializing;
using StackExchange.Redis;

namespace ESS.Framework.Data.Redis
{

    public class RedisRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDatabase _redis;
        private readonly string _key = typeof(TEntity).FullName;
        public RedisRepository()
        {
            _jsonSerializer = ObjectContainer.Resolve<IJsonSerializer>();
            _redis = ObjectContainer.Resolve<IDatabase>();
        }


        public async Task<bool> Add(TKey id, TEntity entity)
        {
            return await _redis.HashSetAsync(_key,id.ToString(), _jsonSerializer.Serialize(entity));
        }

        public async Task<bool> Update(TKey id, TEntity entity)
        {
            return await _redis.HashSetAsync(_key, id.ToString(), _jsonSerializer.Serialize(entity));
        }

        public async Task<bool> Delete(TKey id)
        {
            return await _redis.HashDeleteAsync(_key, id.ToString());
        }

        public async Task<bool> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAll()
        {
            return await _redis.KeyDeleteAsync(_key);
        }
        
        public async Task<TEntity> Get(TKey id)
        {
            return _jsonSerializer.Deserialize<TEntity>(await _redis.HashGetAsync(_key, id.ToString()));
        }
        
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            RedisValue[] result = await _redis.HashValuesAsync(_key);
            return result.Select(r => _jsonSerializer.Deserialize<TEntity>(r));
        }

        public async Task<IEnumerable<TEntity>> GetAllPaged(int page, int pageSize)
        {
            return (await GetAll())
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return (await GetAll()).Where(predicate.Compile());
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
            return (int) await _redis.HashLengthAsync(_key);
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> criteria)
        {
            return (await Find(criteria)).Count();
        }


        public void Dispose()
        {

        }
    }
}
