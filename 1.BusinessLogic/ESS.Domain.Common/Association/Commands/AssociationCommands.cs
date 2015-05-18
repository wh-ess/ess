#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Domain.Common.PartyRole.ReadModels;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Association.Commands
{
    public class CreateAssociation : Command
    {
        public Guid From;
        public Guid To;
        public AssociationRule AssociationRule;
        public Guid TypeId;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Code;
        public bool IsSystem;
    }
    public class EditAssociation : CreateAssociation
    {
    }

    public class DeleteAssociation : Command
    {
    }
}