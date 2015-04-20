using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESS.Framework.CQRS.Event
{
    public interface IEvent
    {
        Guid Id { get; set; }
    }
}
