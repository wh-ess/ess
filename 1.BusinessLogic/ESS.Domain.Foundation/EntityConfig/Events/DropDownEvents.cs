using System;
using System.Collections.Generic;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.EntityConfig.Events
{
    public class DropDownCreated : Event
    {
        public bool IsSystem { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class DropDownEdited : DropDownCreated
    {
        
    }

    public class DropDownDeleted : Event
    {
    }
}
