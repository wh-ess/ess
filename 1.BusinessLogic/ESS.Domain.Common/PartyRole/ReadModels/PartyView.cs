#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IRepository<PartyItem, Guid> _repository;
        private readonly IRepository<PartyRoleItem, Guid> _partyRoleRepository;
        private readonly IRepository<CategoryItem, Guid> _categoryRepository;

        public PartyView(IRepository<PartyItem, Guid> repository, IRepository<PartyRoleItem, Guid> partyRoleRepository, IRepository<CategoryItem, Guid> categoryRepository)
        {
            _repository = repository;
            _partyRoleRepository = partyRoleRepository;
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<PartyItem> PartyList(Expression<Func<PartyItem, bool>> condition)
        {
            var partys = _repository.Find(condition).ToList();
            foreach (var p in partys)
            {
                AddRelation(p);
            }
            return partys;
        }

        public IEnumerable<PartyItem> PartyList()
        {
            var partys = _repository.GetAll().ToList();
            foreach (var p in partys)
            {
                AddRelation(p);
            }
            return partys;
        }

        public PartyItem GetParty(Guid id)
        {
            var p = _repository.Get(id);
            AddRelation(p);
            return p;
        }

        private void AddRelation(PartyItem p)
        {
            var partyRoles = _partyRoleRepository.Find(c => c.Party.Id == p.Id);
            foreach (var r in partyRoles)
            {
                p.PartyRoles.Add(r.Type.Name);
            }
        }
        #region handle

        public void Handle(PartyCreated e)
        {
            var item = Mapper.DynamicMap<PartyItem>(e);

            _repository.Add(e.Id, item);
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
            _repository.Delete(e.Id);
        }


        private void Update(Guid id, Action<PartyItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        public override bool Clear()
        {
            return _repository.DeleteAll();
        }

        public override IEnumerable GetAll()
        {
            return PartyList();
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