using System;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRuleRelation
    {
        public Guid Id { get; set; }
        public Guid RelateId { get; set; }
        public Guid BusinessRuleId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
