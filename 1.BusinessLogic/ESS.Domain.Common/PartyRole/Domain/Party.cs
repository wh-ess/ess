using System;

namespace ESS.Domain.Common.PartyRole.Domain
{
    /// <summary>
    /// The Data Model Resource Book Vol.3
    /// 人或组织
    /// </summary>
    public class Party
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        //身份证号(新)
        public string IcNo { get; set; }
        //身份证号（旧）
        public string IcNoOld { get; set; }
        //护照号
        public string Passport { get; set; }
        //员工相片路径
        public string Photo { get; set; }

    }

}
