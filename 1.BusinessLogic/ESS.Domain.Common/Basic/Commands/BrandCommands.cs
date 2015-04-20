using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Basic.Commands
{
    public class CreateBrand : Command
    {
        public string Code;
        public string Name;
        public string ShortName;
        public string Parent;
        public bool IsActive;
    }

    public class EditBrand : CreateBrand
    {

    }

    public class DeleteBrand : Command
    {

    }
}
