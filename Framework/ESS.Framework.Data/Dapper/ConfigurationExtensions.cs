﻿

using ESS.Framework.Common.Components;
using ESS.Framework.Common.Configurations;

namespace ESS.Framework.Data.Dapper
{
    /// <summary>configuration class Redis extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        
        /// <summary>Use Redis to implement the memory cache.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseDapperRepository(this Configuration configuration, string connString)
        {
            ObjectContainer.Current.RegisterGeneric(typeof(IRepositoryAsync<,>), typeof(DapperRepositoryAsync<,>), LifeStyle.Singleton,connString);
            return configuration;
        }
    }
}