#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Common.Category.Events;
using ESS.Domain.Common.Contact.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Contact.ReadModels
{
    public class ContactView
        : ISubscribeTo<ContactCreated>, ISubscribeTo<ContactChanged>, ISubscribeTo<ContactDeleted>
    {
        private readonly IRepository<ContactItem, Guid> _repository;

        public ContactView(IRepository<ContactItem, Guid> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ContactItem>> ContactList(Expression<Func<ContactItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public Task<IEnumerable<ContactItem>> ContactList()
        {
            return _repository.GetAll();
        }

        public Task<ContactItem> GetContact(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(ContactCreated e)
        {
            var item = Mapper.DynamicMap<ContactItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(ContactDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(ContactChanged e)
        {
            Update(e.Id, c => c.Address = e.Address);
        }


        private void Update(Guid id, Action<ContactItem> action)
        {
            var item = _repository.Single(c => c.Id == id).Result;

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class ContactItem
    {
        public Guid Id;
        public string CountryCode;
        public string AreaCode;
        public string PhoneNumber;
        public string Address;
    }
}