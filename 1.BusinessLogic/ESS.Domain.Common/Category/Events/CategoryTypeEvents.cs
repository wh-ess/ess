using System;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Category.Events
{
    public class CategoryTypeCreated : Event
    {
        public string Name;
        public Guid ParentId;
        public Guid SchemeId;
        public string Code;
        public bool IsSystem;

    }

    public class CategoryTypeNameChanged : Event
    {
        public string Name;
    }
    public class CategoryTypeParentChanged : Event
    {
        public Guid ParentId;
    }
    public class CategoryTypeSchemeChanged : Event
    {
        public Guid SchemeId;
    }
    public class CategoryTypeDeleted : Event
    {
    }
}
