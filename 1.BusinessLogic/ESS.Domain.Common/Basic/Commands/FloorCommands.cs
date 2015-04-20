using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Basic.Commands
{
    public class CreateFloor : Command
    {
        public string Code;
        public string Name;
        public string ShortName;
        public bool IsActive;
    }

    public class EditFloor : CreateFloor
    {
        
    }

    public class DeleteFloor : Command
    {
        
    }
}
