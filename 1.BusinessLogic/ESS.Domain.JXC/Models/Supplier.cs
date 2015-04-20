using System;
using System.Collections.Generic;

namespace ESS.Domain.JXC.Models
{
    public class Supplier
    {
        public Supplier()
        {
            PaymentMasters = new List<PaymentMaster>();
            PurchaseMasters = new List<PurchaseMaster>();
        }

        public int SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierAttribName { get; set; }
        public string SupplierName { get; set; }
        public string InvoiceNo { get; set; }
        public string Owner { get; set; }
        public string RocId { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string ContactName1 { get; set; }
        public string ContactName2 { get; set; }
        public string CompanyAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal? PayDays { get; set; }
        public decimal? Prepaid { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyDate { get; set; }
        public virtual ICollection<PaymentMaster> PaymentMasters { get; set; }
        public virtual ICollection<PurchaseMaster> PurchaseMasters { get; set; }
    }
}