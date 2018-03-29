using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Commands;
using Shopping.Core.Queries;

namespace Shopping.Webapi.Controllers
{
    [Route("api/v1/shoppingcart")]
    public class ShoppingCartController : BaseController
    {
        private readonly IMediator _mediator;

        public ShoppingCartController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(bool includeItems = false)
        {
            return MapToResult(await _mediator.Send(new GetShoppingCartsQuery(includeItems)), 
                               result => Ok(result.ShoppingCarts));
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(Guid uid)
        {
            return MapToResult(await _mediator.Send(new GetShoppingCartQuery(uid)), 
                result => Ok(result.ShoppingCartResponse));
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return MapToResult(await _mediator.Send(new CreateShoppingCartCommand(DateTimeOffset.UtcNow)), 
                result => Ok(new {result.ShoppingCartUid}));
        }

        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(Guid uid)
        {
            return MapToResult(await _mediator.Send(new DeleteShoppingCartCommand(uid)), 
                result => Ok());
        }
    }
}
