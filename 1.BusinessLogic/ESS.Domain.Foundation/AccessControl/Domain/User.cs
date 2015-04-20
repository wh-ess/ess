using System;
using System.Collections;
using AutoMapper;
using ESS.Domain.Foundation.AccessControl.Commands;
using ESS.Domain.Foundation.AccessControl.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.AccessControl.Domain
{
    public class User:Aggregate,
        IHandleCommand<CreateUser>,
        IHandleCommand<ChangeUserInfo>,
        IHandleCommand<ChangePassword>,
        IHandleCommand<LockUser>,
        IHandleCommand<UnlockUser>,
        IApplyEvent<UserCreated>,
        IApplyEvent<UserInfoChanged>,
        IApplyEvent<UserPasswordChanged>,
        IApplyEvent<UserLocked>,
        IApplyEvent<UserUnlocked>
    {

        private bool _locked;

        #region handle
        public IEnumerable Handle(CreateUser c)
        {
            var item = Mapper.DynamicMap<UserCreated>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangeUserInfo c)
        {
            var item = Mapper.DynamicMap<UserInfoChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(ChangePassword c)
        {
            var item = Mapper.DynamicMap<UserPasswordChanged>(c);
            yield return item;
        }

        public IEnumerable Handle(LockUser c)
        {
            if (_locked) throw new InvalidOperationException("用户已锁定!");
            yield return new UserLocked() { Id = c.Id };
        }

        public IEnumerable Handle(UnlockUser c)
        {
            if (!_locked) throw new InvalidOperationException("角色未锁定!");
            yield return new UserUnlocked() { Id = c.Id };
        }

        #endregion

        #region apply
        public void Apply(UserCreated e)
        {
            _locked = false;
        }

        public void Apply(UserInfoChanged e)
        {
            
        }

        public void Apply(UserPasswordChanged e)
        {
            
        }

        public void Apply(UserLocked e)
        {
            _locked = true;
        }

        public void Apply(UserUnlocked e)
        {
            _locked = false;
        }
        #endregion
    }
}
