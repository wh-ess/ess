#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Foundation.AccessControl.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;
using ESS.Framework.Licensing.OAuth;

#endregion

namespace ESS.Domain.Foundation.AccessControl.ReadModels
{
    public class UserView
        : IAuthService, ISubscribeTo<UserCreated>, ISubscribeTo<UserInfoChanged>, ISubscribeTo<UserPasswordChanged>, ISubscribeTo<UserLocked>,
            ISubscribeTo<UserUnlocked>
    {
        private readonly IRepositoryAsync<UserItem, Guid> _repositoryAsync;
        private readonly IRepositoryAsync<RoleItem, Guid> _roleRepositoryAsync;

        public UserView(IRepositoryAsync<UserItem, Guid> repositoryAsync, IRepositoryAsync<RoleItem, Guid> roleRepositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
            _roleRepositoryAsync = roleRepositoryAsync;
        }

        public async Task<IQueryable<UserItem>> UserList(Expression<Func<UserItem, bool>> condition)
        {
            var users = await _repositoryAsync.FindAsync(condition);
            foreach (var u in users)
            {
                AddRelation(u);
            }
            return users;
        }

        public async Task<IQueryable<UserItem>> UserList()
        {
            var users = await _repositoryAsync.GetAllAsync();
            foreach (var u in users)
            {
                AddRelation(u);
            }
            return users;
        }

        public UserItem GetUser(Guid id)
        {
            var user = _repositoryAsync.GetAsync(id).Result;
            AddRelation(user);
            return user;
        }

        public void AddRelation(UserItem user)
        {
            var roles = _roleRepositoryAsync.FindAsync(c => c.Users.Any(d => d.Id == user.Id)).Result;
            user.Roles = roles.Select(c => c.Name).ToList();
        }

        #region handle

        public void Handle(UserCreated e)
        {
            var item = Mapper.DynamicMap<UserItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(UserLocked e)
        {
            Update(e.Id, c => c.Locked = true);
        }

        public void Handle(UserInfoChanged e)
        {
            Update(e.Id, c => c.UserName = e.UserName);
        }

        public void Handle(UserPasswordChanged e)
        {
            Update(e.Id, c => c.Password = e.NewPassword);
        }

        public void Handle(UserUnlocked e)
        {
            Update(e.Id, c => c.Locked = false);
        }

        private void Update(Guid id, Action<UserItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }

        #endregion

        public async Task<IUser> FindUser(string userName, string password)
        {
            return await _repositoryAsync.FirstAsync(c => c.UserName == userName && c.Password == password);
        }
    }

    [Serializable]
    public class UserItem : IUser
    {
        [Required]
        public Guid Id { get; set; }
        public bool Locked;
        [Required]
        public string UserName { get; set; }
        [Required]
        public string No;
        public string Password;
        public List<string> Roles;

        public UserItem()
        {
            Roles = new List<string>();
        }
    }
}