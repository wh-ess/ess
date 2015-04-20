using System;

namespace ESS.Domain.Common.PartyRole.Domain
{
    /// <summary>
    /// contexual role 
    /// 一般和GenericContexualRole 2选1
    /// </summary>
    public class SpecificContexualRole
    {
        public Guid Id { get; set; }
        public RoleType RoleType { get; set; }
        public Entity Entity { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
