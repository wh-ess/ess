using System;
using ESS.Domain.Common.Contact.Domain;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRule
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Statement { get; set; }
        public GeographicBoundary GeographicBoundary { get; set; }
    }
}
