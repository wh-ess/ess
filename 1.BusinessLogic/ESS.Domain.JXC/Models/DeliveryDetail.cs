namespace ESS.Domain.JXC.Models
{
    public class DeliveryDetail
    {
        public string DeliveryId { get; set; }
        public int ProductId { get; set; }
        public string BatchNo { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnSent { get; set; }
        public decimal? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string DeliveryMaster_DeliveryId { get; set; }
        public virtual Product Product { get; set; }
        public virtual DeliveryMaster DeliveryMaster { get; set; }
    }
}