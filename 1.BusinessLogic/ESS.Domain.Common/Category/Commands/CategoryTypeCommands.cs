#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Category.Commands
{
    public class CreateCategoryType : Command
    {
        public string Code;
        public bool IsSystem;
        public string Name;
        public Guid ParentId;
        public Guid SchemeId;
    }

    public class ChangeCategoryTypeName : Command
    {
        public string Name;
    }

    public class ChangeCategoryTypeParent : Command
    {
        public Guid ParentId;
    }

    public class ChangeCategoryTypeScheme : Command
    {
        public Guid SchemeId;
    }

    public class DeleteCategoryType : Command
    {
    }
}