using System;
using System.Collections;
using ESS.Framework.CQRS.Event;

namespace ESS.Framework.CQRS.Domain
{
    /// <summary>
    /// Aggregate base class, which factors out some common infrastructure that
    /// all aggregates have (ID and event application).
    /// </summary>
    public abstract class Aggregate : IAggregate
    {
        /// <summary>
        /// The unique ID of the aggregate.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The number of events loaded into this aggregate.
        /// </summary>
        public int Version { get; private set; }


        /// <summary>
        /// Enuerates the supplied events and applies them in order to the aggregate.
        /// </summary>
        /// <param name="events"></param>
        public void ApplyEvents(IEnumerable events)
        {
            foreach (var e in events)
                GetType().GetMethod("ApplyOneEvent")
                    .MakeGenericMethod(e.GetType())
                    .Invoke(this, new object[] { e });
        }

        /// <summary>
        /// Applies a single event to the aggregate.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="ev"></param>
        public void ApplyOneEvent<TEvent>(TEvent ev)
        {
            var applier = this as IApplyEvent<TEvent>;
            if (applier == null)
                throw new InvalidOperationException(string.Format(
                    "Aggregate {0} does not know how to apply event {1}",
                    GetType().Name, ev.GetType().Name));
            applier.Apply(ev);
            Version++;
        }
    }
}
