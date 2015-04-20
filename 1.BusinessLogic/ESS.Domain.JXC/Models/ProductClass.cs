using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class ProductClass
    {
        public ProductClass()
        {
            Products = new List<Product>();
        }

        public int ClassId { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int? ClassParentId { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}