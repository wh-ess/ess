namespace ESS.Domain.JXC.Models
{
    public class ChangeDetail
    {
        public string ChangeId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string ChangeMaster_ChangeId { get; set; }
        public virtual ChangeMaster ChangeMaster { get; set; }
    }
}