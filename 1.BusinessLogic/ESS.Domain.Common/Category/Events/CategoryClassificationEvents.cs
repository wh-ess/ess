#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Category.Events
{
    public class CategoryClassificationCreated : Event
    {
        public Guid RelateId;
        public Guid CategoryId;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Code;
        public bool IsSystem;
    }


    public class CategoryClassificationDeleted : Event
    {
    }
}