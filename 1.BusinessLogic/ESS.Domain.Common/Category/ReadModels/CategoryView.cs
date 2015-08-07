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
    public class CategoryView
        : ISubscribeTo<CategoryCreated>, ISubscribeTo<CategoryNameChanged>, ISubscribeTo<CategoryDeleted>, ISubscribeTo<CategoryParentChanged>,
            ISubscribeTo<CategoryTypeChanged>, ISubscribeTo<CategoryDateChanged>
    {
        private readonly IRepositoryAsync<CategoryItem, Guid> _repositoryAsync;

        public CategoryView(IRepositoryAsync<CategoryItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<CategoryItem>> CategoryList(Expression<Func<CategoryItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<CategoryItem>> CategoryList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public async Task<CategoryItem> GetCategory(Guid id)
        {
            return await _repositoryAsync.GetAsync(id);
        }
        public async Task<IEnumerable<CategoryItem>> GetCategoryBySchemeType(string schemeName, string typeName)
        {
            return await _repositoryAsync.FindAsync(c => c.Type.Scheme.Name==schemeName && c.Type.Name == typeName);
        }
        #region handle

        public void Handle(CategoryCreated e)
        {
            var item = Mapper.DynamicMap<CategoryItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(CategoryDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        public void Handle(CategoryNameChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }

        public void Handle(CategoryParentChanged e)
        {
            Update(e.Id, c => c.ParentId = e.ParentId);
        }

        public void Handle(CategoryTypeChanged e)
        {
            Update(e.Id, c => c.Type = e.Type);
        }

        public void Handle(CategoryDateChanged e)
        {
            Update(e.Id, c =>
            {
                c.FromDate = e.FromDate;
                c.EndDate = e.EndDate;
            });
        }

        private void Update(Guid id, Action<CategoryItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }

        #endregion

        
    }

    [Serializable]
    public class CategoryItem
    {
        public CategoryTypeItem Type;
        public DateTime EndDate;
        public DateTime FromDate;
        public Guid Id;
        public string Name;
        public Guid ParentId;
        public string Code;
        public bool IsSystem;
    }
}