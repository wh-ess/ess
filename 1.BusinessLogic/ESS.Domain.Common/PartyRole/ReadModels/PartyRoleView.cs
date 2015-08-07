#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Domain.Common.PartyRole.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.CQRS.ReadModel;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.PartyRole.ReadModels
{
    public class PartyRoleView :ReadModel, ISubscribeTo<PartyRoleCreated>, ISubscribeTo<PartyRoleEdited>, ISubscribeTo<PartyRoleDeleted>
    {
        private readonly IRepositoryAsync<PartyRoleItem, Guid> _repositoryAsync;

        public PartyRoleView(IRepositoryAsync<PartyRoleItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<PartyRoleItem>> PartyRoleList(Expression<Func<PartyRoleItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<PartyRoleItem>> PartyRoleList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<PartyRoleItem> GetPartyRole(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        
        #region handle

        public void Handle(PartyRoleCreated e)
        {
            var item = Mapper.DynamicMap<PartyRoleItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(PartyRoleEdited e)
        {
        }

        public void Handle(PartyRoleDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }

        private void Update(Guid id, Action<PartyRoleItem> action)
        {
            var item = _repositoryAsync.SingleAsync(c => c.Id == id).Result;

            action.Invoke(item);
            _repositoryAsync.UpdateAsync(item.Id, item);
        }

        public override Task<bool> Clear()
        {
            return _repositoryAsync.DeleteAllAsync();
        }

        public override async Task<IEnumerable> GetAll()
        {
            return await PartyRoleList();
        }
        #endregion
    }

    [Serializable]
    public class PartyRoleItem
    {
        public DateTime EndDate;
        public DateTime FromDate;
        public Guid Id;

        [Required]
        public PartyItem Party;

        [Required]
        public CategoryTypeItem Type;
    }
}