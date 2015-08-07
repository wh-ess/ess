#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using ESS.Domain.Mall.Pop.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.CQRS.ReadModel;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Mall.Pop.ReadModels
{
    public class PopTemplateView :ReadModel, ISubscribeTo<PopTemplateCreated>, ISubscribeTo<PopTemplateDeleted>, ISubscribeTo<PopTemplateEdited>
    {
        private readonly IRepositoryAsync<PopTemplateItem, Guid> _repositoryAsync;

        public PopTemplateView(IRepositoryAsync<PopTemplateItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<PopTemplateItem>> PopTemplateList(Expression<Func<PopTemplateItem, bool>> condition)
        {
            return await _repositoryAsync.FindAsync(condition);
        }

        public async Task<IEnumerable<PopTemplateItem>> PopTemplateList()
        {
            return await _repositoryAsync.GetAllAsync();
        }

        public Task<PopTemplateItem> GetPopTemplate(Guid id)
        {
            return _repositoryAsync.GetAsync(id);
        }

        #region handle

        public void Handle(PopTemplateCreated e)
        {
            var item = Mapper.DynamicMap<PopTemplateItem>(e);

            _repositoryAsync.AddAsync(e.Id, item);
        }

        public void Handle(PopTemplateEdited e)
        {
            var item = Mapper.DynamicMap<PopTemplateItem>(e);

            _repositoryAsync.UpdateAsync(e.Id, item);
        }

        public void Handle(PopTemplateDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }


        private void Update(Guid id, Action<PopTemplateItem> action)
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
            return await PopTemplateList();
        }

        #endregion

    }

    [Serializable]
    public class PopTemplateItem
    {
        public Guid Id;
        [Required]
        public string Name;
        [Required]
        public DateTime StartDate;
        [Required]
        public DateTime EndDate;
        [Required]
        public string Image;
        public bool IsEnable;
        public int Seq;
    }
}