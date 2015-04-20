using System;
using System.Collections.Generic;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.EntityConfig.Events
{
    public class FieldsSaved : Event
    {
        public string ModuleNo { get; set; }
        public string ActionName { get; set; }
        public List<FieldItem> Fields { get; set; }
    }
}
