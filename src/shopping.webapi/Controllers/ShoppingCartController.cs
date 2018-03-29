using System;
using System.Collections.Generic;
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
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "SOMETHING NEW" };
        }

        // GET api/values/5
        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(Guid uid)
        {
            var result = await _mediator.Send(new GetShoppingCartQuery(uid));
            if (result.HasError)
                return Error(result);
            
            return Ok(result.ShoppingCartResponse);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var result = await _mediator.Send(new CreateShoppingCartCommand(DateTimeOffset.UtcNow));
            if (result.HasError)
                return Error(result);
            
            return Ok(result.ShoppingCartUid);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
