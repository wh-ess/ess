#region

using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Status.Commands;
using ESS.Domain.Common.Status.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Status.Domain
{
    public class Status
        : Aggregate, IHandleCommand<CreateStatus>, IHandleCommand<DeleteStatus>, IApplyEvent<StatusCreated>,
            IApplyEvent<StatusDeleted>
    {
        public Guid RelateId { get; set; }
        public Guid TypeId { get; set; }
        public DateTime StatusDateTime { get; set; }
        public DateTime StatusFromDate { get; set; }
        public DateTime StatusEndDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        #region handle

        public IEnumerable Handle(CreateStatus c)
        {
            var item = Mapper.DynamicMap<StatusCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteStatus c)
        {
            var item = Mapper.DynamicMap<StatusDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(StatusCreated e)
        {
        }

        public void Apply(StatusDeleted e)
        {
        }

        #endregion
    }
}