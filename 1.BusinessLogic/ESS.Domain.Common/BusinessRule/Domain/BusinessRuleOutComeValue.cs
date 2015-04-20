﻿using System;

namespace ESS.Domain.Common.BusinessRule.Domain
{
    public class BusinessRuleOutComeValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BusinessRuleOutCome BusinessRuleOutCome { get; set; }
        public BusinessRuleOutComeValueType BusinessRuleOutComeValueType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Value { get; set; }
    }
}
