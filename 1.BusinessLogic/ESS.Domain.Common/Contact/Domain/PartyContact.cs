#region

using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Contact.Events;
using ESS.Domain.Common.PartyContact.Commands;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Contact.Domain
{
    public class PartyPartyContact: Aggregate, IHandleCommand<CreatePartyContact>, IHandleCommand<ChangePartyContact>, IHandleCommand<DeletePartyContact>,
            IApplyEvent<PartyContactCreated>, IApplyEvent<PartyContactChanged>, IApplyEvent<PartyContactDeleted>
    {
        public Guid PartyId { get; set; }
        public Guid PartyContactId { get; set; }

        #region handle

        public IEnumerable Handle(CreatePartyContact c)
        {
            var item = Mapper.DynamicMap<PartyContactCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeletePartyContact c)
        {
            var item = Mapper.DynamicMap<PartyContactDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangePartyContact c)
        {
            var item = Mapper.DynamicMap<PartyContactChanged>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(PartyContactCreated e)
        {
        }

        public void Apply(PartyContactDeleted e)
        {
        }

        public void Apply(PartyContactChanged e)
        {
        }

        #endregion
    
    }
}