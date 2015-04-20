using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class DeliveryMaster
    {
        public DeliveryMaster()
        {
            DeliveryDetails = new List<DeliveryDetail>();
            ReceiveDetails = new List<ReceiveDetail>();
        }

        public string DeliveryId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int CustomerId { get; set; }
        public int? SalesManId { get; set; }
        public string DeliveryProperty { get; set; }
        public string DeliveryAddress { get; set; }
        public string InvoiceNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ValueAddTax { get; set; }
        public decimal Amount { get; set; }
        public decimal Receivable { get; set; }
        public decimal Received { get; set; }
        public DateTime? LimitDate { get; set; }
        public string CarNo { get; set; }
        public string Tel { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SalesMan SalesMan { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; }
        public virtual ICollection<ReceiveDetail> ReceiveDetails { get; set; }
    }
}