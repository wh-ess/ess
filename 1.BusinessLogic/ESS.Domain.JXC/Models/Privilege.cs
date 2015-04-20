using System;

namespace ESS.Domain.JXC.Models
{
    public class Privilege
    {
        public int RoleId { get; set; }
        public string ModuleNo { get; set; }
        public string ActionName { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual Role Role { get; set; }
    }
}