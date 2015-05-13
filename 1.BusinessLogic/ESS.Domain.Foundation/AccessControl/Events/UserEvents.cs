using System;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.AccessControl.Events
{
    public class UserCreated : Event
    {
        public string No;
        public string Name;
        public string Password;
    }

    public class UserInfoChanged : Event
    {
        public string No;
        public string Name;
    }

    public class UserPasswordChanged : Event
    {
        public string Password;
        public string NewPassword;
    }

    public class UserLocked : Event
    {
    }

    public class UserUnlocked : Event
    {
    }
}
