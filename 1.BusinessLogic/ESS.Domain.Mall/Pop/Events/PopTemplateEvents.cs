#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Mall.Pop.Events
{
    public class PopTemplateCreated : Event
    {
        public DateTime EndDate;
        public string Image;
        public bool IsEnable;
        public string Name;
        public int Seq;
        public DateTime StartDate;
    }

    public class PopTemplateEdited : PopTemplateCreated
    {
    }

    public class PopTemplateDeleted : Event
    {
    }
}