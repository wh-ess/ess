using System;
using System.Collections.Generic;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.EntityConfig.Events
{
    public class EntityCreated : Event
    {
        public string ModuleNo;
        public Dictionary<Guid, object> Fields;
    }
    public class EntityEdited : EntityCreated
    {
        
    }

    public class EntityDeleted : Event
    {
    }
}
