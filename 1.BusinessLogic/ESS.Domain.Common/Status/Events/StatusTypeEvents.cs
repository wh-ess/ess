using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Status.Events
{
    public class StatusTypeCreated : Event
    {
        public string Name;

    }

    public class StatusTypeNameChanged : Event
    {
        public string Name;
    }
    public class StatusTypeDeleted : Event
    {
    }
}
