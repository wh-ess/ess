using System;
using System.Collections.Generic;
using ESS.Domain.Foundation.AccessControl.Domain;
using ESS.Domain.Foundation.AccessControl.ReadModels;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Foundation.AccessControl.Commands
{
    public class CreateRole : Command
    {
        public string Note;
        public string RoleName;
    }

    public class ChangeRoleInfo : CreateRole
    {
    }

    public class LockRole : Command
    {
    }

    public class UnlockRole : Command
    {
    }

    public class AssignUser : Command
    {
        public List<UserItem> Users;
    }

    public class SetPrivige : Command
    {
        public List<Privige> Priviges;
    }
    
}