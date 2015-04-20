using System;

namespace ESS.Domain.JXC.Models
{
    public class Config
    {
        public int ConfigId { get; set; }
        public string Name { get; set; }
        public string Val { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}