#region

using System;
using ESS.Domain.Common.PartyRole.ReadModels;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.PartyRole.Events
{
    public class PartyRoleCreated : Event
    {
        public PartyItem Party;
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