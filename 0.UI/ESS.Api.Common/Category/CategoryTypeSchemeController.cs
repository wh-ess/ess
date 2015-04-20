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
    public class CategoryTypeSchemeController : ApiController
    {
        private readonly CategoryTypeSchemeView _categoryTypeSchemeView;
        private readonly MessageDispatcher _messageDispatcher;

        public CategoryTypeSchemeController(MessageDispatcher messageDispatcher, CategoryTypeSchemeView categoryTypeSchemeView)
        {
            _messageDispatcher = messageDispatcher;
            _categoryTypeSchemeView = categoryTypeSchemeView;
        }

        public IEnumerable<CategoryTypeSchemeItem> Get()
        {
            return _categoryTypeSchemeView.CategoryTypeSchemeList();
        }

        public CategoryTypeSchemeItem Get(Guid id)
        {
            return _categoryTypeSchemeView.GetCategoryTypeScheme(id);
        }

        [HttpPost]
        public IHttpActionResult CreateCategoryTypeScheme(CreateCategoryTypeScheme categoryTypeScheme)
        {
            _messageDispatcher.SendCommand(categoryTypeScheme);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditCategoryTypeScheme(ChangeCategoryTypeSchemeName categoryTypeScheme)
        {
            _messageDispatcher.SendCommand(categoryTypeScheme);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteCategoryTypeScheme(DeleteCategoryTypeScheme categoryTypeScheme)
        {
            _messageDispatcher.SendCommand(categoryTypeScheme);
            return Ok();
        }
    }
}