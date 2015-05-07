#region

using System;
using ESS.Framework.Common.Utilities;

#endregion

namespace ESS.Framework.CQRS.Event
{
    public abstract class Event : IEvent
    {
        protected Event()
        {
            Id = ObjectId.GetNextGuid();
            Date = DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime Date { get; private set; }
    }
}