using System.Web.Hosting;

namespace ESS.Framework.Common.Utilities
{
    public class Path
    {
        public static string ServerPath()
        {
            return HostingEnvironment.MapPath("/");
        }
    }
}
