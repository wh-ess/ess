﻿#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Status.Domain;
using ESS.Domain.Common.Status.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Status.ReadModels
{
    public class StatusView
        : ISubscribeTo<StatusCreated>,  ISubscribeTo<StatusDeleted>
    {
        private readonly IRepository<StatusItem, Guid> _repository;

        public StatusView(IRepository<StatusItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<StatusItem> StatusList(Expression<Func<StatusItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<StatusItem> StatusList()
        {
            return _repository.GetAll();
        }

        public StatusItem GetStatus(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(StatusCreated e)
        {
            var item = Mapper.DynamicMap<StatusItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(StatusDeleted e)
        {
            _repository.Delete(e.Id);
        }




        private void Update(Guid id, Action<StatusItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class StatusItem
    {
        public Guid Id;
        public Guid RelateId;
        public Guid StatusTypeId;
        public DateTime StatusDateTime;
        public DateTime StatusFromDate;
        public DateTime StatusEndDate;
        public DateTime FromDate;
        public DateTime EndDate;
    }
}