#region

using System;
using System.Reflection;
using ESS.Framework.Common.Cache;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Logging;
using ESS.Framework.Common.Retring;
using ESS.Framework.Common.Scheduling;
using ESS.Framework.Common.Serializing;

#endregion

namespace ESS.Framework.Common.Configurations
{
    public class Configuration
    {
        private Configuration(Setting setting)
        {
            Setting = setting ?? new Setting();
        }

        public Setting Setting { get; private set; }
        public static Configuration Instance { get; private set; }

        public static Configuration Create(Setting setting = null)
        {
            if (Instance != null)
            {
                throw new Exception("Could not create configuration instance twice.");
            }
            Instance = new Configuration(setting);
            return Instance;
        }

        public Configuration SetDefault<TService, TImplementer>(LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            ObjectContainer.Register<TService, TImplementer>(life);
            return this;
        }

        public Configuration SetDefault<TService, TImplementer>(TImplementer instance) where TService : class
            where TImplementer : class, TService
        {
            ObjectContainer.RegisterInstance<TService, TImplementer>(instance);
            return this;
        }

        public Configuration RegisterCommonComponents()
        {
            SetDefault<IMemoryCache, DefaultMemoryCache>();
            SetDefault<IBinarySerializer, DefaultBinarySerializer>();
            SetDefault<IScheduleService, ScheduleService>();
            SetDefault<IActionExecutionService, ActionExecutionService>(LifeStyle.Transient);

            return this;
        }

        
    }
}