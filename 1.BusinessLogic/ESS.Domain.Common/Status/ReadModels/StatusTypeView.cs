#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Status.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Status.ReadModels
{
    public class StatusTypeView
        : ISubscribeTo<StatusTypeCreated>, ISubscribeTo<StatusTypeNameChanged>, ISubscribeTo<StatusTypeDeleted>
    {
        private readonly IRepository<StatusTypeItem, Guid> _repository;

        public StatusTypeView(IRepository<StatusTypeItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<StatusTypeItem> StatusTypeList(Expression<Func<StatusTypeItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<StatusTypeItem> StatusTypeList()
        {
            return _repository.GetAll();
        }

        public StatusTypeItem GetStatusType(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(StatusTypeCreated e)
        {
            var item = Mapper.DynamicMap<StatusTypeItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(StatusTypeDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(StatusTypeNameChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<StatusTypeItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class StatusTypeItem
    {
        public Guid Id;
        public string Name;
    }
}