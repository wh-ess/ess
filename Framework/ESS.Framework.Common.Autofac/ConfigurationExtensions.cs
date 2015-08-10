#region

using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Configurations;

#endregion

namespace ESS.Framework.Common.Autofac
{
    /// <summary>
    ///     configuration class Autofac extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        ///     Use Autofac as the object container.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseAutofac(this Configuration configuration)
        {
            return UseAutofac(configuration, new ContainerBuilder());
        }

        /// <summary>
        ///     Use Autofac as the object container.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseAutofac(this Configuration configuration, ContainerBuilder containerBuilder)
        {
            ObjectContainer.SetContainer(new AutofacObjectContainer(containerBuilder));
            return configuration;
        }

        public static Configuration RegisterBusinessComponents(this Configuration configuration, Assembly[] assemblies)
        {
            var autofacObjectContainer = ObjectContainer.Current as AutofacObjectContainer;
            if (autofacObjectContainer != null)
            {
                var container = autofacObjectContainer.Container;
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyTypes(assemblies);
                builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
                builder.Update(container);
            }
            return configuration;
        }

    }
}