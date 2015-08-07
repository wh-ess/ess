#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Common.Contact.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Contact.ReadModels
{
    public class PostalAddressPartView
        : ISubscribeTo<PostalAddressPartCreated>, ISubscribeTo<PostalAddressPartChanged>, ISubscribeTo<PostalAddressPartDeleted>
    {
        private readonly IRepositoryAsync<PostalAddressPartItem, Guid> _repositoryAsync;

        public PostalAddressPartView(IRepositoryAsync<PostalAddressPartItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<PostalAddressPartItem>> PostalAddressPartList(Expression<Func<PostalAddressPartItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<PostalAddressPartItem>> PostalAddressPartList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<PostalAddressPartItem> GetPostalAddressPart(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(PostalAddressPartCreated e)
        {
            var item = Mapper.DynamicMap<PostalAddressPartItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(PostalAddressPartDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        public void Handle(PostalAddressPartChanged e)
        {
            Update(e.Id, c => c.ContactId = e.ContactId);
        }


        private void Update(Guid id, Action<PostalAddressPartItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class PostalAddressPartItem
    {
        public Guid Id;
        public Guid ContactId;
        public Guid TypeId;
        public Guid GeographicBoundaryId;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Text;
    }
}