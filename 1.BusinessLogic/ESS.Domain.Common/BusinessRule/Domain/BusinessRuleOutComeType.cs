using System;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRuleOutComeType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BusinessRuleOutComeType Parent { get; set; }
    }
}
