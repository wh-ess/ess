using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Mall.Pop.Commands
{
    public class CreatePopTemplate : Command
    {
        public string Name;
        public string Image;
        public int Seq;
        public DateTime StartDate;
        public DateTime EndDate;
        public bool IsEnable;
    }

    public class EditPopTemplate : CreatePopTemplate
    {
        
    }

    public class DeletePopTemplate : Command
    {
        
    }
}
