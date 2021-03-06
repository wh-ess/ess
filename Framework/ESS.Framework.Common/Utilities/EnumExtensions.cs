﻿#region

using System;
using System.Collections;
using System.Linq;

#endregion

namespace ESS.Framework.Common.Utilities
{
    public static class EnumExtensions
    {
        public static string ToJson(Type t, string varName = null)
        {
            if (varName == null)
            {
                varName = t.Name.ToCamelCase();
            }
            var ret = "\"" + varName + "\": {";
            var list = (from object v in Enum.GetValues(t) select "\"" + (Enum.GetName(t, v).ToCamelCase() + "\":\"" + v.ToString().ToCamelCase()+ "\""));
            ret += string.Join(",", list);
            ret += "}";
            return ret;
        }

        public static IEnumerable ToEnumClass(Type t)
        {
            return from object v in Enum.GetValues((t)) select new EnumClass { Id = v.ToString().ToCamelCase(), Name = Enum.GetName(t, v) };
        }

        public class EnumClass
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}