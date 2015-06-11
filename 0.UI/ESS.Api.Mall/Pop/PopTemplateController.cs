﻿#region

using System;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Mall.Pop.Commands;
using ESS.Domain.Mall.Pop.ReadModels;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Mall.Pop
{
    public class PopTemplateController : ApiController
    {
        private readonly PopTemplateView _popTemplateView;
        private readonly MessageDispatcher _messageDispatcher;

        public PopTemplateController(MessageDispatcher messageDispatcher, PopTemplateView popTemplateView)
        {
            _messageDispatcher = messageDispatcher;
            _popTemplateView = popTemplateView;
        }

        public IEnumerable<PopTemplateItem> Get()
        {
            return _popTemplateView.PopTemplateList();
        }

        public PopTemplateItem Get(Guid id)
        {
            return _popTemplateView.GetPopTemplate(id);
        }

        [HttpPost]
        public IHttpActionResult CreatePopTemplate(CreatePopTemplate PopTemplate)
        {
            _messageDispatcher.SendCommand(PopTemplate);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditPopTemplate(EditPopTemplate PopTemplate)
        {
            _messageDispatcher.SendCommand(PopTemplate);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeletePopTemplate(Guid id)
        {
            _messageDispatcher.SendCommand(new DeletePopTemplate(){Id=id});
            return Ok();
        }
    }
}