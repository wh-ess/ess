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
    public class CategoryView
        : ISubscribeTo<CategoryCreated>, ISubscribeTo<CategoryNameChanged>, ISubscribeTo<CategoryDeleted>, ISubscribeTo<CategoryParentChanged>,
            ISubscribeTo<CategoryTypeChanged>, ISubscribeTo<CategoryDateChanged>
    {
        private readonly IRepository<CategoryItem, Guid> _repository;

        public CategoryView(IRepository<CategoryItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<CategoryItem> CategoryList(Expression<Func<CategoryItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<CategoryItem> CategoryList()
        {
            return _repository.GetAll();
        }

        public CategoryItem GetCategory(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(CategoryCreated e)
        {
            var item = Mapper.DynamicMap<CategoryItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(CategoryDeleted e)
        {
            _repository.Delete(e.Id);
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
            Update(e.Id, c => c.TypeId = e.TypeId);
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
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        #endregion
    }

    [Serializable]
    public class CategoryItem
    {
        public Guid TypeId;
        public DateTime EndDate;
        public DateTime FromDate;
        public Guid Id;
        public string Name;
        public Guid ParentId;
    }
}