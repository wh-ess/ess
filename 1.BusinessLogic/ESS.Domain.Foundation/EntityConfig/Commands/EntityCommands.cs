using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Foundation.EntityConfig.Commands
{
    public class CreateEntity : Command
    {
        public string ModuleNo;
        public Dictionary<Guid, object> Fields;
    }
    public class EditEntity : CreateEntity
    {
        
    }

    public class DeleteEntity : Command
    {
    }
}
