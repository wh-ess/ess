using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Status.Commands;
using ESS.Domain.Common.Status.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Status.Domain
{
    public class StatusType : Aggregate, IHandleCommand<CreateStatusType>, IHandleCommand<ChangeStatusTypeName>, IHandleCommand<DeleteStatusType>,
            IApplyEvent<StatusTypeCreated>, IApplyEvent<StatusTypeNameChanged>, IApplyEvent<StatusTypeDeleted>
    {
        public string Name { get; set; }
        #region handle

        public IEnumerable Handle(CreateStatusType c)
        {
            var item = Mapper.DynamicMap<StatusTypeCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteStatusType c)
        {
            var item = Mapper.DynamicMap<StatusTypeDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeStatusTypeName c)
        {
            var item = Mapper.DynamicMap<StatusTypeNameChanged>(c);
            yield return item;
        }


        #endregion

        #region apply

        public void Apply(StatusTypeCreated e)
        {
        }

        public void Apply(StatusTypeDeleted e)
        {
        }

        public void Apply(StatusTypeNameChanged e)
        {
        }


        #endregion
    }
}
