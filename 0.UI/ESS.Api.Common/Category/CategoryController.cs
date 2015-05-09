#region

using System;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Common.Category.Commands;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Framework.CQRS;
using ESS.Framework.UI.Attribute;

#endregion

namespace ESS.Api.Common.Category
{
    public class CategoryController : ApiController
    {
        private readonly CategoryView _categoryView;
        private readonly MessageDispatcher _messageDispatcher;

        public CategoryController(MessageDispatcher messageDispatcher, CategoryView categoryView)
        {
            _messageDispatcher = messageDispatcher;
            _categoryView = categoryView;
        }

        public IEnumerable<CategoryItem> Get()
        {
            return _categoryView.CategoryList();
        }

        public CategoryItem Get(Guid id)
        {
            return _categoryView.GetCategory(id);
        }
        [Route("~/api/CategoryTypeScheme/{scheme}/CategoryType/{type}")]
        public IEnumerable<CategoryItem> GetByScheme(string scheme,string type)
        {
            return _categoryView.GetCategoryBySchemeType(scheme,type);
        }
        [HttpPost]
        public IHttpActionResult CreateCategory(CreateCategory category)
        {
            _messageDispatcher.SendCommand(category);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditCategory(ChangeCategoryName category)
        {
            _messageDispatcher.SendCommand(category);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteCategory(Guid id)
        {
            _messageDispatcher.SendCommand(new DeleteCategory(){Id=id});
            return Ok();
        }
    }
}