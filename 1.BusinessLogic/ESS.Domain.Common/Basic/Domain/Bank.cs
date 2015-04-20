using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Basic.Commands;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Basic.Domain
{
    public class Bank : Aggregate, IHandleCommand<CreateBank>, IHandleCommand<EditBank>, IHandleCommand<DeleteBank>, IApplyEvent<BankCreated>,
            IApplyEvent<BankEdited>, IApplyEvent<BankDeleted>
    {
        #region handle

        public IEnumerable Handle(CreateBank c)
        {
            var item = Mapper.DynamicMap<BankCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteBank c)
        {
            var item = Mapper.DynamicMap<BankDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(EditBank c)
        {
            var item = Mapper.DynamicMap<BankEdited>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(BankCreated e)
        {
        }

        public void Apply(BankDeleted e)
        {
        }

        public void Apply(BankEdited e)
        {
        }

        #endregion
    }
}
