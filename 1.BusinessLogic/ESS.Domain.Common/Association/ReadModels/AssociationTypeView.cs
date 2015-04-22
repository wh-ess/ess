#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Association.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Association.ReadModels
{
    public class AssociationTypeView
        : ISubscribeTo<AssociationTypeCreated>, ISubscribeTo<AssociationTypeNameChanged>, ISubscribeTo<AssociationTypeDeleted>,
            ISubscribeTo<AssociationTypeParentChanged>
    {
        private readonly IRepository<AssociationTypeItem, Guid> _repository;

        public AssociationTypeView(IRepository<AssociationTypeItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<AssociationTypeItem> AssociationTypeList(Expression<Func<AssociationTypeItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<AssociationTypeItem> AssociationTypeList()
        {
            return _repository.GetAll();
        }

        public AssociationTypeItem GetAssociationType(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(AssociationTypeCreated e)
        {
            var item = Mapper.DynamicMap<AssociationTypeItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(AssociationTypeDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(AssociationTypeNameChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<AssociationTypeItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        public void Handle(AssociationTypeParentChanged e)
        {
            Update(e.Id, c => c.ParentId = e.ParentId);
        }

        #endregion
    }

    [Serializable]
    public class AssociationTypeItem
    {
        public Guid Id;
        public string Name;
        public Guid ParentId;
    }
}