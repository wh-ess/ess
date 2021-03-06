﻿#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Common.Status.Domain;
using ESS.Domain.Common.Status.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.CQRS.ReadModel;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Status.ReadModels
{
    public class StatusView
        :ReadModel, ISubscribeTo<StatusCreated>,  ISubscribeTo<StatusDeleted>
    {
        private readonly IRepositoryAsync<StatusItem, Guid> _repositoryAsync;

        public StatusView(IRepositoryAsync<StatusItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public Task<IQueryable<StatusItem>> StatusList(Expression<Func<StatusItem, bool>> condition)
        {
            return _repositoryAsync.FindAsync(condition);
        }

        public Task<IQueryable<StatusItem>> StatusList()
        {
            return _repositoryAsync.GetAllAsync();
        }

        public Task<StatusItem> GetStatus(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(StatusCreated e)
        {
            var item = Mapper.DynamicMap<StatusItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(StatusDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        private void Update(Guid id, Action<StatusItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }


        public override Task<bool> Clear()
        {
            return _repositoryAsync.DeleteAllAsync();
        }

        public override async Task<IEnumerable> GetAll()
        {
            return await StatusList();
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