#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Domain.Common.PartyRole.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.PartyRole.ReadModels
{
    public class PartyRoleView
        : ISubscribeTo<PartyRoleCreated>, ISubscribeTo<PartyRoleEdited>, ISubscribeTo<PartyRoleDeleted>
    {
        private readonly IRepository<PartyRoleItem, Guid> _repository;
        private readonly IRepository<PartyItem, Guid> _partyRepository;
        private readonly IRepository<CategoryItem, Guid> _categoryRepository;

        public PartyRoleView(IRepository<PartyRoleItem, Guid> repository, IRepository<PartyItem, Guid> partyRepository, IRepository<CategoryItem, Guid> categoryRepository)
        {
            _repository = repository;
            _partyRepository = partyRepository;
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<PartyRoleItem> PartyRoleList(Expression<Func<PartyRoleItem, bool>> condition)
        {
            var partyRoles = _repository.Find(condition)
                   .ToList();
            foreach (var p in partyRoles)
            {
                AddRelation(p);
            }
            return partyRoles;
        }

        public IEnumerable<PartyRoleItem> PartyRoleList()
        {
            var partyRoles = _repository.GetAll()
                .ToList();
            foreach (var p in partyRoles)
            {
                AddRelation(p);
            }
            return partyRoles;
        }

        public PartyRoleItem GetPartyRole(Guid id)
        {
            var p =  _repository.Get(id);
            AddRelation(p);
            return p;
        }
        private void AddRelation(PartyRoleItem p)
        {
            var r = _partyRepository.First(c => c.Id == p.PartyId);
            if (r != null)
            {
                p.PartyName = r.Name;

                var c = _categoryRepository.Get(p.TypeId);
                p.TypeName = c.Name;
            }
        }
        #region handle

        public void Handle(PartyRoleCreated e)
        {
            var item = Mapper.DynamicMap<PartyRoleItem>(e);

            _repository.Add(e.Id, item);
        }
        public void Handle(PartyRoleEdited e)
        {
        }
        public void Handle(PartyRoleDeleted e)
        {
            _repository.Delete(e.Id);
        }

        private void Update(Guid id, Action<PartyRoleItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }


        #endregion
    }

    [Serializable]
    public class PartyRoleItem
    {
        public Guid Id;
        public Guid PartyId;
        public string PartyName;

        public Guid TypeId;
        public string TypeName;
        public DateTime FromDate;
        public DateTime EndDate;
    }
}