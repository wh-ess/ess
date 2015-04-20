#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.Basic.ReadModels
{
    public class BankView : ISubscribeTo<BankCreated>, ISubscribeTo<BankEdited>, ISubscribeTo<BankDeleted>
    {
        private readonly IRepository<BankItem, Guid> _repository;

        public BankView(IRepository<BankItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<BankItem> BankList(Expression<Func<BankItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<BankItem> BankList()
        {
            return _repository.GetAll();
        }

        public BankItem GetBank(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(BankCreated e)
        {
            var item = Mapper.DynamicMap<BankItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(BankDeleted e)
        {
            _repository.Delete(e.Id);
        }


        public void Handle(BankEdited e)
        {
            Update(e.Id, c => c.Name = e.Name);
        }


        private void Update(Guid id, Action<BankItem> action)
        {
            var item = _repository.Single(c => c.Id == id);

            action.Invoke(item);
            _repository.Update(item.Id, item);
        }

        #endregion
    }

    [Serializable]
    public class BankItem
    {
        public Guid Id;
        public string Code;
        public string Name;
        public string ShortName;
        public bool IsActive;
    }
}