using System;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRuleOutCome
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Common.BusinessRule.Domain.BusinessRule BusinessRule { get; set; }
        public Guid TypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Desc { get; set; }
    }
}
