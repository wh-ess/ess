#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.PartyRole.Events
{
    public class GenericContexualRoleCreated : Event
    {
        public Guid PartyId ;
        public Guid EntityId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }
    public class GenericContexualRoleEdited : GenericContexualRoleCreated
    {

    }

    public class GenericContexualRoleDeleted : Event
    {
    }
}