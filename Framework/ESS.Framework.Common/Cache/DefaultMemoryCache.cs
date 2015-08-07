using System;
using System.Runtime.Caching;

namespace ESS.Framework.Common.Cache
{
    /// <summary>
    ///     Default implementation of IMemoryCache which using ConcurrentDictionary.
    /// </summary>
    public class DefaultMemoryCache : IMemoryCache
    {
        private readonly MemoryCache _memoryCache = new MemoryCache("MemoryCache");

        /// <summary>
        ///     Get an from memory cache.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public object Get(string key, Type type)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return _memoryCache.Get(key);
        }

        /// <summary>
        ///     Get a strong type from memory cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return Get(key, typeof (T)) as T;
        }

        /// <summary>
        ///     Set to memory cache.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Set(object obj, DateTimeOffset absoluteExpiration = default(DateTimeOffset))
        {
            Set(obj.GetType().FullName, obj, absoluteExpiration);
        }

        public void Set(string key, object obj, DateTimeOffset absoluteExpiration = default(DateTimeOffset))
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            if (absoluteExpiration == default(DateTimeOffset))
            {
                var localTime = DateTime.Now;
                absoluteExpiration = new DateTimeOffset(localTime.Ticks,
                    new TimeSpan(2, 0, 0));
            }
            _memoryCache.Set(key, obj, absoluteExpiration);
        }
    }
}