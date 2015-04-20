#region

using System;
using System.Collections;
using System.Linq;
using AutoMapper;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.Common.Utilities;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.Domain
{
    public class Field : Aggregate, IHandleCommand<SaveFields>, IApplyEvent<FieldsSaved>
    {
        #region handle

        public IEnumerable Handle(SaveFields c)
        {
            foreach (var item in c.Fields.Where(item => item.Id == default(Guid)))
            {
                item.Id = ObjectId.GetNextGuid();
            }
            yield return new FieldsSaved() { Id = c.Id, ModuleNo = c.ModuleNo,ActionName = c.ActionName, Fields = c.Fields };

        }
        #endregion

        #region apply

        public void Apply(FieldsSaved e)
        {

        }
        #endregion




    }
}