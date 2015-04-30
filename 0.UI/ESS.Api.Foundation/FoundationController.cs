#region

using System.Web.Http;
using ESS.Framework.UI.Attribute;

#endregion

namespace ESS.Api.Foundation
{
    /// <summary>
    /// for menu
    /// </summary>

    [Module(parentModuleNo: "", moduleNo: "Foundation")]
    public class FoundationController : ApiController
    {
    }

    public class AccessControlController : ApiController
    {
    }
    public class EntityConfigController : ApiController
    {
    }
    public class SystemConfigController : ApiController
    {
    }
}