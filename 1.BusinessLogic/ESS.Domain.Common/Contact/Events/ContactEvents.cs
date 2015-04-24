using System;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Contact.Events
{

    public class ContactCreated: Event
    {
        public string CountryCode;
        public string AreaCode;
        public string PhoneNumber;
        public string Address;

    }

    public class ContactChanged : Event
    {
        public string CountryCode;
        public string AreaCode;
        public string PhoneNumber;
        public string Address;
    }
    public class ContactDeleted : Event
    {
    }
}
