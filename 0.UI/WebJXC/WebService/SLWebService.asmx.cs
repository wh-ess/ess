using ESS.Domain.POP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace WebJXC.WebService
{
    /// <summary>
    /// SLWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SLWebService : System.Web.Services.WebService
    {
        private ESS_POSDataContext popdb = new ESS_POSDataContext();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetFileList()
        {

            IList<EmptyTemplate> etlist = popdb.EmptyTemplate.Where(c=>c.IsEnable==1).OrderByDescending(c=>c.Seq).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (EmptyTemplate et in etlist)
            {
                sb.Append(et.ImageFileDir.Substring(1,et.ImageFileDir.Length-1) + et.ImageFileName.Replace('_', '.'));
                sb.Append("|");
            }
            return sb.ToString();
        }
    }
}
