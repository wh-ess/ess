#region

using System;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Association.Events
{
    public class AssociationTypeCreated : Event
    {
        public string Name;
        public Guid ParentId;
    }

    public class AssociationTypeNameChanged : Event
    {
        public string Name;
    }

    public class AssociationTypeParentChanged : Event
    {
        public Guid ParentId;
    }

    public class AssociationTypeDeleted : Event
    {
    }
}