using System;

namespace ESS.Domain.Common.PartyRole.Domain
{
    /// <summary>
    /// contexual role
    /// 一般和SpecificContexualRole 2选1
    /// </summary>
    public class GenericContexualRole
    {
        public Guid Id { get; set; }
        public Guid PartyId { get; set; }
        public Guid EntityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
