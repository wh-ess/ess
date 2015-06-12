using ESS.Framework.Common.Cache;
using ESS.Framework.Common.Configurations;

namespace ESS.Framework.Common.Redis
{
    /// <summary>configuration class Redis extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>Use Redis to implement the memory cache.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseRedisMemoryCache(this Configuration configuration)
        {
            return UseRedisMemoryCache(configuration, "127.0.0.1", 6379);
        }
        /// <summary>Use Redis to implement the memory cache.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseRedisMemoryCache(this Configuration configuration, string host, int port)
        {
            configuration.SetDefault<IMemoryCache, RedisMemoryCache>(new RedisMemoryCache(host, port));
            return configuration;
        }
    }
}