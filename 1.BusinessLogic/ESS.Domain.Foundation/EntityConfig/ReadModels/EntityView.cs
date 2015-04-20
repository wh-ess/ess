#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.ReadModels
{
    public class EntityView : ISubscribeTo<EntityCreated>, ISubscribeTo<EntityEdited>, ISubscribeTo<EntityDeleted>
    {
        private readonly IRepository<EntityItem, Guid> _repository;
        private readonly ModuleView _moduleView;

        public EntityView(IRepository<EntityItem, Guid> repository, ModuleView moduleView)
        {
            _repository = repository;
            _moduleView = moduleView;
        }

        public IEnumerable<EntityItem> GetEntity(string moduleNo)
        {
            return _repository.Find(c => c.ModuleNo.ToLower() == moduleNo.ToLower());

        }


        public EntityItem GetEntity(string moduleNo, Guid id)
        {
            return _repository.Single(c => c.ModuleNo.ToLower() == moduleNo.ToLower() && c.Id == id);
        }


        #region handle

        public void Handle(EntityCreated e)
        {
            _repository.Add(e.Id, new EntityItem() { Id = e.Id, ModuleNo = e.ModuleNo, Fields = e.Fields });

        }

        public void Handle(EntityDeleted e)
        {
            _repository.Delete(e.Id);
        }

        public void Handle(EntityEdited e)
        {
            _repository.Update(e.Id, new EntityItem() { Id = e.Id, ModuleNo = e.ModuleNo, Fields = e.Fields });

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