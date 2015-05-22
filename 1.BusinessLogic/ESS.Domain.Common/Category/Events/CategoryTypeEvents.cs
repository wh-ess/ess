#region

using System;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Category.Events
{
    public class CategoryTypeCreated : Event
    {
        public string Code;
        public bool IsSystem;
        public string Name;
        public Guid ParentId;
        public CategoryTypeSchemeItem Scheme;
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
        public CategoryTypeSchemeItem Scheme;
    }

    public class CategoryTypeDeleted : Event
    {
    }
}