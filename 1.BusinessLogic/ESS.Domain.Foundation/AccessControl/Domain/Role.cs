#region

using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Foundation.AccessControl.Commands;
using ESS.Domain.Foundation.AccessControl.Events;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Foundation.AccessControl.Domain
{
    public class Role
        : Aggregate, IHandleCommand<CreateRole>, IHandleCommand<ChangeRoleInfo>, IHandleCommand<LockRole>, IHandleCommand<UnlockRole>,
            IHandleCommand<AssignUser>, IHandleCommand<SetPrivige>, IApplyEvent<RoleCreated>, IApplyEvent<RoleInfoChanged>, IApplyEvent<RoleLocked>,
            IApplyEvent<RoleUnlocked>, IApplyEvent<UserAssigned>, IApplyEvent<PrivigeSet>
    {
        private bool _locked;

        #region handle

        public IEnumerable Handle(AssignUser c)
        {
            yield return new UserAssigned { Id = c.Id, Users = c.Users };
        }

        public IEnumerable Handle(ChangeRoleInfo c)
        {
            var item = Mapper.DynamicMap<RoleInfoChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(CreateRole c)
        {
            var item = Mapper.DynamicMap<RoleCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(LockRole c)
        {
            if (_locked)
            {
                throw new InvalidOperationException("角色已锁定!");
            }
            yield return new RoleLocked { Id = c.Id };
        }

        public IEnumerable Handle(SetPrivige c)
        {
            yield return new PrivigeSet { Id = c.Id, Priviges = c.Priviges };
        }

        public IEnumerable Handle(UnlockRole c)
        {
            if (!_locked)
            {
                throw new InvalidOperationException("角色未锁定!");
            }
            yield return new RoleUnlocked { Id = c.Id };
        }

        #endregion

        #region apply

        public void Apply(PrivigeSet e)
        {
        }

        public void Apply(RoleCreated e)
        {
            _locked = false;
        }

        public void Apply(RoleInfoChanged e)
        {
        }

        public void Apply(RoleLocked e)
        {
            _locked = true;
        }

        public void Apply(RoleUnlocked e)
        {
            _locked = false;
        }

        public void Apply(UserAssigned e)
        {
        }

        #endregion
    }

    [Serializable]
    public class Privige
    {
        public ModuleItem Module { get; set; }
        public PrivigeOperation Operation { get; set; }
    }

    public enum PrivigeOperation
    {
        Inherit,
        Allow,
        Deny,
        Authoriz,
    }
}