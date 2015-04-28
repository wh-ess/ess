using System;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRuleOutComeValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid BusinessRuleOutComeId { get; set; }
        public Guid TypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Value { get; set; }
    }
}
