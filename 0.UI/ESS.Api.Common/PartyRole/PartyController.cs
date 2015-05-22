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
    public class PartyController : ApiController
    {
        private readonly PartyView _partyView;
        private readonly MessageDispatcher _messageDispatcher;

        public PartyController(MessageDispatcher messageDispatcher, PartyView partyView)
        {
            _messageDispatcher = messageDispatcher;
            _partyView = partyView;
        }

        public IEnumerable<PartyItem> Get()
        {
            return _partyView.PartyList();
        }

        public PartyItem Get(Guid id)
        {
            return _partyView.GetParty(id);
        }

        [HttpPost]
        public IHttpActionResult CreateParty(CreateParty party)
        {
            _messageDispatcher.SendCommand(party);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditParty(ChangePartyName party)
        {
            _messageDispatcher.SendCommand(party);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteParty(Guid id)
        {
            _messageDispatcher.SendCommand(new DeleteParty(){Id=id});
            return Ok();
        }
    }
}