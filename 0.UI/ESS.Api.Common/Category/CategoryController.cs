﻿#region

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
    [Module(parentModuleNo:"Category",moduleNo:"Category")]
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
        public IHttpActionResult DeleteCategory(DeleteCategory category)
        {
            _messageDispatcher.SendCommand(category);
            return Ok();
        }
    }
}