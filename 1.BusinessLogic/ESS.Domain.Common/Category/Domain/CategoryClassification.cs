#region

using System;

#endregion

namespace ESS.Domain.Common.Category.Domain
{
    public class CategoryClassification
    {
        public Guid Id { get; set; }
        public Guid RelateId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}