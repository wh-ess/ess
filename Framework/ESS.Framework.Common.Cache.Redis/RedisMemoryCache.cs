#region

using System;
using System.IO;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Serializing;
using StackExchange.Redis;

#endregion

namespace ESS.Framework.Common.Cache.Redis
{
    /// <summary>
    ///     Redis based memory cache implementation.
    /// </summary>
    public class RedisMemoryCache : IMemoryCache
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly IDatabase _redis;

        /// <summary>
        ///     Parameterized constructor.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public RedisMemoryCache(string host, int port)
        {
            _redis = ConnectionMultiplexer.Connect(host+":"+port).GetDatabase();
            _binarySerializer = ObjectContainer.Resolve<IBinarySerializer>();
        }

        /// <summary>
        ///     Get from memory cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Get(string key, Type type)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            byte[] data;
            try
            {
                data = _redis.StringGet(key);
            }
            catch (Exception ex)
            {
                throw new IOException($"Get data from redis server has exception, type:{type}, Id:{key}", ex);
            }
            return data.Length > 0 ? _binarySerializer.Deserialize(data, type) : null;
        }

        /// <summary>
        ///     Get a strong type from memory cache.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            byte[] value = _redis.StringGet(key);
            return value.Length > 0 ? _binarySerializer.Deserialize<T>(value) : null;
        }

        /// <summary>
        ///     Set to memory cache.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Set(object obj, DateTimeOffset absoluteExpiration = default(DateTimeOffset))
        {
            Set(obj.GetType().FullName, obj, absoluteExpiration);
        }

        public void Set(string key,object obj, DateTimeOffset absoluteExpiration = default(DateTimeOffset))
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (absoluteExpiration == default(DateTimeOffset))
            {
                absoluteExpiration = DateTime.Now.AddHours(2);
            }
            _redis.StringSet( key, _binarySerializer.Serialize(obj));
            _redis.KeyExpire(key, absoluteExpiration.DateTime);
        }
    }
}