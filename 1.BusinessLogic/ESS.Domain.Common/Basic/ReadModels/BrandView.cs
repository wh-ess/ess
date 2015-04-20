#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Basic.ReadModels
{
    public class BrandView : ISubscribeTo<BrandCreated>, ISubscribeTo<BrandEdited>, ISubscribeTo<BrandDeleted>
    {
        private readonly IRepository<BrandItem, Guid> _repository;

        public BrandView(IRepository<BrandItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<BrandItem> BrandList(Expression<Func<BrandItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<BrandItem> BrandList()
        {
            return _repository.GetAll();
        }

        public BrandItem GetBrand(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(BrandCreated e)
        {
            var item = Mapper.DynamicMap<BrandItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(BrandDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(BrandEdited e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<BrandItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        #endregion
    }

    [Serializable]
    public class BrandItem
    {
        public Guid Id;
        public string Code;
        public string Name;
        public string ShortName;
        public string Parent;
        public bool IsActive;
    }
}