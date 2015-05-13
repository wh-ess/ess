using System;
using ESS.Framework.CQRS.Command;

namespace ESS.Domain.Foundation.AccessControl.Commands
{
    public class CreateUser : Command
    {
        public string No;
        public string Name;
        public string Password;

    }

    public class ChangeUserInfo : Command
    {
        public string No;
        public string Name;
    }

    public class ChangePassword : Command
    {
        public string Password;
        public string NewPassword;
    }

    public class LockUser : Command
    {
    }

    public class UnlockUser : Command
    {
    }
}
