using System;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Contact.Events
{

    public class PartyContactCreated : Event
    {
        public Guid PartyId ;
        public Guid ContactId ;

    }

    public class PartyContactChanged : Event
    {
        public Guid PartyId ;
        public Guid ContactId ;
    }
    public class PartyContactDeleted: Event
    {
    }
}
