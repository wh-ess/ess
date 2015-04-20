using System;
using System.Collections.Generic;
using ESS.Domain.Foundation.AccessControl.Domain;
using ESS.Domain.Foundation.AccessControl.ReadModels;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.AccessControl.Events
{
    public class RoleCreated : Event
    {
        public string RoleName;
        public string Note;
    }

    public class RoleInfoChanged : RoleCreated
    {
    }

    public class RoleLocked : Event
    {
    }

    public class RoleUnlocked : Event
    {
    }

    public class UserAssigned : Event
    {
        public List<UserItem> Users;
    }

    public class PrivigeSet : Event
    {
        public List<Privige> Priviges;
    }
}
