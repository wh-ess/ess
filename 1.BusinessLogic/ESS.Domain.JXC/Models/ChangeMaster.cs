using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class ChangeMaster
    {
        public ChangeMaster()
        {
            ChangeDetails = new List<ChangeDetail>();
        }

        public string ChangeId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<ChangeDetail> ChangeDetails { get; set; }
    }
}