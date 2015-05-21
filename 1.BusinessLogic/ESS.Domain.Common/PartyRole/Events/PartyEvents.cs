#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.PartyRole.Events
{
    public class PartyCreated : Event
    {
        public string Name ;
        public string Photo ;
    }
    public class PartyNameChanged : Event
    {
        public string Name;

    }
    public class PartyPhotoChanged : Event
    {
        public string Photo;

    }

    public class PartyDeleted : Event
    {
    }
}