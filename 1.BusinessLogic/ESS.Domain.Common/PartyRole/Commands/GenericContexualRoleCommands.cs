#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.PartyRole.Commands
{
    public class CreateGenericContexualRole : Command
    {
        public Guid PartyId ;
        public Guid EntityId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }
    public class EditGenericContexualRole : CreateGenericContexualRole
    {

    }


    public class DeleteGenericContexualRole : Command
    {
    }
}