#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private readonly IRepository<CategoryTypeSchemeItem, Guid> _repository;

        public CategoryTypeSchemeView(IRepository<CategoryTypeSchemeItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<CategoryTypeSchemeItem> CategoryTypeSchemeList(Expression<Func<CategoryTypeSchemeItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<CategoryTypeSchemeItem> CategoryTypeSchemeList()
        {
            return _repository.GetAll();
        }

        public CategoryTypeSchemeItem GetCategoryTypeScheme(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(CategoryTypeSchemeCreated e)
        {
            var item = Mapper.DynamicMap<CategoryTypeSchemeItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(CategoryTypeSchemeDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(CategoryTypeSchemeNameChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<CategoryTypeSchemeItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        #endregion
    }

    [Serializable]
    public class CategoryTypeSchemeItem
    {
        public Guid Id;
        public string Name;
    }
}