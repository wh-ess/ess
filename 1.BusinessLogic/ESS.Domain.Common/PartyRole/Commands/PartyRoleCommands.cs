#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Domain.Common.PartyRole.ReadModels;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.PartyRole.Commands
{
    public class CreatePartyRole : Command
    {
        public PartyItem Party ;
        public Guid TypeId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }
    public class EditPartyRole : CreatePartyRole
    {

    }

    public class DeletePartyRole : Command
    {
    }
}