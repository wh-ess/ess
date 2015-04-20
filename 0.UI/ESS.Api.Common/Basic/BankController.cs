using System;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Common.Basic.Commands;
using ESS.Domain.Common.Basic.ReadModels;
using ESS.Framework.CQRS;

namespace ESS.Api.Common.Basic
{
    public class BankController : ApiController
    {
        private readonly MessageDispatcher _messageDispatcher;
        private readonly BankView _bankView;

        public BankController(MessageDispatcher messageDispatcher, BankView bankView)
        {
            _messageDispatcher = messageDispatcher;
            _bankView = bankView;
        }

        public IEnumerable<BankItem> Get()
        {
            return _bankView.BankList();
        }

        public BankItem Get(Guid id)
        {
            return _bankView.GetBank(id);
        }

        [HttpPost]
        public IHttpActionResult CreateBank(CreateBank bank)
        {
            _messageDispatcher.SendCommand(bank);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditBank(EditBank bank)
        {
            _messageDispatcher.SendCommand(bank);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteBank(DeleteBank bank)
        {
            _messageDispatcher.SendCommand(bank);
            return Ok();
        }
    }
}
