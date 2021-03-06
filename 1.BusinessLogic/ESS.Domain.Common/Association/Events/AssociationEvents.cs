﻿#region

using System;
using ESS.Domain.Common.Association.Domain;
using ESS.Domain.Common.Category.ReadModels;
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
        public CategoryTypeItem Type;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Code;
        public bool IsSystem;
    }

    public class AssociationEdited : AssociationCreated
    {
        
    }


    public class AssociationDeleted : Event
    {
    }
}