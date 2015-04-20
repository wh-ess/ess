#region

using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using ESS.Framework.Common;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Logging;
using ESS.Framework.Common.Utilities;
using ESS.Framework.Licensing;

#endregion

namespace ESS.Framework.UI
{
    public class LogAttribute : ActionFilterAttribute, IActionFilter, IResultFilter, IExceptionFilter
    {
        protected readonly ILogger log = ObjectContainer.Resolve<ILoggerFactory>().Create(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region 权限

            var role = CurrentUserInfo.CurrentRoleId;
            if (role == default(int))
            {
                //test
                if (ClientIP.GetClientIP() == "::1")
                {
                    CurrentUserInfo.CurrentUserId = 1;
                    CurrentUserInfo.CurrentUserName = "张飞";
                    CurrentUserInfo.CurrentRoleId = 1;
                    CurrentUserInfo.CurrentRoleName = "电脑测试";
                    CurrentUserInfo.CurrentLastLoginTime = DateTime.Now;
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("/home/index");
                }
            }
            #endregion

            #region log

            var sb = new StringBuilder();
            sb.Append(string.Format("controllerName = '{0}' ", filterContext.Controller.GetType().Name));
            sb.Append(string.Format("actionName = '{0}' ", filterContext.ActionDescriptor.ActionName));
            sb.Append(string.Format("httpmethod = '{0}' ", filterContext.HttpContext.Request.HttpMethod));

            NameValueCollection nv;
            if (filterContext.HttpContext.Request.HttpMethod == "POST")
            {
                nv = filterContext.HttpContext.Request.Form;
            }
            else
            {
                nv = filterContext.HttpContext.Request.QueryString;
            }
            if (nv.Count > 0)
            {
                sb.Append("Parameter : ");
                foreach (string key in nv)
                {
                    sb.Append(string.Format(" '{0}' : '{1}' ", key, nv[key]));
                }
            }
            log.Info(sb.ToString());

            #endregion

            #region gzip压缩

            var acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToLower();
                var response = filterContext.HttpContext.Response;
                if (acceptEncoding.Contains("gzip"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("deflate"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                }
            }

            #endregion

            #region license
            try
            {
                //var path = filterContext.HttpContext.Server.MapPath("/");
                //var publicKey = File.ReadAllText(path + "bin/publicKey.xml");
                //new LicenseValidator(publicKey, path+"bin/license.lic")
                //                  .AssertValidLicense();
            }
            catch (Exception)
            {

                filterContext.HttpContext.Response.Redirect("/home/index");
            }
            #endregion
        }

        public void OnException(ExceptionContext filterContext)
        {
            log.Error(CurrentUserInfo.CurrentUserName + " " + filterContext.Exception.Message);
        }
    }
}