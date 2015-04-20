using System;
using System.Collections;
using System.Collections.Generic;

namespace ESS.Framework.CQRS.Domain
{
    /// <summary>Represents an aggregate root.
    /// </summary>
    public interface IAggregate
    {
        Guid Id { get; set; }
        int Version { get; }

        void ApplyEvents(IEnumerable events);
        void ApplyOneEvent<TEvent>(TEvent ev);
    }
}
