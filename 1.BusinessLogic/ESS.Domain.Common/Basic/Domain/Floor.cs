#region

using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Basic.Commands;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Basic.Domain
{
    public class Floor
        : Aggregate, IHandleCommand<CreateFloor>, IHandleCommand<EditFloor>, IHandleCommand<DeleteFloor>, IApplyEvent<FloorCreated>,
            IApplyEvent<FloorEdited>, IApplyEvent<FloorDeleted>
    {
        #region handle

        public IEnumerable Handle(CreateFloor c)
        {
            var item = Mapper.DynamicMap<FloorCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteFloor c)
        {
            var item = Mapper.DynamicMap<FloorDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(EditFloor c)
        {
            var item = Mapper.DynamicMap<FloorEdited>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(FloorCreated e)
        {
        }

        public void Apply(FloorDeleted e)
        {
        }

        public void Apply(FloorEdited e)
        {
        }

        #endregion
    }
}