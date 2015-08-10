using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ESS.Domain.Foundation.AccessControl.Commands;
using ESS.Domain.Foundation.AccessControl.ReadModels;
using ESS.Framework.CQRS;

namespace ESS.Api.Foundation.AccessControl
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly DefaultMessageBus _messageDispatcher;
        private readonly UserView _userView;

        public UserController(DefaultMessageBus messageDispatcher, UserView userView)
        {
            _messageDispatcher = messageDispatcher;
            _userView = userView;
        }

        public async Task<IEnumerable<UserItem>> Get()
        {
            return await _userView.UserList();
        }

        public UserItem Get(Guid id)
        {
            return _userView.GetUser(id);
        }

        [HttpPost]
        public IHttpActionResult CreateUser(CreateUser user)
        {
            _messageDispatcher.SendCommand(user);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult ChangeUserInfo(ChangeUserInfo user)
        {
            _messageDispatcher.SendCommand(user);
            return Ok();
        }

        [HttpPost]
        [Route("{id}/changePassword")]
        public IHttpActionResult ChangePassword(ChangePassword user)
        {
            _messageDispatcher.SendCommand(user);
            return Ok();
        }

        [Route("{id}/Lock")]
        public IHttpActionResult LockUser(LockUser user)
        {
            _messageDispatcher.SendCommand(user);
            return Ok();
        }

        [Route("{id}/Unlock")]
        public IHttpActionResult UnlockUser(UnlockUser user)
        {
            _messageDispatcher.SendCommand(user);
            return Ok();
        }

    }
}
