using System;
using System.Collections.Generic;
using ESS.Domain.Foundation.AccessControl.Commands;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Framework.Common.Components;
using ESS.Framework.CQRS;

namespace ESS.Api.Foundation
{
    public  class InitData
    {
        private static  MessageDispatcher _messageDispatcher;
        public InitData()
        {
            _messageDispatcher = ObjectContainer.Resolve<MessageDispatcher>();
        }
        public void Init()
        {
            var ran = new Random().Next();
        }
    }
}