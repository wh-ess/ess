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
        private readonly IRepositoryAsync<CategoryClassificationItem, Guid> _repositoryAsync;

        public CategoryClassificationView(IRepositoryAsync<CategoryClassificationItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<CategoryClassificationItem>> CategoryClassificationList(Expression<Func<CategoryClassificationItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<CategoryClassificationItem>> CategoryClassificationList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public async Task<CategoryClassificationItem> GetCategoryClassification(Guid id)
        {
            return await _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(CategoryClassificationCreated e)
        {
            var item = Mapper.DynamicMap<CategoryClassificationItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(CategoryClassificationDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }




        private void Update(Guid id, Action<CategoryClassificationItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
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