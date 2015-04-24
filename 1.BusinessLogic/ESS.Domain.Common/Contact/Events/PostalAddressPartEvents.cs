using System;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Contact.Events
{

    public class PostalAddressPartCreated : Event
    {
        public Guid ContactId;
        public Guid TypeId;
        public Guid GeographicBoundaryId;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Text;

    }

    public class PostalAddressPartChanged : Event
    {
        public Guid ContactId;
        public Guid TypeId;
        public Guid GeographicBoundaryId;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Text;
    }
    public class PostalAddressPartDeleted : Event
    {
    }
}
