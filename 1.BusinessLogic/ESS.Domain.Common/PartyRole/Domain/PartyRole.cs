#region

using System;
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
    ///     PartyRole角色,如客户,供应商
    /// </summary>
    public class PartyRole : Aggregate, IHandleCommand<CreatePartyRole>, IHandleCommand<EditPartyRole>, IHandleCommand<DeletePartyRole>,
        IApplyEvent<PartyRoleCreated>, IApplyEvent<PartyRoleEdited>, IApplyEvent<PartyRoleDeleted>
    {
        public Guid PartyRoleId { get; set; }
        public Guid TypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        #region handle

        public IEnumerable Handle(CreatePartyRole c)
        {
            var item = Mapper.DynamicMap<PartyRoleCreated>(c);
            yield return item;
        }
        public IEnumerable Handle(EditPartyRole c)
        {
            var item = Mapper.DynamicMap<PartyRoleEdited>(c);
            yield return item;
        }
        public IEnumerable Handle(DeletePartyRole c)
        {
            var item = Mapper.DynamicMap<PartyRoleDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(PartyRoleCreated e)
        {
        }
        public void Apply(PartyRoleEdited e)
        {
        }
        public void Apply(PartyRoleDeleted e)
        {
        }

        #endregion
    
    }
}