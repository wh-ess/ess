#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.ReadModels
{
    public class DropDownView : ISubscribeTo<DropDownCreated>, ISubscribeTo<DropDownEdited>, ISubscribeTo<DropDownDeleted>
    {
        private readonly IRepositoryAsync<DropDownItem, Guid> _repositoryAsync;

        public DropDownView(IRepositoryAsync<DropDownItem, Guid> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<IEnumerable<DropDownItem>> GetDropDown(string key)
        {
            return await _repositoryAsync.FindAsync(c => c.Key.ToLower() == key.ToLower());
        }

        #region handle

        public void Handle(DropDownCreated e)
        {
            _repositoryAsync.AddAsync(e.Id, new DropDownItem { Id = e.Id, IsSystem = e.IsSystem, Key = e.Key, Text = e.Text, Value = e.Value });
        }

        public void Handle(DropDownDeleted e)
        {
            _repositoryAsync.DeleteAsync(e.Id);
        }

        public void Handle(DropDownEdited e)
        {
            _repositoryAsync.UpdateAsync(e.Id, new DropDownItem { Id = e.Id, IsSystem = e.IsSystem, Key = e.Key, Text = e.Text, Value = e.Value });
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