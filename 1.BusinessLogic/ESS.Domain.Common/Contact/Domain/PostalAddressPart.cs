using System;

namespace ESS.Domain.Common.Contact.Domain
{
    public class PostalAddressPart
    {
        public Guid Id { get; set; }
        public Common.Contact.Domain.Contact Contact { get; set; }
        public PostalAddressPartType PostalAddressPartType { get; set; }

        public GeographicBoundary GeographicBoundary { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Text { get; set; }

    }
}
