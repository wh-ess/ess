using System;

namespace ESS.Domain.Common.Contact.Domain
{
    public class GeographicBoundary
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        //缩写
        public string Abbreviation { get; set; }
        public string InternetRegionCode { get; set; }
    }


}

