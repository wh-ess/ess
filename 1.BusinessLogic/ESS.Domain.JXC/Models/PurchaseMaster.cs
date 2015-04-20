using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class PurchaseMaster
    {
        public PurchaseMaster()
        {
            PaymentDetails = new List<PaymentDetail>();
            PurchaseDetails = new List<PurchaseDetail>();
        }

        public string PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int SupplierId { get; set; }
        public string PurchaseProperty { get; set; }
        public string InvoiceNo { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ValueAddTax { get; set; }
        public decimal Amount { get; set; }
        public decimal Payable { get; set; }
        public decimal Paid { get; set; }
        public DateTime? LimitDate { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}