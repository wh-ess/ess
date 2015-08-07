using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        private readonly IRepositoryAsync<RoleItem, Guid> _repositoryAsync;

        public RoleView(IRepositoryAsync<RoleItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<RoleItem>> RoleList()
        {
            return await _repositoryAsync.GetAllAsync();
        }
        public Task<RoleItem> GetRole(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        public async Task<IEnumerable<RoleItem>> RoleList(Expression<Func<RoleItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        #region handle
        public void Handle(RoleCreated e)
        {
            var item = Mapper.DynamicMap<RoleItem>(e);
            _repositoryAsync.AddAsync(item.Id, item);
        }

        public void Handle(RoleInfoChanged e)
        {
            Update(e.Id, c =>
            {
                c.Name = e.Name;
                c.Note = e.Note;
            });
       
        }

        public void Handle(RoleLocked e)
        {
            Update(e.Id, c => c.Locked = false);
        }

        public void Handle(RoleUnlocked e)
        {
            Update(e.Id, c => c.Locked = true);
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
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;
            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }

        #endregion


    }

    [Serializable]
    public class RoleItem
    {
        public Guid Id;
        [Required]
        public string Name;
        public string Note;
        public bool Locked;

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
