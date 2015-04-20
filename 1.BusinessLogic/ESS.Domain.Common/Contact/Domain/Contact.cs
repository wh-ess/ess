#region

using System;

#endregion

namespace ESS.Domain.Common.Contact.Domain
{
    public class Contact
    {
        public Guid Id { get; set; }
    }

    public class EmailAddress : Contact
    {
        public string Address { get; set; }
    }

    public class TeleNumber : Contact
    {
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
    }

    
}