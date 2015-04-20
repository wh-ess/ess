#region

using System;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Common.Category.Commands;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Common.Category
{
    public class CategoryTypeController : ApiController
    {
        private readonly CategoryTypeView _categoryTypeView;
        private readonly MessageDispatcher _messageDispatcher;

        public CategoryTypeController(MessageDispatcher messageDispatcher, CategoryTypeView categoryTypeView)
        {
            _messageDispatcher = messageDispatcher;
            _categoryTypeView = categoryTypeView;
        }

        public IEnumerable<CategoryTypeItem> Get()
        {
            return _categoryTypeView.CategoryTypeList();
        }

        public CategoryTypeItem Get(Guid id)
        {
            return _categoryTypeView.GetCategoryType(id);
        }

        [HttpPost]
        public IHttpActionResult CreateCategoryType(CreateCategoryType categoryType)
        {
            _messageDispatcher.SendCommand(categoryType);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditCategoryType(ChangeCategoryTypeName categoryType)
        {
            _messageDispatcher.SendCommand(categoryType);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteCategoryType(DeleteCategoryType categoryType)
        {
            _messageDispatcher.SendCommand(categoryType);
            return Ok();
        }
    }
}