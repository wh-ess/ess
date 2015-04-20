#region

using System;
using System.Linq;
using System.Web.Http;
using ESS.Framework.Common.Utilities;
using ESS.Framework.UI.Configurations;

#endregion

namespace ESS.Api.Foundation.EntityConfig
{
    public class EnumController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public string Enums()
        {
            var contentParts = ModuleBuilder.Enums.Select(e => EnumExtensions.ToJson(e))
                .ToList();
            return String.Format("{{ {0} }}", String.Join(",", contentParts));
        }
    }
}