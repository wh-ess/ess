using System;
using ESS.Domain.Common.Basic.Commands;
using ESS.Framework.Common.Components;
using ESS.Framework.CQRS;

namespace ESS.Api.Common
{
    public  class InitData
    {
        private static MessageDispatcher _messageDispatcher;
        public InitData()
        {
            _messageDispatcher = ObjectContainer.Resolve<MessageDispatcher>();
        }
        public  void Init()
        {
            var ran = new Random().Next();


        }
    }
}