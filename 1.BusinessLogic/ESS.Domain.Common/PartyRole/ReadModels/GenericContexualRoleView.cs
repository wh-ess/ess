#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.PartyRole.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.PartyRole.ReadModels
{
    public class GenericContexualRoleView
        : ISubscribeTo<GenericContexualRoleCreated>,  ISubscribeTo<GenericContexualRoleDeleted>
    {
        private readonly IRepository<GenericContexualRoleItem, Guid> _repository;

        public GenericContexualRoleView(IRepository<GenericContexualRoleItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<GenericContexualRoleItem> GenericContexualRoleList(Expression<Func<GenericContexualRoleItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<GenericContexualRoleItem> GenericContexualRoleList()
        {
            return _repository.GetAll();
        }

        public GenericContexualRoleItem GetGenericContexualRole(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(GenericContexualRoleCreated e)
        {
            var item = Mapper.DynamicMap<GenericContexualRoleItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(GenericContexualRoleDeleted e)
        {
            _repository.Delete(e.Id);
        }




        private void Update(Guid id, Action<GenericContexualRoleItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class GenericContexualRoleItem
    {
        public Guid Id;
        public Guid PartyId;
        public Guid EntityId;
        public DateTime FromDate;
        public DateTime EndDate;
    }
}