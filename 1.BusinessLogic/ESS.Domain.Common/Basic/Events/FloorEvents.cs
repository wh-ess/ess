using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Basic.Events
{
    public class FloorCreated : Event
    {
        public string Name;
    }

    public class FloorEdited : FloorCreated
    {

    }

    public class FloorDeleted : Event
    {

    }
}
