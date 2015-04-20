using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class Product
    {
        public Product()
        {
            DeliveryDetails = new List<DeliveryDetail>();
            ProductStocks = new List<ProductStock>();
            PurchaseDetails = new List<PurchaseDetail>();
        }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? SafeStock { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? ProductSpecification { get; set; }
        public string UnitStock { get; set; }
        public string UnitPackage { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public decimal? ProductWeight { get; set; }
        public string WeightUnit { get; set; }
        public string ProductUse { get; set; }
        public decimal? CostStandard { get; set; }
        public decimal? PriceStandard { get; set; }
        public decimal? PriceMember { get; set; }
        public decimal? PriceVip { get; set; }
        public decimal? PriceWholeSale { get; set; }
        public decimal? PriceOther { get; set; }
        public decimal? ActualStock { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? ProductClass_ClassId { get; set; }
        public int? Brand_BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ProductClass ProductClass { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}