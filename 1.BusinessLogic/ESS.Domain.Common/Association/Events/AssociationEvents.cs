#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Domain.Common.PartyRole.ReadModels;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Association.Events
{
    public class AssociationCreated : Event
    {
        public Guid From;
        public Guid To;
        public AssociationRule AssociationRule;
        public Guid TypeId;
        public DateTime FromDate;
        public DateTime EndDate;
    }

    public class AssociationEdited : AssociationCreated
    {
        
    }


    public class AssociationDeleted : Event
    {
    }
}