#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.PartyRole.Commands
{
    public class CreatePartyRole : Command
    {
        public Guid PartyId ;
        public Guid TypeId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }


    public class DeletePartyRole : Command
    {
    }
}