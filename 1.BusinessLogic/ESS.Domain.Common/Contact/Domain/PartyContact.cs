#region

using System;
using ESS.Domain.Common.PartyRole.Domain;

#endregion

namespace ESS.Domain.Common.Contact.Domain
{
    public class PartyContact
    {
        public Guid Id { get; set; }
        public Guid PartyId { get; set; }
        public Guid ContactId { get; set; }
    }
}