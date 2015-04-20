using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ESS.Domain.Common.Basic.Commands;
using ESS.Domain.Common.Basic.ReadModels;
using ESS.Framework.CQRS;

namespace ESS.Api.Common.Basic
{
    public class FloorController : ApiController
    {
        private readonly MessageDispatcher _messageDispatcher;
        private readonly FloorView _floorView;

        public FloorController(MessageDispatcher messageDispatcher, FloorView floorView)
        {
            _messageDispatcher = messageDispatcher;
            _floorView = floorView;
        }
        

        public IEnumerable<FloorItem> Get()
        {
            return _floorView.FloorList().OrderBy(c => c.Name);
        }

        public FloorItem Get(Guid id)
        {
            return _floorView.GetFloor(id);
        }

        [HttpPost]
        public IHttpActionResult CreateFloor(CreateFloor floor)
        {
            _messageDispatcher.SendCommand(floor);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditFloor(EditFloor floor)
        {
            _messageDispatcher.SendCommand(floor);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteFloor(DeleteFloor floor)
        {
            _messageDispatcher.SendCommand(floor);
            return Ok();
        }
    }
}
