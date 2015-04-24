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
    public class GeographicBoundaryView
        : ISubscribeTo<GeographicBoundaryCreated>, ISubscribeTo<GeographicBoundaryChanged>, ISubscribeTo<GeographicBoundaryDeleted>
    {
        private readonly IRepository<GeographicBoundaryItem, Guid> _repository;

        public GeographicBoundaryView(IRepository<GeographicBoundaryItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<GeographicBoundaryItem> GeographicBoundaryList(Expression<Func<GeographicBoundaryItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<GeographicBoundaryItem> GeographicBoundaryList()
        {
            return _repository.GetAll();
        }

        public GeographicBoundaryItem GetGeographicBoundary(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(GeographicBoundaryCreated e)
        {
            var item = Mapper.DynamicMap<GeographicBoundaryItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(GeographicBoundaryDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(GeographicBoundaryChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<GeographicBoundaryItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
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
        public string Abbreviation;
        public string InternetRegionCode;
    }
}