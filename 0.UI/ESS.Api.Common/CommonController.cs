#region



#endregion

using System.Web.Http;
using ESS.Framework.UI.Attribute;

namespace ESS.Api.Common
{
    /// <summary>
    /// for menu
    /// </summary>

    [Module(parentModuleNo: "", moduleNo: "Common")]
    public class CommonController : ApiController
    {
    }
    public class BasicController : ApiController
    {
    }

    [Module(parentModuleNo: "Common", moduleNo: "Category")]
    public class CategoryMainController : ApiController
    {
    }

    [Module(parentModuleNo: "Common", moduleNo: "Association")]
    public class AssociationMainController : ApiController
    {
    }

}