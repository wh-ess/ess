#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Status.Commands
{
    public class CreateStatusType : Command
    {
        public string Name;
    }

    public class ChangeStatusTypeName : Command
    {
        public string Name;
    }


    public class DeleteStatusType : Command
    {
    }
}