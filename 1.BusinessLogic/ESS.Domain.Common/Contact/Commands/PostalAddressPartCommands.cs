using System;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Contact.Commands
{

    public class CreatePostalAddressPart : Command
    {
        public Guid ContactId ;
        public Guid TypeId ;
        public Guid GeographicBoundaryId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
        public string Text ;

    }

    public class ChangePostalAddressPart : Command
    {
        public Guid ContactId ;
        public Guid TypeId ;
        public Guid GeographicBoundaryId ;
        public DateTime FromDate ;
        public DateTime EndDate ;
        public string Text ;
    }
    public class DeletePostalAddressPart : Command
    {
    }
}
