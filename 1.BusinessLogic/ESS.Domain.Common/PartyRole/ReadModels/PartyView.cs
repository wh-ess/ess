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
    public class PartyView
        :ReadModel, ISubscribeTo<PartyCreated>, ISubscribeTo<PartyNameChanged>, ISubscribeTo<PartyDeleted>
    {
        private readonly IRepositoryAsync<PartyItem, Guid> _repositoryAsync;
        private readonly IRepositoryAsync<PartyRoleItem, Guid> _partyRoleRepositoryAsync;
        private readonly IRepositoryAsync<CategoryItem, Guid> _categoryRepositoryAsync;

        public PartyView(IRepositoryAsync<PartyItem, Guid> repositoryAsync, IRepositoryAsync<PartyRoleItem, Guid> partyRoleRepositoryAsync, IRepositoryAsync<CategoryItem, Guid> categoryRepositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
            _partyRoleRepositoryAsync = partyRoleRepositoryAsync;
            _categoryRepositoryAsync = categoryRepositoryAsync;
        }

        public IEnumerable<PartyItem> PartyList(Expression<Func<PartyItem, bool>> condition)
        {
            var partys = _repositoryAsync.FindAsync(condition).Result.ToList();
            foreach (var p in partys)
            {
                AddRelation(p);
            }
            return partys;
        }

        public IEnumerable<PartyItem> PartyList()
        {
            var partys = _repositoryAsync.GetAllAsync().Result.ToList();
            foreach (var p in partys)
            {
                AddRelation(p);
            }
            return partys;
        }

        public PartyItem GetParty(Guid id)
        {
            var p = _repositoryAsync.GetAsync(id).Result;
            AddRelation(p);
            return p;
        }

        private void AddRelation(PartyItem p)
        {
            var partyRoles = _partyRoleRepositoryAsync.FindAsync(c => c.Party.Id == p.Id).Result;
            foreach (var r in partyRoles)
            {
                p.PartyRoles.Add(r.Type.Name);
            }
        }
        #region handle

        public void Handle(PartyCreated e)
        {
            var item = Mapper.DynamicMap<PartyItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }
        public void Handle(PartyNameChanged e)
        {
            Update(e.Id,c=>
            {
                c.Name = e.Name;
            });
        }
        public void Handle(PartyDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        private void Update(Guid id, Action<PartyItem> action)
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
            return await Task.FromResult(PartyList());
        }
        #endregion
    }

    [Serializable]
    public class PartyItem
    {

        public Guid Id;
        [Required]
        public string Name;
        public string Photo;

        public List<string> PartyRoles;

        public PartyItem()
        {
            PartyRoles = new List<string>();
        }
    }
}