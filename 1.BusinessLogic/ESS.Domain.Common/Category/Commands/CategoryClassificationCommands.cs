﻿#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Category.Commands
{
    public class AddCategoryClassification : Command
    {
        public Guid RelateId;
        public Guid CategoryId;
        public DateTime FromDate;
        public DateTime EndDate;
    }


    public class DeleteCategoryClassification : Command
    {
    }
}