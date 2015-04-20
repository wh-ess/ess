using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class SalesMan
    {
        public SalesMan()
        {
            Customers = new List<Customer>();
            DeliveryMasters = new List<DeliveryMaster>();
        }

        public int SalesManId { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string ContactPhone { get; set; }
        public string MobilePhone { get; set; }
        public string ContactAddress { get; set; }
        public string EMail { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<DeliveryMaster> DeliveryMasters { get; set; }
    }
}