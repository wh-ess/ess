#region

using System.Collections;
using AutoMapper;
using ESS.Domain.Common.PartyRole.Commands;
using ESS.Domain.Common.PartyRole.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.PartyRole.Domain
{
    /// <summary>
    ///     The Data Model Resource Book Vol.3
    ///     人或组织
    /// </summary>
    public class Party
        : Aggregate, IHandleCommand<CreateParty>, IHandleCommand<ChangePartyName>, IHandleCommand<ChangePartyPhoto>, IHandleCommand<DeleteParty>,
        IApplyEvent<PartyCreated>, IApplyEvent<PartyNameChanged>, IApplyEvent<PartyPhotoChanged>, IApplyEvent<PartyDeleted>
    {
        public string Name { get; private set; }
        public string Photo { get; private set; }

        #region handle

        public IEnumerable Handle(CreateParty c)
        {
            var item = Mapper.DynamicMap<PartyCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangePartyName c)
        {
            var item = Mapper.DynamicMap<PartyNameChanged>(c);
            yield return item;
        }
        public IEnumerable Handle(ChangePartyPhoto c)
        {
            var item = Mapper.DynamicMap<PartyPhotoChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteParty c)
        {
            var item = Mapper.DynamicMap<PartyDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(PartyCreated e)
        {
            Name = e.Name;
            Photo = e.Photo;
        }

        public void Apply(PartyNameChanged e)
        {
            Name = e.Name;
        }
        public void Apply(PartyPhotoChanged e)
        {
            Photo = e.Photo;
        }

        public void Apply(PartyDeleted e)
        {
        }

        #endregion
    }
}