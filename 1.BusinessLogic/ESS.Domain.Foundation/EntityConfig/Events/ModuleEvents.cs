using System;
using System.Collections.Generic;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.EntityConfig.Events
{
    public class ModulesSaved : Event
    {
        public List<ModuleItem> Modules { get; set; }
    }

    public class ActionSaved : Event
    {
        public string ModuleNo;
        public List<ActionItem> Actions;
    }

}
