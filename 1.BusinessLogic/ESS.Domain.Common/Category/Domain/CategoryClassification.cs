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
    public class CategoryClassification
        : Aggregate, IHandleCommand<CreateCategoryClassification>, IHandleCommand<DeleteCategoryClassification>,
            IApplyEvent<CategoryClassificationCreated>, IApplyEvent<CategoryClassificationDeleted>
    {
        public Guid RelateId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        #region handle

        public IEnumerable Handle(CreateCategoryClassification c)
        {
            var item = Mapper.DynamicMap<CategoryClassificationCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteCategoryClassification c)
        {
            var item = Mapper.DynamicMap<CategoryClassificationDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(CategoryClassificationCreated e)
        {
        }

        public void Apply(CategoryClassificationDeleted e)
        {
        }

        #endregion
    }
}