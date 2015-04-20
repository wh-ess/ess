using ESS.Domain.Foundation.AccessControl.Events;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Foundation.AccessControl.EventHandlers
{
    public class UserEventHandler :
        ISubscribeTo<UserCreated>
    {


        public void Handle(UserCreated e)
        {
            //todo 发邮件或信息
        }

    }
}
