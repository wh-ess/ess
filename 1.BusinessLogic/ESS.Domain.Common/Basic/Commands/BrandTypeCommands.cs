using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Basic.Commands
{
    public class CreateBrandType : Command
    {
        public string Code;
        public string Name;
        public string ShortName;
        public string Parent;
        public bool IsActive;
    }

    public class EditBrandType : CreateBrandType
    {

    }

    public class DeleteBrandType : Command
    {

    }
}
