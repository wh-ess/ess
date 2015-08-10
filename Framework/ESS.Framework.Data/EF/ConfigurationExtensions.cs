using ESS.Framework.Common.Components;
using ESS.Framework.Common.Configurations;
using ESS.Framework.Data.Dapper;

namespace ESS.Framework.Data.EF
{
    /// <summary>configuration class Redis extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        
        /// <summary>Use Redis to implement the memory cache.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseEfRepositoryAsync(this Configuration configuration, string connString)
        {
            ObjectContainer.Current.RegisterGeneric(typeof(IRepositoryAsync<,>), typeof(EfRepositoryAsync<,>), LifeStyle.Singleton,connString);
            return configuration;
        }

        public static Configuration UseEfRepository(this Configuration configuration, string connString)
        {
            ObjectContainer.Current.RegisterGeneric(typeof(IRepository<,>), typeof(EfRepository<,>), LifeStyle.Singleton, connString);
            return configuration;
        }
    }
}