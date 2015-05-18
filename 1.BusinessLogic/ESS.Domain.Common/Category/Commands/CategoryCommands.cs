#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Category.Commands
{
    public class CreateCategory : Command
    {
        public Guid TypeId;
        public DateTime EndDate;
        public DateTime FromDate;
        public string Name;
        public Guid ParentId;
        public string Code;
        public bool IsSystem;
    }

    public class ChangeCategoryName : Command
    {
        public string Name;
    }

    public class ChangeCategoryParent : Command
    {
        public Guid ParentId;
    }

    public class ChangeCategoryType : Command
    {
        public Guid TypeId;
    }

    public class ChangeCategoryDate : Command
    {
        public DateTime EndDate;
        public DateTime FromDate;
    }

    public class DeleteCategory : Command
    {
    }
}