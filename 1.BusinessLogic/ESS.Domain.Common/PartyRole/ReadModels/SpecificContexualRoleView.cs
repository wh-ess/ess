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
    public class SpecificContexualRoleView
        : ISubscribeTo<SpecificContexualRoleCreated>,  ISubscribeTo<SpecificContexualRoleDeleted>
    {
        private readonly IRepository<SpecificContexualRoleItem, Guid> _repository;

        public SpecificContexualRoleView(IRepository<SpecificContexualRoleItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<SpecificContexualRoleItem> SpecificContexualRoleList(Expression<Func<SpecificContexualRoleItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<SpecificContexualRoleItem> SpecificContexualRoleList()
        {
            return _repository.GetAll();
        }

        public SpecificContexualRoleItem GetSpecificContexualRole(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(SpecificContexualRoleCreated e)
        {
            var item = Mapper.DynamicMap<SpecificContexualRoleItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(SpecificContexualRoleDeleted e)
        {
            _repository.Delete(e.Id);
        }




        private void Update(Guid id, Action<SpecificContexualRoleItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class SpecificContexualRoleItem
    {
        public Guid Id;
        public Guid RoleTypeId;
        public Guid EntityId;
        public DateTime FromDate;
        public DateTime EndDate;
    }
}