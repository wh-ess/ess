using System;
using System.Collections.Generic;
using System.Linq;
using ESS.Domain.JXC.Models;
using ESS.Framework.UI.Attribute;
using WebUI.Controllers;
using ESS.Framework.UI;


public class SystemConfig
{

    #region ModuleInfo
    private static Dictionary<string, string[]> _actionInfo = new Dictionary<string, string[]>();
    private static Dictionary<string, List<ModuleAttribute>> _moduleInfo = new Dictionary<string, List<ModuleAttribute>>();
    public static Dictionary<string, string[]> ActionInfo
    {
        get
        {
            if (_actionInfo.Count() <= 0)
            {
                var types = typeof(HomeController).Assembly.GetTypes().Where(c => c.Name.Contains("Controller"));
                foreach (var t in types)
                {
                    var methods = t.GetMethods();
                    foreach (var m in methods)
                    {

                        var parameters = m.GetParameters();

                        if (!_actionInfo.ContainsKey("/" + t.Name.Replace("Controller", "").ToLower() + "/" + m.Name.ToLower()))
                        {
                            _actionInfo.Add("/" + t.Name.Replace("Controller", "").ToLower() + "/" + m.Name.ToLower(), parameters.Select(c => c.Name).ToArray());
                        }
                    }
                }

            }
            return _actionInfo;
        }
    }

    public static Dictionary<string, List<ModuleAttribute>> ModuleInfo
    {
        get
        {
            if (_moduleInfo.Count() <= 0)
            {
                var types = typeof(HomeController).Assembly.GetTypes().Where(c => c.Name.Contains("Controller"));
                foreach (var t in types)
                {
                    var methods = t.GetMethods();
                    foreach (var m in methods)
                    {
                        foreach (var attr in m.GetCustomAttributes(typeof(ModuleAttribute), false))
                        {
                            ModuleAttribute att = attr as ModuleAttribute;
                            if (att != null)
                            {
                                att.Url = "/" + t.Name.Replace("Controller", "").ToLower() + "/" + m.Name.ToLower();
                                if (!_moduleInfo.ContainsKey(att.ModuleNo))
                                {
                                    List<ModuleAttribute> list = new List<ModuleAttribute>();
                                    list.Add(att);
                                    _moduleInfo.Add(att.ModuleNo, list);
                                }
                                else
                                {
                                    List<ModuleAttribute> List = _moduleInfo[att.ModuleNo];
                                    List.Add(att);
                                }
                            }
                        }
                    }
                }

            }
            return _moduleInfo;
        }
    }



    #endregion

    private static IList<Column> cf = new List<Column>();
    public static IList<Column> Columns
    {
        get
        {
            if (cf.Count <= 0)
            {
                cf = (new ESS_ERPContext()).Columns.ToList();
            }
            return cf;
        }
    }

    public static decimal Tax
    {
        get
        {
            return Convert.ToDecimal((new ESS_ERPContext()).Configs.First(c => c.Name == "Tax").Val);
        }

    }

    public static bool AllowNegativeInventory
    {
        get
        {
            var conf = new ESS_ERPContext().Configs.First(c => c.Name == "AllowNegativeInventory");
            return conf.Val == "1" ? true : false;
        }

    }

    public static bool AllowNoInventory
    {
        get
        {
            var conf = new ESS_ERPContext().Configs.First(c => c.Name == "AllowNoInventory");
            return conf.Val == "1" ? true : false;
        }

    }


    public string GetConfigValue(string key)
    {
        Config cf = new ESS_ERPContext().Configs.Where(c => c.Name == key).SingleOrDefault();
        if (cf == null)
            return "0";
        return cf.Val;
    }

}
