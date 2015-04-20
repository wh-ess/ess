#region

using System;
using System.Collections.Generic;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.ReadModels
{
    public class DropDownView : ISubscribeTo<DropDownCreated>, ISubscribeTo<DropDownEdited>, ISubscribeTo<DropDownDeleted>
    {
        private readonly IRepository<DropDownItem, Guid> _repository;

        public DropDownView(IRepository<DropDownItem, Guid> repository)
        {
            _repository = repository;
        }

        public IEnumerable<DropDownItem> GetDropDown(string key)
        {
            return _repository.Find(c => c.Key.ToLower() == key.ToLower());
        }

        #region handle

        public void Handle(DropDownCreated e)
        {
            _repository.Add(e.Id, new DropDownItem { Id = e.Id, IsSystem = e.IsSystem, Key = e.Key, Text = e.Text, Value = e.Value });
        }

        public void Handle(DropDownDeleted e)
        {
            _repository.Delete(e.Id);
        }

        public void Handle(DropDownEdited e)
        {
            _repository.Update(e.Id, new DropDownItem { Id = e.Id, IsSystem = e.IsSystem, Key = e.Key, Text = e.Text, Value = e.Value });
        }

        #endregion
    }

    [Serializable]
    public class DropDownItem
    {
        public Guid Id;
        public bool IsSystem { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}