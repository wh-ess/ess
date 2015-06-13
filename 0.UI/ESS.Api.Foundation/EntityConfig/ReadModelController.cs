#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using ESS.Framework.Common.Components;
using ESS.Framework.CQRS;
using ESS.Framework.Data;
using ESS.Framework.UI.Configurations;

#endregion

namespace ESS.Api.Foundation.EntityConfig
{
    /// <summary>
    ///     对read model 进行重建和查询
    /// </summary>
    public class ReadModelController : ApiController
    {
        private readonly MessageDispatcher _messageDispatcher;

        public ReadModelController(MessageDispatcher messageDispatcher)
        {
            _messageDispatcher = messageDispatcher;
        }
        [HttpGet]
        public IEnumerable<string> ReadModels()
        {
            return ModuleBuilder.ReadModels.Keys;
        }

        [HttpGet]
        public IEnumerable ReadModelData(string id)
        {
            if (ModuleBuilder.ReadModels.ContainsKey(id))
            {
                var r = ModuleBuilder.ReadModels[id];
                if (r != null)
                    return r.GetAll();
            }
            return null;
        }
        public void Replay(string id)
        {
            if (ModuleBuilder.ReadModels.ContainsKey(id))
            {
                _messageDispatcher.Repaly(ModuleBuilder.ReadModels[id]);
            }
        }

    }
}