#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Category.Commands
{
    public class CreateCategoryClassification : Command
    {
        public Guid RelateId;
        public Guid CategoryId;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Code;
        public bool IsSystem;
    }


    public class DeleteCategoryClassification : Command
    {
    }
}