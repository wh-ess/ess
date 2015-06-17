﻿#region

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
        private readonly IRepository<CategoryItem, Guid> _repository;

        public CategoryView(IRepository<CategoryItem, Guid> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<CategoryItem>> CategoryList(Expression<Func<CategoryItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public Task<IEnumerable<CategoryItem>> CategoryList()
        {
            return _repository.GetAll();
        }

        public Task<CategoryItem> GetCategory(Guid id)
        {
            return _repository.Get(id);
        }
        public Task<IEnumerable<CategoryItem>> GetCategoryBySchemeType(string schemeName, string typeName)
        {
            return _repository.Find(c => c.Type.Scheme.Name==schemeName && c.Type.Name == typeName);
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
            var item = _repository.Single(c => c.Id == id).Result;

            action.Invoke(item);
            _repository.Update(item.Id, item);
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