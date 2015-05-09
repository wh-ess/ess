#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.PartyRole.Events
{
    public class PartyCreated : Event
    {
        public string Name ;
        public DateTime BirthDay ;
        //员工相片路径
        public string Photo ;
    }
    public class PartyEdited : PartyCreated
    {

    }

    public class PartyDeleted : Event
    {
    }
}