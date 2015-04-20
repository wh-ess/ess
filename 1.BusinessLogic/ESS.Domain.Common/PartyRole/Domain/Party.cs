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
    }

    public class Organization : Party
    {
        public string Name { get; set; }
    }

    public class Persion : Party
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Sex { get; set; }
        public DateTime BirthDay { get; set; }
        //身份证号(新)
        public string IcNo { get; set; }
        //身份证号（旧）
        public string IcNoOld { get; set; }
        //护照号
        public string Passport { get; set; }
        //籍贯
        public string NativePlace { get; set; }
        //民族
        public string Nation { get; set; }
        //种族
        public string NationName { get; set; }
        //宗教信仰
        public string Religion { get; set; }
        //员工相片路径
        public string Photo { get; set; }
        //婚姻状态
        public string IsMarry { get; set; }
        //国籍
        public string Country { get; set; }
        //户籍
        public string Census { get; set; }

        public Contact.Domain.Contact Contact { get; set; }

    }
}
