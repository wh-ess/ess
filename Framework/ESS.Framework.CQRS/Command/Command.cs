#region

using System;
using ESS.Framework.Common.Utilities;

#endregion

namespace ESS.Framework.CQRS.Command
{
    public abstract class Command : ICommand
    {
        protected Command()
        {
            Id = ObjectId.GetNextGuid();
        }

        public Guid Id { get; set; }
    }
}