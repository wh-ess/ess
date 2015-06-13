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
    public class PartyRoleView :ReadModel, ISubscribeTo<PartyRoleCreated>, ISubscribeTo<PartyRoleEdited>, ISubscribeTo<PartyRoleDeleted>
    {
        private readonly IRepository<PartyRoleItem, Guid> _repository;

        public PartyRoleView(IRepository<PartyRoleItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<PartyRoleItem> PartyRoleList(Expression<Func<PartyRoleItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<PartyRoleItem> PartyRoleList()
        {
            return _repository.GetAll();
        }

        public PartyRoleItem GetPartyRole(Guid id)
        {
            return _repository.Get(id);
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

        public override bool Clear()
        {
            return _repository.DeleteAll();
        }

        public override IEnumerable GetAll()
        {
            return PartyRoleList();
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