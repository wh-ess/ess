#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Common.Category.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.CQRS.ReadModel;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Category.ReadModels
{
    public class CategoryTypeView
        : ReadModel,ISubscribeTo<CategoryTypeCreated>, ISubscribeTo<CategoryTypeNameChanged>, ISubscribeTo<CategoryTypeDeleted>,
            ISubscribeTo<CategoryTypeParentChanged>, ISubscribeTo<CategoryTypeSchemeChanged>,
            ISubscribeTo<CategoryTypeSchemeNameChanged>
    {
        private readonly IRepositoryAsync<CategoryTypeItem, Guid> _repositoryAsync;

        public CategoryTypeView(IRepositoryAsync<CategoryTypeItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<CategoryTypeItem>> CategoryTypeList(Expression<Func<CategoryTypeItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<CategoryTypeItem>> CategoryTypeList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<CategoryTypeItem> GetCategoryType(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        public async Task<IEnumerable<CategoryTypeItem>> GetCategoryTypeByScheme(string name)
        {
            return await _repositoryAsync.FindAsync(c => c.Scheme.Name == name);
        }

        #region handle

        public void Handle(CategoryTypeCreated e)
        {
            var item = Mapper.DynamicMap<CategoryTypeItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(CategoryTypeDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        public void Handle(CategoryTypeNameChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<CategoryTypeItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }

        public void Handle(CategoryTypeParentChanged e)
        {
            Update(e.Id, c => c.ParentId = e.ParentId);
        }

        public void Handle(CategoryTypeSchemeChanged e)
        {
            Update(e.Id, c => c.Scheme = e.Scheme);
        }
        public void Handle(CategoryTypeSchemeNameChanged e)
        {
            Update(e.Id, c => c.Scheme.Name = e.Name);
        }

        public override Task<bool> Clear()
        {
            return _repositoryAsync.DeleteAllAsync();
        }

        public override async Task<IEnumerable> GetAll()
        {
            return await CategoryTypeList();
        }
        #endregion

    }

    [Serializable]
    public class CategoryTypeItem
    {
        public Guid Id;
        public string Name;
        public Guid ParentId;
        public CategoryTypeSchemeItem Scheme;
        public string Code;
        public bool IsSystem;
    }

    public static class CategoryTypeType
    {
        public const string Party = "Party";
    }
}