﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ESS.Domain.Foundation.AccessControl.Commands;
using ESS.Domain.Foundation.AccessControl.ReadModels;
using ESS.Framework.CQRS;

namespace ESS.Api.Foundation.AccessControl
{
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {
        private readonly MessageDispatcher _messageDispatcher;
        private readonly RoleView _roleView;

        public RoleController(MessageDispatcher messageDispatcher, RoleView roleView)
        {
            _messageDispatcher = messageDispatcher;
            _roleView = roleView;
        }

        public IEnumerable<RoleItem> Get()
        {
            return _roleView.RoleList();
        }

        public RoleItem Get(Guid id)
        {
            return _roleView.GetRole(id);
        }

        [HttpPost]
        public IHttpActionResult CreateRole(CreateRole role)
        {
            _messageDispatcher.SendCommand(role);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult ChangeRoleInfo(ChangeRoleInfo role)
        {
            _messageDispatcher.SendCommand(role);
            return Ok();
        }

        [Route("{id}/AssignUser")]
        public IHttpActionResult AssignUser(AssignUser role)
        {
            _messageDispatcher.SendCommand(role);
            return Ok();
        }

        [Route("{id}/Lock")]
        public IHttpActionResult LockRole(LockRole role)
        {
            _messageDispatcher.SendCommand(role);
            return Ok();
        }

        [Route("{id}/Unlock")]
        public IHttpActionResult UnlockRole(UnlockRole role)
        {
            _messageDispatcher.SendCommand(role);
            return Ok();
        }

    }
}
