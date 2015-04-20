using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Basic.Commands
{
    public class CreateBank : Command
    {
        public string Code;
        public string Name;
        public string ShortName;
        public bool IsActive;
    }

    public class EditBank : CreateBank
    {

    }

    public class DeleteBank : Command
    {

    }
}
