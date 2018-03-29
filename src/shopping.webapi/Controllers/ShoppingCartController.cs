using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Commands;
using Shopping.Core.Commands.ShoppingCart;
using Shopping.Core.Queries;
using Shopping.Core.Queries.ShoppingCart;

namespace Shopping.Webapi.Controllers
{
    /// <summary>
    /// Shopping cart controller - Get a new shopping cart
    /// </summary>
    [Route("api/v1/shoppingcarts")]
    public class ShoppingCartController : BaseController
    {
        private readonly IMediator _mediator;

        public ShoppingCartController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// List the shopping carts. Option to include items if needed
        /// </summary>
        /// <param name="includeItems"> Decides wether to include items or not - defaults to false </param>
        /// <returns> A list of shopping carts </returns>
        [HttpGet]
        public async Task<IActionResult> Get(bool includeItems = false)
        {
            return MapToResult(await _mediator.Send(new GetShoppingCartsQuery(includeItems)), 
                               result => Ok(result.ShoppingCarts));
        }

        /// <summary>
        /// Get a specific shopping cart including the items in it
        /// </summary>
        /// <param name="uid"> The uid of the shopping cart you want </param>
        /// <returns> A response with the shopping cart and items </returns>
        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(Guid uid)
        {
            return MapToResult(await _mediator.Send(new GetShoppingCartQuery(uid)), 
                result => Ok(result.ShoppingCartResponse));
        }

        /// <summary>
        /// Create a new shopping cart
        /// </summary>
        /// <returns> The uid of the newly created shopping cart </returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return MapToResult(await _mediator.Send(new CreateShoppingCartCommand(DateTimeOffset.UtcNow)), 
                result => Ok(new {result.ShoppingCartUid}));
        }

        /// <summary>
        /// Delete a shopping cart
        /// </summary>
        /// <param name="uid"> The uid of the shopping cart to delete </param>
        /// <returns> Ok if the cart was deleted </returns>
        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(Guid uid)
        {
            return MapToResult(await _mediator.Send(new DeleteShoppingCartCommand(uid)), 
                result => Ok());
        }
    }
}
