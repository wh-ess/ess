using System;

namespace ESS.Framework.Common.Cache
{
    /// <summary>Represents a high speed memory cache to get or set aggregate.
    /// </summary>
    public interface IMemoryCache
    {
        /// <summary>Get an aggregate from memory cache.
        /// </summary>
        /// <returns></returns>
        object Get(string key, Type type);
        /// <summary>Get a strong type aggregate from memory cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string key) where T : class;
        /// <summary>Set an aggregate to memory cache.
        /// </summary>
        void Set(object obj);

        void Set(string key,object obj);

    }
}
