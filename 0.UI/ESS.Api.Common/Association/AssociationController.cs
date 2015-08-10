#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ESS.Domain.Common.Association.Commands;
using ESS.Domain.Common.Association.ReadModels;
using ESS.Framework.CQRS;

#endregion

namespace ESS.Api.Common.Association
{
    public class AssociationController : ApiController
    {
        private readonly AssociationView _associationView;
        private readonly DefaultMessageBus _messageDispatcher;

        public AssociationController(DefaultMessageBus messageDispatcher, AssociationView associationView)
        {
            _messageDispatcher = messageDispatcher;
            _associationView = associationView;
        }

        public async Task<IEnumerable<AssociationItem>> Get()
        {
            return await _associationView.AssociationListAsync();
        }

        public Task<AssociationItem> Get(Guid id)
        {
            return _associationView.GetAssociationAsync(id);
        }

        [HttpPost]
        public IHttpActionResult CreateAssociation(CreateAssociation association)
        {
            _messageDispatcher.SendCommand(association);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditAssociation(EditAssociation association)
        {
            _messageDispatcher.SendCommand(association);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAssociation(Guid id)
        {
            _messageDispatcher.SendCommand(new DeleteAssociation(){Id=id});
            return Ok();
        }
    }
}