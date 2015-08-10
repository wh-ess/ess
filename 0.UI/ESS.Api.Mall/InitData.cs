using System;
using System.Collections.Generic;
using ESS.Framework.Common.Components;
using ESS.Framework.CQRS;

namespace ESS.Api.Mall
{
    public  class InitData
    {
        private static DefaultMessageBus _messageDispatcher;
        public InitData()
        {
            _messageDispatcher = ObjectContainer.Resolve<DefaultMessageBus>();
        }
        public void Init()
        {
            var ran = new Random().Next();


        }
    }
}