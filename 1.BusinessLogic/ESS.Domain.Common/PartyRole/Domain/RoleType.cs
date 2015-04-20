using System;

namespace ESS.Domain.Common.PartyRole.Domain
{
    public class RoleType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RoleType Parent { get; set; }
    }
}
