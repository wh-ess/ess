﻿#region

using System;
using System.Collections;
using System.Linq;
using System.Web.Http;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.ReadModels;
using ESS.Framework.Common.Utilities;
using ESS.Framework.CQRS;
using ESS.Framework.UI.Configurations;

#endregion

namespace ESS.Api.Foundation.EntityConfig
{
    public class DdlController : ApiController
    {
        private readonly MessageDispatcher _messageDispatcher;
        private readonly DropDownView _dropDownView;

        public DdlController(MessageDispatcher messageDispatcher, DropDownView dropDownView)
        {
            _messageDispatcher = messageDispatcher;
            _dropDownView = dropDownView;
        }
        public IEnumerable Get(string id)
        {
            if (ModuleBuilder.Enums.Any(c => c.Name == id))
            {
                return EnumExtensions.ToEnumClass(ModuleBuilder.Enums.First(c => c.Name == id));
            }
            return _dropDownView.GetDropDown(id);
        }

        [HttpPost]
        public IHttpActionResult Create(CreateDropDown ddl)
        {
            _messageDispatcher.SendCommand(ddl);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Edit(EditDropDown ddl)
        {
            _messageDispatcher.SendCommand(ddl);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            _messageDispatcher.SendCommand(new DeleteDropDown() { Id = id });
            return Ok();
        }
    }
}