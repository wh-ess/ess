#region

using System.Collections;
using AutoMapper;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.Domain
{
    public class Entity
        : Aggregate, IHandleCommand<CreateEntity>, IHandleCommand<EditEntity>, IHandleCommand<DeleteEntity>, IApplyEvent<EntityCreated>,
            IApplyEvent<EntityEdited>, IApplyEvent<EntityDeleted>
    {
        #region handle

        public IEnumerable Handle(CreateEntity c)
        {
            var e = Mapper.DynamicMap<CreateEntity, EntityCreated>(c);
            yield return e;
        }

        public IEnumerable Handle(DeleteEntity c)
        {
            yield return new EntityDeleted { Id = c.Id };
        }

        public IEnumerable Handle(EditEntity c)
        {
            var e = Mapper.DynamicMap<EditEntity, EntityEdited>(c);
            yield return e;
        }

        #endregion

        #region apply

        public void Apply(EntityCreated e)
        {
        }

        public void Apply(EntityDeleted e)
        {
        }

        public void Apply(EntityEdited e)
        {
        }

        #endregion
    }
}