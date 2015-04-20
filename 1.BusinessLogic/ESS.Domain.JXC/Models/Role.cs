using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class Role
    {
        public Role()
        {
            Privileges = new List<Privilege>();
            Users = new List<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<Privilege> Privileges { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}