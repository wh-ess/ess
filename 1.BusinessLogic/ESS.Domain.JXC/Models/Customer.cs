using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class Customer
    {
        public Customer()
        {
            DeliveryMasters = new List<DeliveryMaster>();
            ReceiveMasters = new List<ReceiveMaster>();
        }

        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerAttribName { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNo { get; set; }
        public string Owner { get; set; }
        public string RocId { get; set; }
        public string ContactMan1 { get; set; }
        public string ContactMan2 { get; set; }
        public string ContactPhone1 { get; set; }
        public string ContactPhone2 { get; set; }
        public string Fax { get; set; }
        public string CustomerAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public decimal? PayDays { get; set; }
        public decimal? CreditLine { get; set; }
        public decimal? CreditBalance { get; set; }
        public DateTime? LastDeliveryDate { get; set; }
        public decimal? Advance { get; set; }
        public string CarNo { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? SalesMan_SalesManId { get; set; }
        public virtual SalesMan SalesMan { get; set; }
        public virtual ICollection<DeliveryMaster> DeliveryMasters { get; set; }
        public virtual ICollection<ReceiveMaster> ReceiveMasters { get; set; }
    }
}