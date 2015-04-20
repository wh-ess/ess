using System.Collections;

namespace ESS.Framework.CQRS.Command
{
    public interface IHandleCommand<TCommand>
    {
        IEnumerable Handle(TCommand c);
    }
}
