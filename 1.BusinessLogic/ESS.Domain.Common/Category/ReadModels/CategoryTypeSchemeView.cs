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
    public class CategoryTypeSchemeView
        : ISubscribeTo<CategoryTypeSchemeCreated>, ISubscribeTo<CategoryTypeSchemeNameChanged>, ISubscribeTo<CategoryTypeSchemeDeleted>
    {
        private readonly IRepositoryAsync<CategoryTypeSchemeItem, Guid> _repositoryAsync;

        public CategoryTypeSchemeView(IRepositoryAsync<CategoryTypeSchemeItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<CategoryTypeSchemeItem>> CategoryTypeSchemeList(Expression<Func<CategoryTypeSchemeItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<CategoryTypeSchemeItem>> CategoryTypeSchemeList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<CategoryTypeSchemeItem> GetCategoryTypeScheme(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(CategoryTypeSchemeCreated e)
        {
            var item = Mapper.DynamicMap<CategoryTypeSchemeItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(CategoryTypeSchemeDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        public void Handle(CategoryTypeSchemeNameChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<CategoryTypeSchemeItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }

        #endregion
    }

    [Serializable]
    public class CategoryTypeSchemeItem
    {
        public Guid Id;
        public string Name;
        public string Code;
        public bool IsSystem;
    }

    public static class CategoryTypeSchemeType
    {
        public const string Association = "Association";
    }
}