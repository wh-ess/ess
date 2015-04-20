#region

using System;

#endregion

namespace ESS.Framework.UI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MenuAttribute : System.Attribute
    {
        public MenuInfo menuInfo = new MenuInfo();

        public MenuAttribute(string menuNo, string title) :
            this(menuNo, title, null, null, null, null, 0)
        {
        }

        public MenuAttribute(string menuNo, string title, string parentNo) :
            this(menuNo, title, null, parentNo, null, null, 0)
        {
        }

        public MenuAttribute(string menuNo, string title, string application, string parentNo, string url, string icon,
            int order)
        {
            menuInfo.MenuNo = menuNo;
            menuInfo.Application = application;
            menuInfo.Title = title;
            menuInfo.ParentNo = parentNo;
            menuInfo.Url = url;
            menuInfo.Icon = icon;
            menuInfo.Order = order;
        }
    }

    public class MenuInfo
    {
        public string MenuNo { get; set; }
        public string Application { get; set; }
        public string Title { get; set; }
        public string ParentNo { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
    }
}