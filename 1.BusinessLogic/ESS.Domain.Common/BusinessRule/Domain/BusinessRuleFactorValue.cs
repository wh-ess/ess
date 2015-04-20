using System;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRuleFactorValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BusinessRuleFactor BusinessRuleFactor { get; set; }
        public BusinessRuleFactorValueType BusinessRuleFactorValueType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Value { get; set; }
    }
}
