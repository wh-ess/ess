#region

using System;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Category.Events
{
    public class CategoryCreated : Event
    {
        public Guid CategoryTypeId;
        public DateTime EndDate;
        public DateTime FromDate;
        public string Name;
        public Guid ParentId;
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
        public Guid CategoryTypeId;
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