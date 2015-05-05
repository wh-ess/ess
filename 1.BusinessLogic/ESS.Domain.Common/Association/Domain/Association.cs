#region

using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Association.Commands;
using ESS.Domain.Common.Association.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Association.Domain
{
    public class Association
        : Aggregate, IHandleCommand<CreateAssociation>,IHandleCommand<EditAssociation>, IHandleCommand<DeleteAssociation>, IApplyEvent<AssociationCreated>,
            IApplyEvent<AssociationDeleted>,IApplyEvent<AssociationEdited>
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public AssociationRule AssociationRule { get; set; }
        public Guid AssociationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        #region handle

        public IEnumerable Handle(CreateAssociation c)
        {
            var item = Mapper.DynamicMap<AssociationCreated>(c);
            yield return item;
        }
        public IEnumerable Handle(EditAssociation c)
        {
            var item = Mapper.DynamicMap<AssociationEdited>(c);
            yield return item;
        }
        public IEnumerable Handle(DeleteAssociation c)
        {
            var item = Mapper.DynamicMap<AssociationDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(AssociationCreated e)
        {
        }
        public void Apply(AssociationEdited e)
        {
        }
        public void Apply(AssociationDeleted e)
        {
        }

        #endregion
    }
}