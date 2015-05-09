#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.PartyRole.Commands
{
    public class CreateSpecificContexualRole : Command
    {
        public Guid RoleTypeId ;
        public Guid EntityId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }
    public class EditSpecificContexualRole : CreateSpecificContexualRole
    {

    }

    public class DeleteSpecificContexualRole : Command
    {
    }
}