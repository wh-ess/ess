#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Foundation.AccessControl.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Foundation.AccessControl.ReadModels
{
    public class UserView
        : ISubscribeTo<UserCreated>, ISubscribeTo<UserInfoChanged>, ISubscribeTo<UserPasswordChanged>, ISubscribeTo<UserLocked>,
            ISubscribeTo<UserUnlocked>
    {
        private readonly IRepository<UserItem, Guid> _repository; 
        private readonly IRepository<RoleItem, Guid> _roleRepository;

        public UserView(IRepository<UserItem, Guid> repository, IRepository<RoleItem, Guid> roleRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
        }

        public IEnumerable<UserItem> UserList(Expression<Func<UserItem, bool>> condition)
        {
            var users = _repository.Find(condition)
                .ToList();
            foreach (var u in users)
            {
                AddRelation(u);
            }
            return users;
        }

        public IEnumerable<UserItem> UserList()
        {
            var users = _repository.GetAll()
                 .ToList();
            foreach (var u in users)
            {
                AddRelation(u);
            }
            return users;
        }

        public UserItem GetUser(Guid id)
        {
            var user = _repository.Get(id);
            AddRelation(user);
            return user;
        }

        public void AddRelation(UserItem user)
        {
            var roles = _roleRepository.Find(c => c.Users.Any(d => d.Id == user.Id));
            user.Roles = roles.Select(c=>c.Name).ToList();
        }

        #region handle

        public void Handle(UserCreated e)
        {
            var item = Mapper.DynamicMap<UserItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(UserLocked e)
        {
            Update(e.Id, c => c.Locked = true);
        }

        public void Handle(UserInfoChanged e)
        {
            Update(e.Id, c => c.Name = e.Name);
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
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        #endregion
    }

    [Serializable]
    public class UserItem
    {
        [Required]
        public Guid Id;
        public bool Locked;
        [Required]
        public string Name;
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