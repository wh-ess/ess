#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Common.Association.Domain;
using ESS.Domain.Common.Association.Events;
using ESS.Domain.Common.Category.ReadModels;
using ESS.Domain.Common.PartyRole.ReadModels;
using ESS.Framework.CQRS.Event;
using ESS.Framework.CQRS.ReadModel;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Association.ReadModels
{
    public class AssociationView
        :ReadModel, ISubscribeTo<AssociationCreated>, ISubscribeTo<AssociationDeleted>, ISubscribeTo<AssociationEdited>
    {
        private readonly IRepositoryAsync<AssociationItem, Guid> _repositoryAsync;

        public AssociationView(IRepositoryAsync<AssociationItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IQueryable<AssociationItem>> AssociationListAsync(Expression<Func<AssociationItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IQueryable<AssociationItem>> AssociationListAsync()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<AssociationItem> GetAssociationAsync(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(AssociationCreated e)
        {
            var item = Mapper.DynamicMap<AssociationItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }
        public void Handle(AssociationEdited e)
        {
            var item = Mapper.DynamicMap<AssociationItem>(e);

            _repositoryAsync.UpdateAsync(e.Id, item);
        }

        public void Handle(AssociationDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }

        private void Update(Guid id, Action<AssociationItem> action)
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
            return await AssociationListAsync();
        }

        #endregion
    }

    [Serializable]
    public class AssociationItem
    {
        public Guid Id;
        public Guid From;
        public Guid To;
        public AssociationRule AssociationRule;
        public CategoryTypeItem Type;
        public DateTime FromDate;
        public DateTime EndDate;

        public string Code;
        public bool IsSystem;
    }
    
}