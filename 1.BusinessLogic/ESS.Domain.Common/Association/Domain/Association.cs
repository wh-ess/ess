using System;
using ESS.Domain.Common.PartyRole.Domain;
using ESS.Domain.Common.Status.Domain;

namespace ESS.Domain.Common.Association.Domain
{
    public class Association
    {
        public Guid Id { get; set; }
        public Entity EntityFrom { get; set; }
        public Entity EntityTo { get; set; }
        public PartyRole.Domain.PartyRole PartyRoleFrom { get; set; }
        public PartyRole.Domain.PartyRole PartyRoleTo { get; set; }
        public StatusType StatusTypeFrom { get; set; }
        public StatusType StatusTypeTo { get; set; }
        public AssociationRule EntityAssociationRule { get; set; }
        public AssociationType EntityAssociationType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
