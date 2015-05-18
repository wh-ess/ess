using System;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Contact.Events
{

    public class GeographicBoundaryCreated : Event
    {
        public Guid TypeId ;
        public string Name ;
        public string Code ;
        //缩写
        public string Abbr ;
        public string InternetRegionCode ;

    }

    public class GeographicBoundaryChanged : GeographicBoundaryCreated
    {
    }
    public class GeographicBoundaryDeleted : Event
    {
    }
}
