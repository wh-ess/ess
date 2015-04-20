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


            _messageDispatcher.SendCommand(new CreateBank() { Name = "中国工商银行", ShortName = "ICBC", Code = "ICBC" });
            _messageDispatcher.SendCommand(new CreateBank() { Name = "中国建设银行", ShortName = "CCB", Code = "CCB" });

            _messageDispatcher.SendCommand(new CreateFloor() { Name = "1F" });
            _messageDispatcher.SendCommand(new CreateFloor() { Name = "2F" });
            _messageDispatcher.SendCommand(new CreateFloor() { Name = "3F" });
            _messageDispatcher.SendCommand(new CreateFloor() { Name = "4F" });
            _messageDispatcher.SendCommand(new CreateFloor() { Name = "5F" });
            _messageDispatcher.SendCommand(new CreateFloor() { Name = "6F" });


            _messageDispatcher.SendCommand(new CreateBrandType() { Name = "黄金" });
            _messageDispatcher.SendCommand(new CreateBrandType() { Name = "男装" });
            _messageDispatcher.SendCommand(new CreateBrandType() { Name = "女装" });
            _messageDispatcher.SendCommand(new CreateBrandType() { Name = "电器" });
            _messageDispatcher.SendCommand(new CreateBrandType() { Name = "内衣" });
            _messageDispatcher.SendCommand(new CreateBrandType() { Name = "大客户" });


            _messageDispatcher.SendCommand(new CreateBrand() { Name = "周大幅" });
            _messageDispatcher.SendCommand(new CreateBrand() { Name = "九牧王" });
            _messageDispatcher.SendCommand(new CreateBrand() { Name = "美的" });
            _messageDispatcher.SendCommand(new CreateBrand() { Name = "格力" });
            _messageDispatcher.SendCommand(new CreateBrand() { Name = "鄂尔多斯" });
            _messageDispatcher.SendCommand(new CreateBrand() { Name = "绿茵阁" });
        }
    }
}