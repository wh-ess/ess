using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class User
    {
        public User()
        {
            Favorites = new List<Favorite>();
            Roles = new List<Role>();
        }

        public int UserId { get; set; }
        public string UserNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Locked { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}