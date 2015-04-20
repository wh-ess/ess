#region

using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Category.Commands;
using ESS.Domain.Common.Category.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Category.Domain
{
    /// <summary>
    /// 用途,如产品,客户
    /// </summary>
    public class CategoryTypeScheme
        : Aggregate, IHandleCommand<CreateCategoryTypeScheme>, IHandleCommand<ChangeCategoryTypeSchemeName>, IHandleCommand<DeleteCategoryTypeScheme>,
            IApplyEvent<CategoryTypeSchemeCreated>, IApplyEvent<CategoryTypeSchemeNameChanged>, IApplyEvent<CategoryTypeSchemeDeleted>
    {
        public string Name { get; private set; }
        #region handle

        public IEnumerable Handle(CreateCategoryTypeScheme c)
        {
            var item = Mapper.DynamicMap<CategoryTypeSchemeCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteCategoryTypeScheme c)
        {
            var item = Mapper.DynamicMap<CategoryTypeSchemeDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeCategoryTypeSchemeName c)
        {
            var item = Mapper.DynamicMap<CategoryTypeSchemeNameChanged>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(CategoryTypeSchemeCreated e)
        {
            this.Name = e.Name;
        }

        public void Apply(CategoryTypeSchemeDeleted e)
        {
        }

        public void Apply(CategoryTypeSchemeNameChanged e)
        {
        }

        #endregion
    }
}