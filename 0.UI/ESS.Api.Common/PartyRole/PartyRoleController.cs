#region

using System;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Common.PartyRole.Commands;
using ESS.Domain.Common.PartyRole.ReadModels;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Common.PartyRole
{
    public class PartyRoleController : ApiController
    {
        private readonly PartyRoleView _partyRoleView;
        private readonly MessageDispatcher _messageDispatcher;

        public PartyRoleController(MessageDispatcher messageDispatcher, PartyRoleView partyRoleView)
        {
            _messageDispatcher = messageDispatcher;
            _partyRoleView = partyRoleView;
        }

        public IEnumerable<PartyRoleItem> Get()
        {
            return _partyRoleView.PartyRoleList();
        }

        public PartyRoleItem Get(Guid id)
        {
            return _partyRoleView.GetPartyRole(id);
        }

        [HttpPost]
        public IHttpActionResult CreatePartyRole(CreatePartyRole partyRole)
        {
            _messageDispatcher.SendCommand(partyRole);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditPartyRole(EditPartyRole partyRole)
        {
            _messageDispatcher.SendCommand(partyRole);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeletePartyRole(Guid id)
        {
            _messageDispatcher.SendCommand(new DeletePartyRole(){Id=id});
            return Ok();
        }
    }
}