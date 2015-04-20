using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Basic.Commands;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Basic.Domain
{
    public class BrandType : Aggregate, IHandleCommand<CreateBrandType>, IHandleCommand<EditBrandType>, IHandleCommand<DeleteBrandType>, IApplyEvent<BrandTypeCreated>,
            IApplyEvent<BrandTypeEdited>, IApplyEvent<BrandTypeDeleted>
    {
        #region handle

        public IEnumerable Handle(CreateBrandType c)
        {
            var item = Mapper.DynamicMap<BrandTypeCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteBrandType c)
        {
            var item = Mapper.DynamicMap<BrandTypeDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(EditBrandType c)
        {
            var item = Mapper.DynamicMap<BrandTypeEdited>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(BrandTypeCreated e)
        {
        }

        public void Apply(BrandTypeDeleted e)
        {
        }

        public void Apply(BrandTypeEdited e)
        {
        }

        #endregion
    }
}
