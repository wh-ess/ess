using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Foundation.AccessControl.Domain;
using ESS.Domain.Foundation.AccessControl.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

namespace ESS.Domain.Foundation.AccessControl.ReadModels
{
    public class RoleView :
        ISubscribeTo<RoleCreated>,
        ISubscribeTo<RoleInfoChanged>,
        ISubscribeTo<RoleLocked>,
        ISubscribeTo<RoleUnlocked>,
        ISubscribeTo<UserAssigned>,
        ISubscribeTo<PrivigeSet>
    {
        private readonly IRepository<RoleItem, Guid> _repository;

        public RoleView(IRepository<RoleItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<RoleItem> RoleList()
        {
            return _repository.GetAll();
        }
        public RoleItem GetRole(Guid id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<RoleItem> RoleList(Expression<Func<RoleItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        #region handle
        public void Handle(RoleCreated e)
        {
            var item = Mapper.DynamicMap<RoleItem>(e);
            _repository.Add(item.Id, item);
        }

        public void Handle(RoleInfoChanged e)
        {
            Update(e.Id, c =>
            {
                c.RoleName = e.RoleName;
                c.Note = e.Note;
            });
       
        }

        public void Handle(RoleLocked e)
        {
            Update(e.Id, c => c.IsActive = false);
        }

        public void Handle(RoleUnlocked e)
        {
            Update(e.Id, c => c.IsActive = true);
        }

        public void Handle(UserAssigned e)
        {
            Update(e.Id, c => c.Users = e.Users);
        }
        public void Handle(PrivigeSet e)
        {
            Update(e.Id, c => c.Priviges = e.Priviges);
        }
        private void Update(Guid id, Action<RoleItem> action)
        {
            var item = _repository.Single(c => c.Id == id);
            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        #endregion


    }

    [Serializable]
    public class RoleItem
    {
        public Guid Id;
        public string RoleName;
        public string Note;
        public bool IsActive;

        public List<UserItem> Users;
        public List<Privige> Priviges;

        public RoleItem()
        {
            Users = new List<UserItem>();
            Priviges = new List<Privige>();
        }
        public string UserNames
        {
            get { return string.Join(",", Users.Select(c => c.UserName)); }
        }
    }






}
