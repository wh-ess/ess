using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Association.Commands;
using ESS.Domain.Common.Association.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Association.Domain
{
    public class AssociationType : Aggregate, IHandleCommand<CreateAssociationType>, IHandleCommand<ChangeAssociationTypeName>, IHandleCommand<DeleteAssociationType>,
            IHandleCommand<ChangeAssociationTypeParent>,
            IApplyEvent<AssociationTypeCreated>, IApplyEvent<AssociationTypeNameChanged>, IApplyEvent<AssociationTypeDeleted>,
            IApplyEvent<AssociationTypeParentChanged>
    {
        #region handle

        public IEnumerable Handle(CreateAssociationType c)
        {
            var item = Mapper.DynamicMap<AssociationTypeCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteAssociationType c)
        {
            var item = Mapper.DynamicMap<AssociationTypeDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeAssociationTypeName c)
        {
            var item = Mapper.DynamicMap<AssociationTypeNameChanged>(c);
            yield return item;
        }
        public IEnumerable Handle(ChangeAssociationTypeParent c)
        {
            var item = Mapper.DynamicMap<AssociationTypeParentChanged>(c);
            yield return item;
        }


        #endregion

        #region apply

        public void Apply(AssociationTypeCreated e)
        {
        }

        public void Apply(AssociationTypeDeleted e)
        {
        }

        public void Apply(AssociationTypeNameChanged e)
        {
        }
        public void Apply(AssociationTypeParentChanged e)
        {
        }


        #endregion



    }
}
