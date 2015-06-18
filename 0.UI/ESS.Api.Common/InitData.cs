#region

using System;
using ESS.Framework.Common.Components;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Common
{
    public class InitData
    {
        private static MessageDispatcher _messageDispatcher;

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