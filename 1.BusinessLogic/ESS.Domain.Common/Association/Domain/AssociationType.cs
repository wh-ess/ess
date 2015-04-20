using System;

namespace ESS.Domain.Common.Association.Domain
{
    public class AssociationType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AssociationType Parent { get; set; }
    }
}
