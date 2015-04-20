using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Basic.Commands
{
    public class CreateSupplier : Command
    {
        public string Code;
        public string Name;
        public string ShortName;
        public bool IsActive;
    }

    public class EditSupplier : CreateSupplier
    {

    }

    public class DeleteSupplier : Command
    {

    }
}
