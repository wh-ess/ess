using System;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Common.Basic.Commands;
using ESS.Domain.Common.Basic.ReadModels;
using ESS.Framework.CQRS;

namespace ESS.Api.Common.Basic
{
    public class BrandController : ApiController
    {
        private readonly MessageDispatcher _messageDispatcher;
        private readonly BrandView _brandView;

        public BrandController(MessageDispatcher messageDispatcher, BrandView brandView)
        {
            _messageDispatcher = messageDispatcher;
            _brandView = brandView;
        }

        public IEnumerable<BrandItem> Get()
        {
            return _brandView.BrandList();
        }

        public BrandItem Get(Guid id)
        {
            return _brandView.GetBrand(id);
        }

        [HttpPost]
        public IHttpActionResult CreateBrand(CreateBrand brand)
        {
            _messageDispatcher.SendCommand(brand);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditBrand(EditBrand brand)
        {
            _messageDispatcher.SendCommand(brand);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteBrand(DeleteBrand brand)
        {
            _messageDispatcher.SendCommand(brand);
            return Ok();
        }
    }
}
