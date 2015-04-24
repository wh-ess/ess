using System;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Common.Contact.Commands
{

    public class CreateContact : Command
    {
        public string CountryCode;
        public string AreaCode;
        public string PhoneNumber;
        public string Address;

    }

    public class ChangeContact : Command
    {
        public string CountryCode;
        public string AreaCode;
        public string PhoneNumber;
        public string Address;
    }
    public class DeleteContact : Command
    {
    }
}
