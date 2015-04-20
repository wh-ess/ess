#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.Commands
{
    public class CreateDropDown : Command
    {
        public bool IsSystem { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class EditDropDown : CreateDropDown
    {
    }

    public class DeleteDropDown : Command
    {
    }
}