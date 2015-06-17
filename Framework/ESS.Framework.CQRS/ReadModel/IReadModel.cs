#region

using System.Collections;
using System.Threading.Tasks;

#endregion

namespace ESS.Framework.CQRS.ReadModel
{
    public interface IReadModel
    {
        Task<bool> Clear();
        void Rebuild(IEnumerable events);

        Task<IEnumerable> GetAll();
    }
}