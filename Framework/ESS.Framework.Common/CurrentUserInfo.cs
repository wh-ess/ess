#region

using System;
using System.Web;

#endregion

namespace ESS.Framework.Common
{
    public class CurrentUserInfo
    {
        public static int CurrentUserId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["userid"]); }
            set { HttpContext.Current.Session["userid"] = value; }
        }

        public static string CurrentUserName
        {
            get { return (string)HttpContext.Current.Session["username"]; }
            set { HttpContext.Current.Session["username"] = value; }
        }

        public static int CurrentRoleId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["roleId"]); }
            set { HttpContext.Current.Session["roleId"] = value; }
        }

        public static string CurrentRoleName
        {
            get { return (string)HttpContext.Current.Session["rolename"]; }
            set { HttpContext.Current.Session["rolename"] = value; }
        }

        public static DateTime CurrentLastLoginTime
        {
            get { return (DateTime)HttpContext.Current.Session["lastlogintime"]; }
            set { HttpContext.Current.Session["lastlogintime"] = value; }
        }
    }
}