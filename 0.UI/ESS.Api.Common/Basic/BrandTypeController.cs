using System;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Common.Basic.Commands;
using ESS.Domain.Common.Basic.ReadModels;
using ESS.Framework.CQRS;

namespace ESS.Api.Common.Basic
{
    public class BrandTypeController : ApiController
    {
        private readonly MessageDispatcher _messageDispatcher;
        private readonly BrandTypeView _brandTypeView;

        public BrandTypeController(MessageDispatcher messageDispatcher, BrandTypeView brandTypeView)
        {
            _messageDispatcher = messageDispatcher;
            _brandTypeView = brandTypeView;
        }
        
        public IEnumerable<BrandTypeItem> Get()
        {
            return _brandTypeView.BrandTypeList();
        }

        public BrandTypeItem Get(Guid id)
        {
            return _brandTypeView.GetBrandType(id);
        }

        [HttpPost]
        public IHttpActionResult CreateBrandType(CreateBrandType brandType)
        {
            _messageDispatcher.SendCommand(brandType);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditBrandType(EditBrandType brandType)
        {
            _messageDispatcher.SendCommand(brandType);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteBrandType(DeleteBrandType brandType)
        {
            _messageDispatcher.SendCommand(brandType);
            return Ok();
        }
    }
}
