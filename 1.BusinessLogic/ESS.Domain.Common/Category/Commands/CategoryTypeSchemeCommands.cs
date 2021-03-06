﻿#region

using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Category.Commands
{
    public class CreateCategoryTypeScheme : Command
    {
        public string Name;
        public string Code;
        public bool IsSystem;
    }

    public class ChangeCategoryTypeSchemeName : Command
    {
        public string Name;
    }

    public class DeleteCategoryTypeScheme : Command
    {
    }
}