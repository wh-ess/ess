#region

using System;
using ESS.Domain.Common.Category.ReadModels;
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
        public CategoryTypeSchemeItem Scheme;
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