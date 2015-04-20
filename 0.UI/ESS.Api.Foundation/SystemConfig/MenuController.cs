#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.UI.Configurations;

#endregion

namespace ESS.Api.Foundation.SystemConfig
{
    [RoutePrefix("api/Menu")]
    public class MenuController : ApiController
    {
        private readonly ModuleView _moduleView;

        public MenuController(ModuleView moduleView)
        {
            _moduleView = moduleView;
        }

        [HttpGet]
        public IEnumerable<ModuleItem> Menu()
        {
            var modules = from p in ModuleBuilder.Modules
                          group p by new { p.ModuleNo, p.ParentModuleNo }
                              into g
                              select new { g.Key.ParentModuleNo, g.Key.ModuleNo };
            var moduleConfig = _moduleView.ModuleConfig;

            var list = from m in modules
                       join mc in moduleConfig on new { m.ParentModuleNo, m.ModuleNo } equals new { mc.ParentModuleNo, mc.ModuleNo } into gg
                       from g in gg.DefaultIfEmpty(new ModuleItem())
                       select
                           new ModuleItem
                           {
                               ParentModuleNo = m.ParentModuleNo,
                               ModuleNo = m.ModuleNo,
                               Text = g.Text,
                               Icon = g.Icon,
                               Url = g.Url,
                               IsMenu = g.IsMenu,
                               Actions = g.Actions,
                               Order = g.Order
                           };

            return list.Where(c => c.IsMenu).OrderBy(c => c.Order);
        }

    }
}