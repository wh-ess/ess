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
    public class CategoryTypeView
        : ISubscribeTo<CategoryTypeCreated>, ISubscribeTo<CategoryTypeNameChanged>, ISubscribeTo<CategoryTypeDeleted>,
            ISubscribeTo<CategoryTypeParentChanged>, ISubscribeTo<CategoryTypeSchemeChanged>,
            ISubscribeTo<CategoryTypeSchemeNameChanged>
    {
        private readonly IRepository<CategoryTypeItem, Guid> _repository;

        public CategoryTypeView(IRepository<CategoryTypeItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<CategoryTypeItem> CategoryTypeList(Expression<Func<CategoryTypeItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<CategoryTypeItem> CategoryTypeList()
        {
            return _repository.GetAll();
        }

        public CategoryTypeItem GetCategoryType(Guid id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<CategoryTypeItem> GetCategoryTypeByScheme(string name)
        {
            return _repository.Find(c => c.Scheme.Name == name);
        }

        #region handle

        public void Handle(CategoryTypeCreated e)
        {
            var item = Mapper.DynamicMap<CategoryTypeItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(CategoryTypeDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(CategoryTypeNameChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<CategoryTypeItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
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
}