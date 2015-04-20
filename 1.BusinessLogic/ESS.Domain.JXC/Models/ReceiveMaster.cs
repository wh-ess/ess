using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class ReceiveMaster
    {
        public ReceiveMaster()
        {
            ReceiveDetails = new List<ReceiveDetail>();
        }

        public string ReceiveId { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int CustomerId { get; set; }
        public decimal ReceiveCash { get; set; }
        public decimal ReceiveCheck { get; set; }
        public string NoteNo { get; set; }
        public DateTime? DueDate { get; set; }
        public int? BankId { get; set; }
        public string AccountNo { get; set; }
        public decimal Discount { get; set; }
        public decimal Remittance { get; set; }
        public decimal AdvancePay { get; set; }
        public decimal Others { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal AccountAmt { get; set; }
        public string Type { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<ReceiveDetail> ReceiveDetails { get; set; }
    }
}