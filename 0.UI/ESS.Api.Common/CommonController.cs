#region



#endregion

using System.Web.Http;
using ESS.Framework.UI.Attribute;

namespace ESS.Api.Common
{
    /// <summary>
    /// for menu
    /// </summary>
    public class CommonController : ApiController
    {
    }
    public class BasicController : ApiController
    {
    }

    [Module(parentModuleNo: "", moduleNo: "Category")]
    public class CategoryMainController : ApiController
    {
    }

}