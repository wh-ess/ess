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
    /// contexual role
    /// 一般和SpecificContexualRole 2选1
    /// </summary>
    public class GenericContexualRole: Aggregate, IHandleCommand<CreateGenericContexualRole>, IHandleCommand<DeleteGenericContexualRole>, IApplyEvent<GenericContexualRoleCreated>, IApplyEvent<GenericContexualRoleDeleted>
    {
        public Guid PartyId { get; set; }
        public Guid EntityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        #region handle

        public IEnumerable Handle(CreateGenericContexualRole c)
        {
            var item = Mapper.DynamicMap<GenericContexualRoleCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteGenericContexualRole c)
        {
            var item = Mapper.DynamicMap<GenericContexualRoleDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(GenericContexualRoleCreated e)
        {
        }

        public void Apply(GenericContexualRoleDeleted e)
        {
        }

        #endregion
    }
}
