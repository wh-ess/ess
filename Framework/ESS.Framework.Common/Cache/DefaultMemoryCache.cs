using System;
using System.Collections.Concurrent;
using ESS.Framework.Common.Serializing;

namespace ESS.Framework.Common.Cache
{
    /// <summary>Default implementation of IMemoryCache which using ConcurrentDictionary.
    /// </summary>
    public class DefaultMemoryCache : IMemoryCache
    {
        private readonly ConcurrentDictionary<string, byte[]> _cacheDict = new ConcurrentDictionary<string, byte[]>();
        private readonly IBinarySerializer _binarySerializer;

        /// <summary>Parameterized constructor.
        /// </summary>
        /// <param name="binarySerializer"></param>
        public DefaultMemoryCache(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        /// <summary>Get an aggregate from memory cache.
        /// </summary>
        /// <returns></returns>
        public object Get(string key, Type type)
        {
            if (key == null) throw new ArgumentNullException("key");
            byte[] value;
            if (_cacheDict.TryGetValue(key, out value))
            {
                return _binarySerializer.Deserialize(value, type);
            }
            return null;
        }
        
        /// <summary>Get a strong type aggregate from memory cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return Get(key, typeof(T)) as T;
        }
        /// <summary>Set an aggregate to memory cache.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Set(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            _cacheDict[obj.GetType().FullName] = _binarySerializer.Serialize(obj);
        }
        public void Set(string key,object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            _cacheDict[key] = _binarySerializer.Serialize(obj);
        }
    }
}
