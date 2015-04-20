using System;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRuleFactorType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BusinessRuleFactorType Parent { get; set; }
    }
}
