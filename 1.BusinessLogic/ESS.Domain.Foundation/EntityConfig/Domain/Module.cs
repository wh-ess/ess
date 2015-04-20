#region

using System.Collections;
using AutoMapper;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.Domain
{
    public class Module: Aggregate, IHandleCommand<SaveModules>, IHandleCommand<SaveAction>,IApplyEvent<ModulesSaved>,IApplyEvent<ActionSaved>
    {
        #region handle

        public IEnumerable Handle(SaveModules c)
        {
            yield return new ModulesSaved() {Id = c.Id,Modules = c.Modules };
        }
        public IEnumerable Handle(SaveAction c)
        {
            yield return new ActionSaved() { Id = c.Id, ModuleNo = c.ModuleNo,Actions = c.Actions};
        }
        #endregion

        #region apply

        public void Apply(ModulesSaved e)
        {
            
        }
        public void Apply(ActionSaved e)
        {
            
        }

        #endregion

        
    }
}