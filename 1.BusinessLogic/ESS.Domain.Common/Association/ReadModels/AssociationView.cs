#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Association.Domain;
using ESS.Domain.Common.Association.Events;
using ESS.Domain.Common.PartyRole.ReadModels;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Association.ReadModels
{
    public class AssociationView
        : ISubscribeTo<AssociationCreated>, ISubscribeTo<AssociationDeleted>, ISubscribeTo<AssociationEdited>
    {
        private readonly IRepository<AssociationItem, Guid> _repository;

        public AssociationView(IRepository<AssociationItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<AssociationItem> AssociationList(Expression<Func<AssociationItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<AssociationItem> AssociationList()
        {
            return _repository.GetAll();
        }

        public AssociationItem GetAssociation(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(AssociationCreated e)
        {
            var item = Mapper.DynamicMap<AssociationItem>(e);

            _repository.Add(e.Id, item);
        }
        public void Handle(AssociationEdited e)
        {
            var item = Mapper.DynamicMap<AssociationItem>(e);

            _repository.Update(e.Id, item);
        }

        public void Handle(AssociationDeleted e)
        {
            _repository.Delete(e.Id);
        }




        private void Update(Guid id, Action<AssociationItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class AssociationItem
    {
        public Guid Id;
        public Guid From;
        public Guid To;
        public AssociationRule AssociationRule;
        public Guid TypeId;
        public DateTime FromDate;
        public DateTime EndDate;

        public string Code;
        public bool IsSystem;
    }
    
}