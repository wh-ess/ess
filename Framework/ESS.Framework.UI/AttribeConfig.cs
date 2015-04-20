#region

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ESS.Framework.UI.Attribute;

#endregion

namespace ESS.Framework.UI
{
    public sealed class AttribeConfig
    {
        public Dictionary<string, string[]> ActionInfo = new Dictionary<string, string[]>();
        public IList<MenuInfo> MenuInfo = new List<MenuInfo>();
        public List<ModuleAttribute> ModuleInfo = new List<ModuleAttribute>();


        public void GenModulInfo(Assembly[] assemblys)
        {
            if (!ModuleInfo.Any() || !MenuInfo.Any())
            {
                var types = from a in assemblys from t in a.GetTypes() where t.Name.Contains("Controller") select t;

                foreach (var t in types)
                {
                    var areaName = t.FullName.Split('.')[2];
                    var methods = t.GetMethods();
                    foreach (var m in methods)
                    {
                        var url = "/" + areaName + "/" + t.Name.Replace("Controller", "")
                            .ToLower() + "/" + m.Name.ToLower();

                        #region moduleinfo

                        foreach (var attr in m.GetCustomAttributes(typeof(ModuleAttribute), false))
                        {
                            var att = attr as ModuleAttribute;
                            if (att != null)
                            {
                                att.Url = url;

                                ModuleInfo.Add(att);
                            }
                        }

                        #endregion

                        #region menuinfo

                        foreach (var attr in m.GetCustomAttributes(typeof(MenuAttribute), false))
                        {
                            var att = attr as MenuAttribute;
                            if (att != null)
                            {
                                att.menuInfo.Url = url;
                                if (string.IsNullOrEmpty(att.menuInfo.Application))
                                {
                                    att.menuInfo.Application = areaName;
                                    att.menuInfo.ParentNo = areaName;
                                }
                                MenuInfo.Add(att.menuInfo);
                            }
                        }

                        #endregion

                        #region ActionInfo

                        var parameters = m.GetParameters();
                        if (!ActionInfo.ContainsKey(url))
                        {
                            ActionInfo.Add(url, parameters.Select(c => c.Name)
                                .ToArray());
                        }

                        #endregion
                    }
                }
            }
        }
    }
}