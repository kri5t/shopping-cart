using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Commands.Item;
using Shopping.Core.Queries.Item;
using Shopping.Webapi.RequestModel.Item;

namespace Shopping.Webapi.Controllers
{
    [Route("api/v1/shoppingcarts/{shoppingcartuid}/items")]
    public class ItemController : BaseController
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(Guid shoppingCartUid)
        {
            return MapToResult(
                await _mediator.Send(new GetItemsQuery(shoppingCartUid)), 
                result => Ok(result.Items)); 
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(Guid shoppingCartUid, Guid uid)
        {
            return MapToResult(
                await _mediator.Send(new GetItemQuery(uid)), 
                result => Ok(result.ItemResponse)); 
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid shoppingCartUid, [FromBody]CreateItemRequestModel model)
        {
            return MapToResult(
                await _mediator.Send(new CreateItemCommand(shoppingCartUid, DateTimeOffset.UtcNow, model.Description, model.Quantity)), 
                result => Ok(new {result.ItemUid}));
        }

        [HttpDelete("{uid}")]
        public void Delete(Guid shoppingCartUid, Guid uid)
        {
            throw new NotImplementedException();
        }
    }
}