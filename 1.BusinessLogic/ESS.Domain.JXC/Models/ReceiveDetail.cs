namespace ESS.Domain.JXC.Models
{
    public class ReceiveDetail
    {
        public string ReceiveId { get; set; }
        public string DeliveryId { get; set; }
        public decimal Balance { get; set; }
        public string ReceiveMaster_ReceiveId { get; set; }
        public virtual DeliveryMaster DeliveryMaster { get; set; }
        public virtual ReceiveMaster ReceiveMaster { get; set; }
    }
}