namespace ESS.Domain.JXC.Models
{
    public class PaymentDetail
    {
        public string PaymentId { get; set; }
        public string PurchaseId { get; set; }
        public decimal Balance { get; set; }
        public string PaymentMaster_PaymentId { get; set; }
        public virtual PaymentMaster PaymentMaster { get; set; }
        public virtual PurchaseMaster PurchaseMaster { get; set; }
    }
}