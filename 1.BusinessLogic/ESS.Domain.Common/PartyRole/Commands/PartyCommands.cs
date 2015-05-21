#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.PartyRole.Commands
{
    public class CreateParty : Command
    {
        public string Name ;
        public string Photo ;
    }

    public class ChangePartyName : Command
    {
        public string Name;
    }
    public class ChangePartyPhoto : Command
    {
        public string Photo;
    }


    public class DeleteParty : Command
    {
    }
}