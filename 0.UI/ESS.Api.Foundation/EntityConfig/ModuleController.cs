#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS;
using ESS.Framework.UI.Configurations;

#endregion

namespace ESS.Api.Foundation.EntityConfig
{
    [RoutePrefix("api/Module")]
    public class ModuleController : ApiController
    {
        private readonly DefaultMessageBus _messageDispatcher;
        private readonly ModuleView _moduleView;

        public ModuleController(DefaultMessageBus messageDispatcher, ModuleView moduleView)
        {
            _messageDispatcher = messageDispatcher;
            _moduleView = moduleView;
        }

        [HttpGet]
        public IEnumerable<ModuleItem> ModuleList()
        {
            var modules = from p in ModuleBuilder.Modules
                group p by new { p.ModuleNo, p.ParentModuleNo }
                into g select new { g.Key.ParentModuleNo, g.Key.ModuleNo };
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

            return list.OrderBy(c => c.ParentModuleNo).ThenBy(c=>c.ModuleNo);
        }

        public void SaveModule(List<ModuleItem> list)
        {
            _messageDispatcher.SendCommand(new SaveModules { Modules = list });
        }

        [Route("{moduleNo}/Actions")]
        public IEnumerable<ActionItem> GetActions(string moduleNo)
        {
            var actions = from p in ModuleBuilder.Modules.Where(c => c.ModuleNo == moduleNo && c.Action!=null) group p by p.Action into g select g.Key;
            var actionConfig = new List<ActionItem>();
            var module = _moduleView.ModuleConfig.FirstOrDefault(c => c.ModuleNo == moduleNo);
            if (module != null)
            {
                actionConfig = module.Actions;
            }
            var list = from m in actions
                join mc in actionConfig on m equals mc.Name into gg
                from g in gg.DefaultIfEmpty(new ActionItem())
                select
                    new ActionItem { Name = m, Text = g.Text, Type = g.Type, IsBatch = g.IsBatch, Icon = g.Icon, Action = g.Action, Order = g.Order };

            return list.OrderBy(c => c.Order);
        }

        [Route("{moduleNo}/Actions")]
        public void SaveActions(string moduleNo, List<ActionItem> list)
        {
            _messageDispatcher.SendCommand(new SaveAction {ModuleNo = moduleNo, Actions = list });
        }
    }
}