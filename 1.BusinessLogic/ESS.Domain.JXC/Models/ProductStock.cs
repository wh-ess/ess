using System;

namespace ESS.Domain.JXC.Models
{
    public class ProductStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BatchNo { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public string OrderId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateById { get; set; }
        public virtual Product Product { get; set; }
    }
}