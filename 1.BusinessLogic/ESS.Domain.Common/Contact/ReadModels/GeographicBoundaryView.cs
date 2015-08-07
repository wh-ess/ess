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
    public class GeographicBoundaryView
        : ISubscribeTo<GeographicBoundaryCreated>, ISubscribeTo<GeographicBoundaryChanged>, ISubscribeTo<GeographicBoundaryDeleted>
    {
        private readonly IRepositoryAsync<GeographicBoundaryItem, Guid> _repositoryAsync;

        public GeographicBoundaryView(IRepositoryAsync<GeographicBoundaryItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<GeographicBoundaryItem>> GeographicBoundaryList(Expression<Func<GeographicBoundaryItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<GeographicBoundaryItem>> GeographicBoundaryList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<GeographicBoundaryItem> GetGeographicBoundary(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(GeographicBoundaryCreated e)
        {
            var item = Mapper.DynamicMap<GeographicBoundaryItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(GeographicBoundaryDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        public void Handle(GeographicBoundaryChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<GeographicBoundaryItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class GeographicBoundaryItem
    {
        public Guid Id;
        public Guid TypeId;
        public string Name;
        public string Code;
        //缩写
        public string Abbr;
        public string InternetRegionCode;
    }
}