#region

using System;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Category.Events
{
    public class CategoryCreated : Event
    {
        public CategoryTypeItem Type;
        public DateTime EndDate;
        public DateTime FromDate;
        public string Name;
        public Guid ParentId;
        public string Code;
        public bool IsSystem;
    }

    public class CategoryNameChanged : Event
    {
        public string Name;
    }

    public class CategoryParentChanged : Event
    {
        public Guid ParentId;
    }

    public class CategoryTypeChanged : Event
    {
        public CategoryTypeItem Type;
    }

    public class CategoryDateChanged : Event
    {
        public DateTime EndDate;
        public DateTime FromDate;
    }

    public class CategoryDeleted : Event
    {
    }
}