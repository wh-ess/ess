using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

namespace ESS.Domain.Common.Basic.ReadModels
{
    public class BrandTypeView: ISubscribeTo<BrandTypeCreated>, ISubscribeTo<BrandTypeEdited>, ISubscribeTo<BrandTypeDeleted>
    {
        private readonly IRepository<BrandTypeItem, Guid> _repository;

        public BrandTypeView(IRepository<BrandTypeItem, Guid> repository)
        {
            _repository = repository;
        }
        
        public IEnumerable<BrandTypeItem> BrandTypeList(Expression<Func<BrandTypeItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<BrandTypeItem> BrandTypeList()
        {
            return _repository.GetAll();
        }
        public BrandTypeItem GetBrandType(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle
        public void Handle(BrandTypeCreated e)
        {
            var item = Mapper.DynamicMap<BrandTypeItem>(e);

            _repository.Add(e.Id, item);

        }

        public void Handle(BrandTypeDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(BrandTypeEdited e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<BrandTypeItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);

        }
        #endregion

        
    }

    [Serializable]
    public class BrandTypeItem
    {
        public Guid Id;
        public string Code;
        public string Name;
        public string ShortName;
        public string Parent;
        public bool IsActive;
    }
}
