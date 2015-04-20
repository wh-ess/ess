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
             
            //role
            _messageDispatcher.SendCommand(new CreateRole() { RoleName = "RoleName" + ran });

            //user
            _messageDispatcher.SendCommand(new CreateUser() { UserName = "UserName" + ran, UserNo = "UserNo" + ran });

            //ddl
            _messageDispatcher.SendCommand(new CreateDropDown() { Key = "Type", Text = "ddl" + ran, Value = "ddl" + ran });
            _messageDispatcher.SendCommand(new CreateDropDown() { Key = "Type", Text = "ddl2" + ran, Value = "ddl2" + ran });
        }
    }
}