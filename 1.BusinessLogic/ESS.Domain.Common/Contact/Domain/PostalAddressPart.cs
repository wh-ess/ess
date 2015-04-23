using System;

namespace ESS.Domain.Common.Contact.Domain
{
    public class PostalAddressPart
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Guid TypeId { get; set; }

        public GeographicBoundary GeographicBoundary { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Text { get; set; }

    }
}
