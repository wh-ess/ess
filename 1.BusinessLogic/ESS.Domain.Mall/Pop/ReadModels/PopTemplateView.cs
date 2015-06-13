#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private readonly IRepository<PopTemplateItem, Guid> _repository;

        public PopTemplateView(IRepository<PopTemplateItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<PopTemplateItem> PopTemplateList(Expression<Func<PopTemplateItem, bool>> condition)
        {
            return _repository.Find(condition);
        }

        public IEnumerable<PopTemplateItem> PopTemplateList()
        {
            return _repository.GetAll();
        }

        public PopTemplateItem GetPopTemplate(Guid id)
        {
            return _repository.Get(id);
        }

        #region handle

        public void Handle(PopTemplateCreated e)
        {
            var item = Mapper.DynamicMap<PopTemplateItem>(e);

            _repository.Add(e.Id, item);
        }

        public void Handle(PopTemplateEdited e)
        {
            var item = Mapper.DynamicMap<PopTemplateItem>(e);

            _repository.Update(e.Id, item);
        }

        public void Handle(PopTemplateDeleted e)
        {
            _repository.Delete(e.Id);
        }


        private void Update(Guid id, Action<PopTemplateItem> action)
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
            return PopTemplateList();
        }

        #endregion

    }

    [Serializable]
    public class PopTemplateItem
    {
        public DateTime EndDate;
        public Guid Id;
        public string Image;
        public bool IsEnable;
        public string Name;
        public int Seq;
        public DateTime StartDate;
    }
}