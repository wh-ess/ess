#region

using System.Reflection;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Configurations;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Framework.CQRS.Configurations
{
    /// <summary>
    ///     Configuration class for enode framework.
    /// </summary>
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// 默认 InMemoryEventStore
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static Configuration InitializeCQRSAssemblies(this Configuration configuration, Assembly[] assemblies)
        {
            return InitializeCQRSAssemblies(configuration, new InMemoryEventStore(), assemblies);
        }

        public static Configuration InitializeCQRSAssemblies(this Configuration configuration, IEventStore es, Assembly[] assemblies)
        {
            MessageDispatcher dispatcher = new MessageDispatcher(es);
            dispatcher.ScanAssembly(assemblies);
            Configuration.Instance.SetDefault<MessageDispatcher, MessageDispatcher>(dispatcher);
            
            return configuration;
        }
    }

}