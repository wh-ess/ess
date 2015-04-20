using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class PaymentMaster
    {
        public PaymentMaster()
        {
            PaymentDetails = new List<PaymentDetail>();
        }

        public string PaymentId { get; set; }
        public DateTime PayDate { get; set; }
        public int SupplierId { get; set; }
        public decimal PayCash { get; set; }
        public decimal PayCheck { get; set; }
        public string NoteNo { get; set; }
        public DateTime? DueDate { get; set; }
        public int? BankId { get; set; }
        public string AccountNo { get; set; }
        public decimal AccountAmt { get; set; }
        public decimal Discount { get; set; }
        public decimal Remittance { get; set; }
        public decimal Prepayment { get; set; }
        public decimal Others { get; set; }
        public decimal PayAmount { get; set; }
        public decimal TotalBalance { get; set; }
        public string Type { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}