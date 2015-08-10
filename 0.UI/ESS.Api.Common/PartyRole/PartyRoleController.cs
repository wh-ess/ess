#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly DefaultMessageBus _messageDispatcher;

        public PartyRoleController(DefaultMessageBus messageDispatcher, PartyRoleView partyRoleView)
        {
            _messageDispatcher = messageDispatcher;
            _partyRoleView = partyRoleView;
        }

        public Task<IEnumerable<PartyRoleItem>> Get()
        {
            return _partyRoleView.PartyRoleList();
        }

        public Task<PartyRoleItem> Get(Guid id)
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