#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.ReadModels
{
    public class EntityView : ISubscribeTo<EntityCreated>, ISubscribeTo<EntityEdited>, ISubscribeTo<EntityDeleted>
    {
        private readonly IRepositoryAsync<EntityItem, Guid> _repositoryAsync;
        private readonly ModuleView _moduleView;

        public EntityView(IRepositoryAsync<EntityItem, Guid> repositoryAsync, ModuleView moduleView)
        {
            _repositoryAsync = repositoryAsync;
            _moduleView = moduleView;
        }

        public async Task<IEnumerable<EntityItem>> GetEntity(string moduleNo)
        {
            return await _repositoryAsync.FindAsync(c => c.ModuleNo.ToLower() == moduleNo.ToLower());

        }


        public async Task<EntityItem> GetEntity(string moduleNo, Guid id)
        {
            return await _repositoryAsync.SingleAsync(c => c.ModuleNo.ToLower() == moduleNo.ToLower() && c.Id == id);
        }


        #region handle

        public void Handle(EntityCreated e)
        {
            _repositoryAsync.AddAsync(e.Id, new EntityItem() { Id = e.Id, ModuleNo = e.ModuleNo, Fields = e.Fields });

        }

        public void Handle(EntityDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }

        public void Handle(EntityEdited e)
        {
            _repositoryAsync.UpdateAsync(e.Id, new EntityItem() { Id = e.Id, ModuleNo = e.ModuleNo, Fields = e.Fields });

        }

        #endregion
    }

    [Serializable]
    public class EntityItem
    {
        public Guid Id;
        public string ModuleNo;
        public Dictionary<Guid, object> Fields;
    }

}