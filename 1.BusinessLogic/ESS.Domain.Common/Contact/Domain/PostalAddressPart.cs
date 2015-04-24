using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Contact.Commands;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;
using ESS.Domain.Common.Contact.Events;

namespace ESS.Domain.Common.Contact.Domain
{
    public class PostalAddressPart: Aggregate, IHandleCommand<CreatePostalAddressPart>, IHandleCommand<ChangePostalAddressPart>, IHandleCommand<DeletePostalAddressPart>,
            IApplyEvent<PostalAddressPartCreated>, IApplyEvent<PostalAddressPartChanged>, IApplyEvent<PostalAddressPartDeleted>
    {
        public Guid PostalAddressPartId { get; set; }
        public Guid TypeId { get; set; }

        public Guid GeographicBoundaryId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Text { get; set; }

        #region handle

        public IEnumerable Handle(CreatePostalAddressPart c)
        {
            var item = Mapper.DynamicMap<PostalAddressPartCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeletePostalAddressPart c)
        {
            var item = Mapper.DynamicMap<PostalAddressPartDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangePostalAddressPart c)
        {
            var item = Mapper.DynamicMap<PostalAddressPartChanged>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(PostalAddressPartCreated e)
        {
        }

        public void Apply(PostalAddressPartDeleted e)
        {
        }

        public void Apply(PostalAddressPartChanged e)
        {
        }

        #endregion

    }
}
