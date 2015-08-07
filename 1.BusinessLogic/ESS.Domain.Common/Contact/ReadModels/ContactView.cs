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
        private readonly IRepositoryAsync<ContactItem, Guid> _repositoryAsync;

        public ContactView(IRepositoryAsync<ContactItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<ContactItem>> ContactList(Expression<Func<ContactItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<ContactItem>> ContactList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<ContactItem> GetContact(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(ContactCreated e)
        {
            var item = Mapper.DynamicMap<ContactItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(ContactDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        public void Handle(ContactChanged e)
        {
            Update(e.Id, c => c.Address = e.Address);
        }


        private void Update(Guid id, Action<ContactItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
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