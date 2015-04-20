#region

using System;
using ESS.Framework.Common.Configurations;
using ESS.Framework.Common.Serializing;

#endregion

namespace ESS.Framework.Common.JsonNet
{
    /// <summary>
    ///     ECommon configuration class JsonNet extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        ///     Use Json.Net as the json serializer.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseJsonNet(this Configuration configuration, params Type[] creationWithoutConstructorTypes)
        {
            configuration.SetDefault<IJsonSerializer, NewtonsoftJsonSerializer>(new NewtonsoftJsonSerializer(creationWithoutConstructorTypes));
            return configuration;
        }
    }
}