#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Category.Events
{
    public class CategoryClassificationAdded : Event
    {
        public Guid RelateId;
        public Guid CategoryId;
        public DateTime FromDate;
        public DateTime EndDate;
    }


    public class CategoryClassificationDeleted : Event
    {
    }
}