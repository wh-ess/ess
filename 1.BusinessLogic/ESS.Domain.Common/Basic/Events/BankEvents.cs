using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Basic.Events
{
    public class BankCreated : Event
    {
        public string Code;
        public string Name;
        public string ShortName;
        public bool IsActive;
    }

    public class BankEdited : BankCreated
    {

    }

    public class BankDeleted : Event
    {

    }
}
