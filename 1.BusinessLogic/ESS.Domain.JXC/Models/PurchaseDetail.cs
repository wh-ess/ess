namespace ESS.Domain.JXC.Models
{
    public class PurchaseDetail
    {
        public string PurchaseId { get; set; }
        public int ProductId { get; set; }
        public string BatchNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string PurchaseMaster_PurchaseId { get; set; }
        public virtual Product Product { get; set; }
        public virtual PurchaseMaster PurchaseMaster { get; set; }
    }
}