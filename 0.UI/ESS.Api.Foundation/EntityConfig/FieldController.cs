#region

using System.Collections.Generic;
using System.Web.Http;
using ESS.Domain.Common.Association.ReadModels;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Foundation.EntityConfig
{
    [RoutePrefix("api/Module/{moduleNo}/Actions/{actionName}")]
    public class FieldController : ApiController
    {
        private readonly FieldView _fieldView;
        private readonly CategoryView _categoryView;
        private readonly DefaultMessageBus _messageDispatcher;

        public FieldController(DefaultMessageBus messageDispatcher, FieldView fieldView, CategoryView categoryView)
        {
            _messageDispatcher = messageDispatcher;
            _fieldView = fieldView;
            _categoryView = categoryView;
        }

        [HttpGet]
        [Route("Fields")]
        public IEnumerable<FieldItem> Fields(string moduleNo,string actionName)
        {
            var fields = _fieldView.FieldList(moduleNo, actionName);
            var association = _categoryView.GetCategoryBySchemeType(CategoryTypeSchemeType.Association, CategoryTypeType.Party);
            return fields;
        }

        [HttpGet]
        [Route("~/api/Fields")]
        public IEnumerable<FieldItem> FieldSelect()
        {
            return _fieldView.FieldList();
        }

        [Route("Fields")]
        public void SaveFields(string moduleNo,string actionName, List<FieldItem> list)
        {
            _messageDispatcher.SendCommand(new SaveFields(){ModuleNo = moduleNo,ActionName = actionName,Fields = list});
        }
    }
}