#region

using System;

#endregion

namespace ESS.Framework.UI.Attribute
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false)]
    public class ModuleAttribute : System.Attribute
    {
        public string ModuleNo { get; set; }
        public string ParentModuleNo { get; set; }
        public string Action { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool IsMenu { get; set; }

        public ModuleAttribute()
        {
            
        }
        public ModuleAttribute(string moduleNo, string parentModuleNo,string icon="")
        {
            ModuleNo = moduleNo;
            ParentModuleNo = parentModuleNo;
            Icon = icon;
            IsMenu = true;
        }

        public ModuleAttribute(string moduleNo, string actionName)
        {
            ModuleNo = moduleNo;
            Action = actionName;
            switch (actionName)
            {
                case "query":
                    Text = "查询";
                    break;
                case "add":
                    Text = "增加";
                    Icon = "/content/icons/silkicons/add.png";
                    Order = 1;
                    break;
                case "edit":
                    Text = "编辑";
                    Icon = "/content/icons/silkicons/application_edit.png";
                    Order = 2;
                    break;
                case "del":
                    Text = "删除";
                    Icon = "/content/icons/silkicons/delete.png";
                    Order = 3;
                    break;
                case "print":
                    Text = "打印";
                    Icon = "/Content/icons/32X32/print.gif";
                    Order = 4;
                    break;
            }
        }

        public ModuleAttribute(string parentModuleNo, string moduleNo, string actionName, string text, string icon)
        {
            ParentModuleNo = parentModuleNo;
            ModuleNo = moduleNo;
            Action = actionName;
            Text = text;
            Icon = icon;
        }
        
        public ModuleAttribute(string parentModuleNo, string moduleNo, string actionName, string text)
        {
            ParentModuleNo = parentModuleNo;
            ModuleNo = moduleNo;
            Action = actionName;
            Text = text;
        }

    }

}