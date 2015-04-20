#region

using System;
using ESS.Domain.Common.PartyRole.Domain;

#endregion

namespace ESS.Domain.Common.Contact.Domain
{
    public class PartyContact
    {
        public Guid Id { get; set; }
        public Party Party { get; set; }
        public Contact Contact { get; set; }
    }
}