using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Commands.Item;

namespace Shopping.Webapi.Controllers
{
    [Route("api/v1/shoppingcart/{shoppingcartuid}/item")]
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
            throw new NotImplementedException();
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(Guid shoppingCartUid, Guid uid)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid shoppingCartUid, [FromBody]string description, [FromBody] int quantity)
        {
            return MapToResult(
                await _mediator.Send(new CreateItemCommand(shoppingCartUid, DateTimeOffset.UtcNow, description, quantity)), 
                result => Ok(new {result.ItemUid}));
        }

        [HttpDelete("{uid}")]
        public void Delete(Guid shoppingCartUid, Guid uid)
        {
            throw new NotImplementedException();
        }
    }
}