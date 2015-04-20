using System;

namespace ESS.Domain.Common.PartyRole.Domain
{
    public class EntityType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EntityType Parent { get; set; }
    }
}
