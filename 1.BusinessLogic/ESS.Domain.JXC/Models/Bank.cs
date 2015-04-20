using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class Bank
    {
        public Bank()
        {
            ReceiveMasters = new List<ReceiveMaster>();
        }

        public int BankId { get; set; }
        public string BankCode { get; set; }
        public string BankAttribName { get; set; }
        public string BankName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<ReceiveMaster> ReceiveMasters { get; set; }
    }
}