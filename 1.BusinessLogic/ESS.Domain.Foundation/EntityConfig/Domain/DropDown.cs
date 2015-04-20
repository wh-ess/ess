#region

using System.Collections;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.Domain
{
    internal class DropDown
        : Aggregate, IHandleCommand<CreateDropDown>, IHandleCommand<EditDropDown>, IHandleCommand<DeleteDropDown>, IApplyEvent<DropDownCreated>,
            IApplyEvent<DropDownEdited>, IApplyEvent<DropDownDeleted>
    {
        #region handle

        public IEnumerable Handle(CreateDropDown c)
        {
            yield return new DropDownCreated { Id = c.Id, Key = c.Key, IsSystem = c.IsSystem, Text = c.Text, Value = c.Value };
        }

        public IEnumerable Handle(DeleteDropDown c)
        {
            yield return new DropDownDeleted { Id = c.Id };
        }

        public IEnumerable Handle(EditDropDown c)
        {
            yield return new DropDownEdited { Id = c.Id, Key = c.Key, IsSystem = c.IsSystem, Text = c.Text, Value = c.Value };
        }

        #endregion

        #region apply

        public void Apply(DropDownCreated e)
        {
        }

        public void Apply(DropDownDeleted e)
        {
        }

        public void Apply(DropDownEdited e)
        {
        }

        #endregion
    }
}