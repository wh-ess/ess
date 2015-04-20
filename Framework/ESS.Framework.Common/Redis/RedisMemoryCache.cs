#region

using System;
using System.IO;
using ESS.Framework.Common.Cache;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Serializing;
using StackExchange.Redis;

#endregion

namespace ESS.Framework.Common.Redis
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
        ///     Get an aggregate from memory cache.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Get(string id, Type type)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            byte[] data;
            try
            {
                data = _redis.StringGet(id);
            }
            catch (Exception ex)
            {
                throw new IOException(string.Format("Get data from redis server has exception, type:{0}, Id:{1}", type, id), ex);
            }
            return data.Length > 0 ? _binarySerializer.Deserialize(data, type) : null;
        }

        /// <summary>
        ///     Get a strong type aggregate from memory cache.
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string id) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            byte[] value = _redis.StringGet(id);
            return value.Length > 0 ? _binarySerializer.Deserialize<T>(value) : null;
        }

        /// <summary>
        ///     Set an aggregate to memory cache.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Set(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            _redis.StringSet( obj.GetType().FullName, _binarySerializer.Serialize(obj));
        }

        public void Set(string key,object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            _redis.StringSet( key, _binarySerializer.Serialize(obj));
        }
    }
}