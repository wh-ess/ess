#region

using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Category.Commands;
using ESS.Domain.Common.Category.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Category.Domain
{
    public class Category
        : Aggregate, IHandleCommand<CreateCategory>, IHandleCommand<ChangeCategoryName>, IHandleCommand<DeleteCategory>,
            IHandleCommand<ChangeCategoryParent>, IHandleCommand<ChangeCategoryType>, IHandleCommand<ChangeCategoryDate>, IApplyEvent<CategoryCreated>,
            IApplyEvent<CategoryNameChanged>, IApplyEvent<CategoryDeleted>, IApplyEvent<CategoryParentChanged>, IApplyEvent<CategoryTypeChanged>,
            IApplyEvent<CategoryDateChanged>
    {
        public Guid CategoryTypeId { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime FromDate { get; private set; }
        public string Name { get; private set; }
        public Guid ParentId { get; private set; }

        #region handle

        public IEnumerable Handle(CreateCategory c)
        {
            var item = Mapper.DynamicMap<CategoryCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteCategory c)
        {
            var item = Mapper.DynamicMap<CategoryDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeCategoryName c)
        {
            var item = Mapper.DynamicMap<CategoryNameChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeCategoryParent c)
        {
            var item = Mapper.DynamicMap<CategoryParentChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeCategoryType c)
        {
            var item = Mapper.DynamicMap<CategoryTypeChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeCategoryDate c)
        {
            var item = Mapper.DynamicMap<CategoryDateChanged>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(CategoryCreated e)
        {
        }

        public void Apply(CategoryDeleted e)
        {
        }

        public void Apply(CategoryNameChanged e)
        {
        }

        public void Apply(CategoryParentChanged e)
        {
        }

        public void Apply(CategoryTypeChanged e)
        {
        }

        public void Apply(CategoryDateChanged e)
        {
        }

        #endregion
    }
}