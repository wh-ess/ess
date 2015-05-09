#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.PartyRole.Events
{
    public class PartyRoleCreated : Event
    {
        public Guid PartyId ;
        public Guid TypeId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }
    public class PartyRoleEdited : PartyRoleCreated
    {

    }

    public class PartyRoleDeleted : Event
    {
    }
}