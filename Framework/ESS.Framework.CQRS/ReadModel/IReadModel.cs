#region

using System.Collections;

#endregion

namespace ESS.Framework.CQRS.ReadModel
{
    public interface IReadModel
    {
        bool Clear();
        void Rebuild(IEnumerable events);

        IEnumerable GetAll();
    }
}