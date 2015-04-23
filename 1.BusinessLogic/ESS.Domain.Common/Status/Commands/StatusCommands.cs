#region

using System;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.Status.Commands
{
    public class CreateStatus : Command
    {
        public Guid RelateId ;
        public Guid TypeId;
        public DateTime StatusDateTime ;
        public DateTime StatusFromDate ;
        public DateTime StatusEndDate ;
        public DateTime FromDate ;
        public DateTime EndDate ;
    }


    public class DeleteStatus : Command
    {
    }
}