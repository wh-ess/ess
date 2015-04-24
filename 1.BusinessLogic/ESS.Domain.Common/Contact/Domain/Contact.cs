#region

using System.Collections;
using AutoMapper;
using ESS.Domain.Common.Contact.Commands;
using ESS.Domain.Common.Contact.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Common.Contact.Domain
{
    public class Contact
        : Aggregate, IHandleCommand<CreateContact>, IHandleCommand<ChangeContact>, IHandleCommand<DeleteContact>,
            IApplyEvent<ContactCreated>, IApplyEvent<ContactChanged>, IApplyEvent<ContactDeleted>
    {
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }

        #region handle

        public IEnumerable Handle(CreateContact c)
        {
            var item = Mapper.DynamicMap<ContactCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(DeleteContact c)
        {
            var item = Mapper.DynamicMap<ContactDeleted>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeContact c)
        {
            var item = Mapper.DynamicMap<ContactChanged>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(ContactCreated e)
        {
        }

        public void Apply(ContactDeleted e)
        {
        }

        public void Apply(ContactChanged e)
        {
        }

        #endregion
    }
}