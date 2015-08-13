

using System;
using System.Linq.Expressions;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Configurations;
using StackExchange.Redis;

namespace ESS.Framework.Data.Redis
{
    /// <summary>configuration class Redis extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>Use Redis to implement the memory cache.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseRedisRepositoryAsync(this Configuration configuration)
        {
            return UseRedisRepositoryAsync(configuration, "127.0.0.1", 6379);
        }
        /// <summary>Use Redis to implement the memory cache.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseRedisRepositoryAsync(this Configuration configuration, string host, int port)
        {
            var redis = ConnectionMultiplexer.Connect(host + ":" + port).GetDatabase(1);
            configuration.SetDefault<IDatabase, IDatabase>(redis);
            ObjectContainer.Current.RegisterGeneric(typeof(IRepositoryAsync<,>), typeof(RedisRepositoryAsync<,>));
            return configuration;
        }
    }
}