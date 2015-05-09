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
    public class GenericContexualRoleController : ApiController
    {
        private readonly GenericContexualRoleView _genericContexualRoleView;
        private readonly MessageDispatcher _messageDispatcher;

        public GenericContexualRoleController(MessageDispatcher messageDispatcher, GenericContexualRoleView genericContexualRoleView)
        {
            _messageDispatcher = messageDispatcher;
            _genericContexualRoleView = genericContexualRoleView;
        }

        public IEnumerable<GenericContexualRoleItem> Get()
        {
            return _genericContexualRoleView.GenericContexualRoleList();
        }

        public GenericContexualRoleItem Get(Guid id)
        {
            return _genericContexualRoleView.GetGenericContexualRole(id);
        }

        [HttpPost]
        public IHttpActionResult CreateGenericContexualRole(CreateGenericContexualRole genericContexualRole)
        {
            _messageDispatcher.SendCommand(genericContexualRole);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditGenericContexualRole(EditGenericContexualRole genericContexualRole)
        {
            _messageDispatcher.SendCommand(genericContexualRole);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteGenericContexualRole(Guid id)
        {
            _messageDispatcher.SendCommand(new DeleteGenericContexualRole(){Id=id});
            return Ok();
        }
    }
}