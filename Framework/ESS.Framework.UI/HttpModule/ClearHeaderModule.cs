using System;
using System.Web;

namespace ESS.Framework.UI.HttpModule
{
    public class ClearHeaderModule : IHttpModule
    {
        private static readonly string[] Headers = new string[3] { "Server", "X-AspNet-Version", "X-Powered-By", };

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        private static void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                return;
            for (var i = 0;i < 3;i++)
            {
                HttpContext.Current.Response.Headers.Remove(Headers[i]);
            }
        }

        public void Dispose()
        {
        }
    }
}
