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
        private readonly IRepository<PostalAddressPartItem, Guid> _repository;

        public PostalAddressPartView(IRepository<PostalAddressPartItem, Guid> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<PostalAddressPartItem>> PostalAddressPartList(Expression<Func<PostalAddressPartItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public Task<IEnumerable<PostalAddressPartItem>> PostalAddressPartList()
        {
            return _repository.GetAll();
        }

        public Task<PostalAddressPartItem> GetPostalAddressPart(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(PostalAddressPartCreated e)
        {
            var item = Mapper.DynamicMap<PostalAddressPartItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(PostalAddressPartDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(PostalAddressPartChanged e)
        {
            Update(e.Id, c => c.ContactId = e.ContactId);
        }


        private void Update(Guid id, Action<PostalAddressPartItem> action)
        {
            var item = _repository.Single(c => c.Id == id).Result;

            action.Invoke(item);
            _repository.Update(item.Id, item);
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