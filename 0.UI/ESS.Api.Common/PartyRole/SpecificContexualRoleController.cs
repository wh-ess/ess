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
    public class SpecificContexualRoleController : ApiController
    {
        private readonly SpecificContexualRoleView _specificContexualRoleView;
        private readonly MessageDispatcher _messageDispatcher;

        public SpecificContexualRoleController(MessageDispatcher messageDispatcher, SpecificContexualRoleView specificContexualRoleView)
        {
            _messageDispatcher = messageDispatcher;
            _specificContexualRoleView = specificContexualRoleView;
        }

        public IEnumerable<SpecificContexualRoleItem> Get()
        {
            return _specificContexualRoleView.SpecificContexualRoleList();
        }

        public SpecificContexualRoleItem Get(Guid id)
        {
            return _specificContexualRoleView.GetSpecificContexualRole(id);
        }

        [HttpPost]
        public IHttpActionResult CreateSpecificContexualRole(CreateSpecificContexualRole specificContexualRole)
        {
            _messageDispatcher.SendCommand(specificContexualRole);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditSpecificContexualRole(EditSpecificContexualRole specificContexualRole)
        {
            _messageDispatcher.SendCommand(specificContexualRole);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteSpecificContexualRole(Guid id)
        {
            _messageDispatcher.SendCommand(new DeleteSpecificContexualRole(){Id=id});
            return Ok();
        }
    }
}