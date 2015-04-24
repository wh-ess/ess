using System;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.PartyContact.Commands
{

    public class CreatePartyContact : Command
    {
        public Guid PartyId ;
        public Guid ContactId ;

    }

    public class ChangePartyContact : Command
    {
        public Guid PartyId ;
        public Guid ContactId ;
    }
    public class DeletePartyContact : Command
    {
    }
}
