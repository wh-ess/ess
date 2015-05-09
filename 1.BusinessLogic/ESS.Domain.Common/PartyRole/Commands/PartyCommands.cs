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
        public DateTime BirthDay ;
        //员工相片路径
        public string Photo ;
    }

    public class EditParty : CreateParty
    {
        
    }


    public class DeleteParty : Command
    {
    }
}