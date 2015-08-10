#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Foundation.EntityConfig
{
    [RoutePrefix("api/Entity")]
    public class EntityController : ApiController
    {
        private readonly EntityView _entityView;
        private readonly DefaultMessageBus _messageDispatcher;

        public EntityController(DefaultMessageBus messageDispatcher, EntityView entityView)
        {
            _messageDispatcher = messageDispatcher;
            _entityView = entityView;
        }

        [Route("{moduleNo}")]
        public Task<IEnumerable<EntityItem>> GetEntity(string moduleNo)
        {
            return _entityView.GetEntity(moduleNo);
        }

        [Route("{moduleNo}/{id}")]
        public Task<EntityItem> GetEntity(string moduleNo, Guid id)
        {
            return _entityView.GetEntity(moduleNo, id);
        }

        [Route("{moduleNo}/{id}")]
        [HttpPut]
        public void UpdateEntity(string moduleNo, Guid id, EditEntity entity)
        {
            _messageDispatcher.SendCommand(entity);
        }

        [Route("{moduleNo}")]
        public void CreateEntity(string moduleNo, CreateEntity entity)
        {
            entity.ModuleNo = moduleNo;
            _messageDispatcher.SendCommand(entity);
        }
    }
}