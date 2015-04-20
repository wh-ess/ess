#region

using System;

#endregion

namespace ESS.Framework.Common.Serializing
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        object Deserialize(string value, Type type);
        T Deserialize<T>(string value) where T : class;
    }
}