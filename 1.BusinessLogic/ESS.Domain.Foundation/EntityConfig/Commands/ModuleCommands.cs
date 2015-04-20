using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Foundation.EntityConfig.Commands
{
    public class SaveModules : Command
    {
        public List<ModuleItem> Modules { get; set; }
    }

    public class SaveAction : Command
    {
        public string ModuleNo;
        public List<ActionItem> Actions;
    }

}
