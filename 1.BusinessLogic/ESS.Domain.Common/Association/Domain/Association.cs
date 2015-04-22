using System;
using ESS.Domain.Common.PartyRole.Domain;
using ESS.Domain.Common.Status.Domain;

namespace ESS.Domain.Common.Association.Domain
{
    public class Association
    {
        public Guid Id { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public AssociationRule AssociationRule { get; set; }
        public Guid AssociationTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
