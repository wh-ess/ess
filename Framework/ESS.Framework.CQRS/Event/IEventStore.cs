using System;
using System.Collections;

namespace ESS.Framework.CQRS.Event
{
    public interface IEventStore
    {
        IEnumerable LoadEventsFor<TAggregate>(Guid id);
        IEnumerable LoadEventsAll();
        void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, ArrayList newEvents);
    }
}
