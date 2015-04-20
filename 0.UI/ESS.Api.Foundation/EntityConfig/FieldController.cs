#region

using System.Collections.Generic;
using System.Web.Http;
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
        private readonly MessageDispatcher _messageDispatcher;

        public FieldController(MessageDispatcher messageDispatcher, FieldView fieldView)
        {
            _messageDispatcher = messageDispatcher;
            _fieldView = fieldView;
            
        }

        [HttpGet]
        [Route("Fields")]
        public IEnumerable<FieldItem> Fields(string moduleNo,string actionName)
        {
            return _fieldView.FieldList(moduleNo, actionName);
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