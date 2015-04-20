﻿using ESS.Framework.Common.Components;
using ESS.Framework.Common.Configurations;

namespace ESS.Framework.Data.InMemory
{
    /// <summary>ENode configuration class Redis extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>Use InMemory to implement the Repository.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseInMemoryRepository(this Configuration configuration)
        {
            ObjectContainer.Current.RegisterGeneric(typeof(IRepository<,>), typeof(InMemoryRepository<,>));
            return configuration;
        }
    }
}