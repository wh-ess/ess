﻿#region

using System;
using ESS.Framework.Common.Components;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Common
{
    public class InitData
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