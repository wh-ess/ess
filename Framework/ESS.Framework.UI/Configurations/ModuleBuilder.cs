#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using ESS.Framework.UI.Attribute;

#endregion

namespace ESS.Framework.UI.Configurations
{
    public class ModuleBuilder
    {
        public static List<ModuleAttribute> Modules = new List<ModuleAttribute>();
        public static List<Type> Enums = new List<Type>();

        public static void Build(Assembly[] assemblies)
        {
            BuildModules(assemblies);
            BuildEnum(assemblies);
        }

        private static void BuildModules(IEnumerable<Assembly> assemblies)
        {
            Modules.Clear();
            var types = from a in assemblies from t in a.GetTypes() where t.Name.EndsWith("Controller") select t;
            foreach (var t in types)
            {
                var moduleInfoType = t.GetCustomAttribute<ModuleAttribute>();
                var moduleNo = t.Name.Replace("Controller", "");
                var moduleParentNo = "";
                if (t.Namespace != null)
                {
                    moduleParentNo = t.Namespace.Substring(t.Namespace.LastIndexOf('.') + 1);
                }
                if (moduleInfoType != null)
                {
                    if (!string.IsNullOrEmpty(moduleInfoType.ModuleNo))
                    {
                        moduleNo = moduleInfoType.ModuleNo;
                    }
                    moduleParentNo = moduleInfoType.ParentModuleNo;
                    
                }

                //for menu
                Modules.Add(new ModuleAttribute(){ParentModuleNo = moduleParentNo,ModuleNo = moduleNo});

                var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var m in methods)
                {
                    var moduleInfo = new ModuleAttribute();
                    var attr = m.GetCustomAttribute<ModuleAttribute>();

                    if (attr != null)
                    {
                        moduleInfo = attr;
                    }
                    if (string.IsNullOrEmpty(moduleInfo.ParentModuleNo))
                    {
                        moduleInfo.ParentModuleNo = moduleParentNo;
                    }
                    if (string.IsNullOrEmpty(moduleInfo.ModuleNo))
                    {
                        moduleInfo.ModuleNo = moduleNo;
                    }
                    if (string.IsNullOrEmpty(moduleInfo.Action))
                    {
                        moduleInfo.Action = m.Name;
                    }
                    Modules.Add(moduleInfo);
                }
            }
            Modules.Distinct();
        }

        private static void BuildEnum(IEnumerable<Assembly> assemblies)
        {
            Enums.Clear();
            var types = from a in assemblies from t in a.GetTypes() where t.FullName.Contains("Domain")&&t.IsEnum select t;
            foreach (var t in types)
            {
               Enums.Add(t);
            }
        }
}
}