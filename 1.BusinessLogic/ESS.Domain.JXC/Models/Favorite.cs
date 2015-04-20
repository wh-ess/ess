using System;

namespace ESS.Domain.JXC.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime AddTime { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public virtual User User { get; set; }
    }
}