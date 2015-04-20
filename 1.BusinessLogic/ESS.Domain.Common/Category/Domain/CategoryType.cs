using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Category.Commands;
using ESS.Domain.Common.Category.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Category.Domain
{
    /// <summary>
    /// 类型,如品牌,产品规格
    /// </summary>
    public class CategoryType : Aggregate, IHandleCommand<CreateCategoryType>, IHandleCommand<ChangeCategoryTypeName>, IHandleCommand<DeleteCategoryType>,
            IHandleCommand<ChangeCategoryTypeParent>,IHandleCommand<ChangeCategoryTypeScheme>,
            IApplyEvent<CategoryTypeCreated>, IApplyEvent<CategoryTypeNameChanged>, IApplyEvent<CategoryTypeDeleted>,
            IApplyEvent<CategoryTypeParentChanged>, IApplyEvent<CategoryTypeSchemeChanged>
    {
        #region handle

        public IEnumerable Handle(CreateCategoryType c)
        {
            var item = Mapper.DynamicMap<CategoryTypeCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteCategoryType c)
        {
            var item = Mapper.DynamicMap<CategoryTypeDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeCategoryTypeName c)
        {
            var item = Mapper.DynamicMap<CategoryTypeNameChanged>(c);
            yield return item;
        }
        public IEnumerable Handle(ChangeCategoryTypeParent c)
        {
            var item = Mapper.DynamicMap<CategoryTypeParentChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeCategoryTypeScheme c)
        {
            var item = Mapper.DynamicMap<CategoryTypeSchemeChanged>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(CategoryTypeCreated e)
        {
        }

        public void Apply(CategoryTypeDeleted e)
        {
        }

        public void Apply(CategoryTypeNameChanged e)
        {
        }
        public void Apply(CategoryTypeParentChanged e)
        {
        }

        public void Apply(CategoryTypeSchemeChanged e)
        {
        }

        #endregion

        

    }
}
