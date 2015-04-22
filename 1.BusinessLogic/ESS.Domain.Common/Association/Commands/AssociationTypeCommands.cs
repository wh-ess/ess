#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Association.Commands
{
    public class CreateAssociationType : Command
    {
        public string Name;
        public Guid ParentId;
    }

    public class ChangeAssociationTypeName : Command
    {
        public string Name;
    }

    public class ChangeAssociationTypeParent : Command
    {
        public Guid ParentId;
    }

    public class DeleteAssociationType : Command
    {
    }
}