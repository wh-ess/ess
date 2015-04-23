using System;

namespace ESS.Domain.Common.PartyRole.Domain
{
    /// <summary>
    /// 业务和交易等,如订单,项目
    /// </summary>
    public class Entity
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid PartyRoleId { get; set; }
    }
}
