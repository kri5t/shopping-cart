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
        
        /// <summary>
        /// Get the list of items for the shopping cart 
        /// </summary>
        /// <param name="shoppingCartUid"> Shopping cart uid to get items from </param>
        /// <returns> A list of items in the shopping cart </returns>
        [HttpGet]
        public async Task<IActionResult> Get(Guid shoppingCartUid)
        {
            return MapToResult(
                await _mediator.Send(new GetItemsQuery(shoppingCartUid)), 
                result => Ok(result.Items)); 
        }

        /// <summary>
        /// Get a specific item
        /// </summary>
        /// <param name="shoppingCartUid"> Uid for the shopping cart to get an item in </param>
        /// <param name="uid"> The uid for the item </param>
        /// <returns> The item </returns>
        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(Guid shoppingCartUid, Guid uid)
        {
            return MapToResult(
                await _mediator.Send(new GetItemQuery(uid)), 
                result => Ok(result.ItemResponse)); 
        }

        /// <summary>
        /// Create an item in a shopping cart
        /// </summary>
        /// <param name="shoppingCartUid"> The shopping cart to create it in </param>
        /// <param name="description"> The description to create item with</param>
        /// <param name="quantity"> The quantity to create item with </param>
        /// <returns> The uid of the item just created </returns>
        [HttpPost]
        public async Task<IActionResult> Post(Guid shoppingCartUid, [FromBody]CreateItemRequestModel model)
        {
            return MapToResult(
                await _mediator.Send(new CreateItemCommand(shoppingCartUid, DateTimeOffset.UtcNow, model.Description, model.Quantity)), 
                result => Ok(new {result.ItemUid}));
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="uid"> The uid of the item </param>
        /// <param name="description"> The description to update item with</param>
        /// <param name="quantity"> The quantity to update item with </param>
        /// <returns> Returns ok when done </returns>
        [HttpPut("{uid}")]
        public async Task<IActionResult> Put(Guid uid, [FromBody]UpdateItemRequestModel model)
        {
            return MapToResult(
                await _mediator.Send(new UpdateItemCommand(DateTimeOffset.UtcNow, uid, model.Description, model.Quantity)), 
                result => Ok());
        }
        
        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="uid"> The uid of the item to delete </param>
        /// <returns> Returns ok when done </returns>
        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(Guid uid)
        {
            return MapToResult(
                await _mediator.Send(new DeleteItemCommand(uid)), 
                result => Ok());
        }
    }
}