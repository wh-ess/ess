using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class Brand
    {
        public Brand()
        {
            Products = new List<Product>();
        }

        public int BrandId { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public int? BrandParentId { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}