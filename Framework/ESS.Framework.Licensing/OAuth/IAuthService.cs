using System;

using System.Threading.Tasks;

namespace ESS.Framework.Licensing.OAuth
{
    public interface IAuthService
    {
        Task<IUser> FindUser(string userName, string password);
    }

    public interface IUser
    {
        Guid Id { get; set; }
        string UserName { get; set; }
    }
}
