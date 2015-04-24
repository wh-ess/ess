#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Contact.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Contact.ReadModels
{
    public class PartyContactView
        : ISubscribeTo<PartyContactCreated>, ISubscribeTo<PartyContactChanged>, ISubscribeTo<PartyContactDeleted>
    {
        private readonly IRepository<PartyContactItem, Guid> _repository;

        public PartyContactView(IRepository<PartyContactItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<PartyContactItem> PartyContactList(Expression<Func<PartyContactItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<PartyContactItem> PartyContactList()
        {
            return _repository.GetAll();
        }

        public PartyContactItem GetPartyContact(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(PartyContactCreated e)
        {
            var item = Mapper.DynamicMap<PartyContactItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(PartyContactDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(PartyContactChanged e)
        {
            Update(e.Id, c => c.ContactId = e.ContactId);
        }


        private void Update(Guid id, Action<PartyContactItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class PartyContactItem
    {
        public Guid Id;
        public Guid PartyId;
        public Guid ContactId;
    }
}