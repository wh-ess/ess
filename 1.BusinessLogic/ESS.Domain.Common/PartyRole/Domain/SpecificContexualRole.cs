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
    /// 一般和GenericContexualRole 2选1
    /// </summary>
    public class SpecificContexualRole: Aggregate, IHandleCommand<CreateSpecificContexualRole>, IHandleCommand<DeleteSpecificContexualRole>, IApplyEvent<SpecificContexualRoleCreated>, IApplyEvent<SpecificContexualRoleDeleted>
    {
        public Guid RoleTypeId { get; set; }
        public Guid EntityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        #region handle

        public IEnumerable Handle(CreateSpecificContexualRole c)
        {
            var item = Mapper.DynamicMap<SpecificContexualRoleCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteSpecificContexualRole c)
        {
            var item = Mapper.DynamicMap<SpecificContexualRoleDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(SpecificContexualRoleCreated e)
        {
        }

        public void Apply(SpecificContexualRoleDeleted e)
        {
        }

        #endregion
    
    }
}
