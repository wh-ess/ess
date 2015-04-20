using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Basic.Events
{
    public class BrandTypeCreated : Event
    {
        public string Name;
    }

    public class BrandTypeEdited : BrandTypeCreated
    {

    }

    public class BrandTypeDeleted : Event
    {

    }
}
