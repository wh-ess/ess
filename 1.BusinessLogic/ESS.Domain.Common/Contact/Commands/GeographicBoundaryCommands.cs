using System;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Contact.Commands
{

    public class CreateGeographicBoundary : Command
    {
        public Guid TypeId ;
        public string Name ;
        public string Code ;
        //缩写
        public string Abbreviation ;
        public string InternetRegionCode ;

    }

    public class ChangeGeographicBoundary : Command
    {
        public Guid TypeId;
        public string Name;
        public string Code;
        //缩写
        public string Abbreviation;
        public string InternetRegionCode;
    }
    public class DeleteGeographicBoundary : Command
    {
    }
}
