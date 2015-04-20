using System;

namespace ESS.Framework.CQRS.Command
{
    public interface ICommand
    {
        Guid Id { get; set; }
    }
}
