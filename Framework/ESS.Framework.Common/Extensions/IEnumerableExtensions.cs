﻿#region

using System;
using System.Collections.Generic;

#endregion

namespace ESS.Framework.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var element in enumerable)
                action(element);
        }
    }
}