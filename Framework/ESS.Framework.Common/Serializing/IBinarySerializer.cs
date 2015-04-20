#region

using System;
using System.Collections.Generic;

#endregion

namespace ESS.Framework.Common.Serializing
{
    public interface IBinarySerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(byte[] data, Type type);
        T Deserialize<T>(byte[] data) where T : class;

        IEnumerable<T> Deserialize<T>(byte[][] data) where T : class;
    }
}