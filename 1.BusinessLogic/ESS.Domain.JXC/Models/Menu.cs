namespace ESS.Domain.JXC.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuNo { get; set; }
        public string Title { get; set; }
        public int? MenuParentId { get; set; }
        public string Url { get; set; }
        public bool? Enabled { get; set; }
        public int? Seq { get; set; }
        public string Icon { get; set; }
    }
}