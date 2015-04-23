﻿#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Framework.CQRS.Command;

#endregion

namespace ESS.Domain.Common.PartyRole.Commands
{
    public class CreateParty : Command
    {
        public string Name ;
        public string FullName ;
        public string FirstName ;
        public string LastName ;
        public DateTime BirthDay ;
        //身份证号(新)
        public string IcNo ;
        //身份证号（旧）
        public string IcNoOld ;
        //护照号
        public string Passport ;
        //员工相片路径
        public string Photo ;
    }


    public class DeleteParty : Command
    {
    }
}