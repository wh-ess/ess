using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

namespace ESS.Domain.Common.Basic.ReadModels
{
    public class FloorView : ISubscribeTo<FloorCreated>, ISubscribeTo<FloorEdited>, ISubscribeTo<FloorDeleted>
    {
        private readonly IRepository<FloorItem, Guid> _repository;

        public FloorView(IRepository<FloorItem, Guid> repository)
        {
            _repository = repository;
        }
        
        public IEnumerable<FloorItem> FloorList(Expression<Func<FloorItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<FloorItem> FloorList()
        {
            return _repository.GetAll();
        }

        public FloorItem GetFloor(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle
        public void Handle(FloorCreated e)
        {
            var item = Mapper.DynamicMap<FloorItem>(e);

            _repository.Add(e.Id, item);

        }

        public void Handle(FloorDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(FloorEdited e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<FloorItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);

        }
        #endregion
    }

    [Serializable]
    public class FloorItem
    {
        public Guid Id;
        public string Code;
        public string Name;
        public string ShortName;
        public bool IsActive;
    }
}
