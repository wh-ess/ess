using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.PartyRole.Commands;
using ESS.Domain.Common.PartyRole.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.PartyRole.Domain
{
    /// <summary>
    /// The Data Model Resource Book Vol.3
    /// 人或组织
    /// </summary>
    public class Party : Aggregate, IHandleCommand<CreateParty>, IHandleCommand<DeleteParty>, IApplyEvent<PartyCreated>, IApplyEvent<PartyDeleted>
    {
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        //员工相片路径
        public string Photo { get; set; }

        #region handle

        public IEnumerable Handle(CreateParty c)
        {
            var item = Mapper.DynamicMap<PartyCreated>(c);
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
        }

        public void Apply(PartyDeleted e)
        {
        }

        #endregion
    }

}
