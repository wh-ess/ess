#region

using System;

#endregion

namespace ESS.Domain.Common.PartyRole.Domain
{
    /// <summary>
    ///     party角色,如客户,供应商
    /// </summary>
    public class PartyRole
    {
        public Guid Id { get; set; }
        public Party Party { get; set; }
        public RoleType RoleType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}