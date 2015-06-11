#region

using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Mall.Pop.Commands;
using ESS.Domain.Mall.Pop.Events;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Mall.Pop.Domain
{
    public class PopTemplate
        : Aggregate, IHandleCommand<CreatePopTemplate>, IHandleCommand<EditPopTemplate>, IHandleCommand<DeletePopTemplate>, IApplyEvent<PopTemplateCreated>,
            IApplyEvent<PopTemplateDeleted>, IApplyEvent<PopTemplateEdited>
    {

        #region handle

        public IEnumerable Handle(CreatePopTemplate c)
        {
            var item = Mapper.DynamicMap<PopTemplateCreated>(c);
            yield return item;
        }
        public IEnumerable Handle(EditPopTemplate c)
        {
            var item = Mapper.DynamicMap<PopTemplateEdited>(c);
            yield return item;
        }
        public IEnumerable Handle(DeletePopTemplate c)
        {
            var item = Mapper.DynamicMap<PopTemplateDeleted>(c);
            yield return item;
        }

        #endregion

        #region apply

        public void Apply(PopTemplateCreated e)
        {
        }
        public void Apply(PopTemplateEdited e)
        {
        }
        public void Apply(PopTemplateDeleted e)
        {
        }

        #endregion
    }
}