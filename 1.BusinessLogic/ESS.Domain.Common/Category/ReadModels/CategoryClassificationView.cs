#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Common.Category.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Category.ReadModels
{
    public class CategoryClassificationView
        : ISubscribeTo<CategoryClassificationCreated>,  ISubscribeTo<CategoryClassificationDeleted>
    {
        private readonly IRepository<CategoryClassificationItem, Guid> _repository;

        public CategoryClassificationView(IRepository<CategoryClassificationItem, Guid> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<CategoryClassificationItem>> CategoryClassificationList(Expression<Func<CategoryClassificationItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public Task<IEnumerable<CategoryClassificationItem>> CategoryClassificationList()
        {
            return _repository.GetAll();
        }

        public Task<CategoryClassificationItem> GetCategoryClassification(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(CategoryClassificationCreated e)
        {
            var item = Mapper.DynamicMap<CategoryClassificationItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(CategoryClassificationDeleted e)
        {
            _repository.Delete(e.Id);
        }




        private void Update(Guid id, Action<CategoryClassificationItem> action)
        {
            var item = _repository.Single(c => c.Id == id).Result;

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        #endregion
    }

    [Serializable]
    public class CategoryClassificationItem
    {
        public Guid Id;
        public Guid RelateId;
        public Guid CategoryId;
        public DateTime FromDate;
        public DateTime EndDate;
        public string Code;
        public bool IsSystem;
    }
}