using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Category.Commands
{
    public class CreateCategoryType : Command
    {
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
