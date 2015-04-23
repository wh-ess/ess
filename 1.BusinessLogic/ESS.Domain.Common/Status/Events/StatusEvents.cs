#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Status.Events
{
    public class StatusCreated : Event
    {
        public Guid RelateId ;
        public Guid TypeId;
        public DateTime StatusDateTime ;
        public DateTime StatusFromDate ;
        public DateTime StatusEndDate ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }


    public class StatusDeleted : Event
    {
    }
}