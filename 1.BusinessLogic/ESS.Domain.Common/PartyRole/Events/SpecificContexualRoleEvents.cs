#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.PartyRole.Events
{
    public class SpecificContexualRoleCreated : Event
    {
        public Guid RoleTypeId;
        public Guid EntityId;
        public DateTime FromDate;
        public DateTime EndDate;
    }
    public class SpecificContexualRoleEdited : SpecificContexualRoleCreated
    {

    }

    public class SpecificContexualRoleDeleted : Event
    {
    }
}