using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Basic.Events
{
    public class BrandCreated : Event
    {
        public string Name;
    }

    public class BrandEdited : BrandCreated
    {

    }

    public class BrandDeleted : Event
    {

    }
}
