using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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


        public bool Add(TKey id, TEntity entity)
        {
            return _redis.HashSetAsync(_key,id.ToString(), _jsonSerializer.Serialize(entity)).Result;
        }

        public bool Update(TKey id, TEntity entity)
        {
            return _redis.HashSetAsync(_key, id.ToString(), _jsonSerializer.Serialize(entity)).Result;
        }

        public bool Delete(TKey id)
        {
            return _redis.HashDeleteAsync(_key, id.ToString()).Result;
        }

        public bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAll()
        {
            return _redis.KeyDelete(_key);
        }
        
        public TEntity Get(TKey id)
        {
            return _jsonSerializer.Deserialize<TEntity>(_redis.HashGetAsync(_key, id.ToString()).Result);
        }
        
        public IEnumerable<TEntity> GetAll()
        {
            RedisValue[] result = _redis.HashValuesAsync(_key).Result;
            return result.Select(r => _jsonSerializer.Deserialize<TEntity>(r));
        }

        public IEnumerable<TEntity> GetAllPaged(int page, int pageSize)
        {
            return GetAll()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate.Compile());
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
                .SingleOrDefault();
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate)
                .FirstOrDefault();
        }


        public int Count()
        {
            return (int)_redis.HashLengthAsync(_key).Result;
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return Find(criteria).Count();
        }


        public void Dispose()
        {

        }
    }
}
