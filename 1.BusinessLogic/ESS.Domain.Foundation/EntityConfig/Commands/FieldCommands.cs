using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Foundation.EntityConfig.Commands
{
    public class SaveFields : Command
    {
        public string ModuleNo { get; set; }
        public string ActionName { get; set; }
        public List<FieldItem> Fields { get; set; }
    }

}
