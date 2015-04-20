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
    public class Brand : Aggregate, IHandleCommand<CreateBrand>, IHandleCommand<EditBrand>, IHandleCommand<DeleteBrand>, IApplyEvent<BrandCreated>,
            IApplyEvent<BrandEdited>, IApplyEvent<BrandDeleted>
    {
        #region handle

        public IEnumerable Handle(CreateBrand c)
        {
            var item = Mapper.DynamicMap<BrandCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteBrand c)
        {
            var item = Mapper.DynamicMap<BrandDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(EditBrand c)
        {
            var item = Mapper.DynamicMap<BrandEdited>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(BrandCreated e)
        {
        }

        public void Apply(BrandDeleted e)
        {
        }

        public void Apply(BrandEdited e)
        {
        }

        #endregion
    }
}
