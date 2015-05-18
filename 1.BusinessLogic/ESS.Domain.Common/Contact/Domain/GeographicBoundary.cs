using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Contact.Commands;
using ESS.Domain.Common.Contact.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Contact.Domain
{
    public class GeographicBoundary : Aggregate, IHandleCommand<CreateGeographicBoundary>, IHandleCommand<ChangeGeographicBoundary>, IHandleCommand<DeleteGeographicBoundary>,
            IApplyEvent<GeographicBoundaryCreated>, IApplyEvent<GeographicBoundaryChanged>, IApplyEvent<GeographicBoundaryDeleted>
    {
        public Guid TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        //缩写
        public string Abbr { get; set; }
        public string InternetRegionCode { get; set; }
        #region handle

        public IEnumerable Handle(CreateGeographicBoundary c)
        {
            var item = Mapper.DynamicMap<GeographicBoundaryCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteGeographicBoundary c)
        {
            var item = Mapper.DynamicMap<GeographicBoundaryDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeGeographicBoundary c)
        {
            var item = Mapper.DynamicMap<GeographicBoundaryChanged>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(GeographicBoundaryCreated e)
        {
        }

        public void Apply(GeographicBoundaryDeleted e)
        {
        }

        public void Apply(GeographicBoundaryChanged e)
        {
        }

        #endregion
    }


}

