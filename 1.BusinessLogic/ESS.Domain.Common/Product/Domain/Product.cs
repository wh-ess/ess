using System;

namespace ESS.Domain.Common.Product.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Product
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        //缩写
        public string Abbr { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ParentId { get; set; }
        public Guid PartyRoleId { get; set; }
    }
}
