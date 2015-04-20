#region

using System;
using ESS.Domain.Common.PartyRole.Domain;

#endregion

namespace ESS.Domain.Common.Status.Domain
{
    public class StatusApplication
    {
        public Guid Id { get; set; }
        public Entity Entity { get; set; }
        public PartyRole.Domain.PartyRole PartyRole { get; set; }
        public StatusType StatusType { get; set; }
        public DateTime StatusDateTime { get; set; }
        public DateTime StatusFromDate { get; set; }
        public DateTime StatusEndDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}