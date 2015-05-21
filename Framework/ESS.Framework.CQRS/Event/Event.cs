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
            EventDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime EventDate { get; private set; }
    }
}