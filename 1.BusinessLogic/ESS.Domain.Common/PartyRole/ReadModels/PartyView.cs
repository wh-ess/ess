#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using ESS.Domain.Common.PartyRole.Events;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Common.PartyRole.ReadModels
{
    public class PartyView
        : ISubscribeTo<PartyCreated>,  ISubscribeTo<PartyDeleted>
    {
        private readonly IRepository<PartyItem, Guid> _repository;

        public PartyView(IRepository<PartyItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<PartyItem> PartyList(Expression<Func<PartyItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<PartyItem> PartyList()
        {
            return _repository.GetAll();
        }

        public PartyItem GetParty(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(PartyCreated e)
        {
            var item = Mapper.DynamicMap<PartyItem>(e);

            _repository.Add(e.Id, item);
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


        #endregion
    }

    [Serializable]
    public class PartyItem
    {
        public Guid Id;
        public string Name ;
        public string FullName ;
        public string FirstName ;
        public string LastName ;
        public DateTime BirthDay ;
        //身份证号(新)
        public string IcNo ;
        //身份证号（旧）
        public string IcNoOld ;
        //护照号
        public string Passport ;
        //员工相片路径
        public string Photo ;
    }
}