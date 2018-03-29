using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Get(bool includeItems = false)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(Guid uid)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{uid}")]
        public void Delete(Guid uid)
        {
            throw new NotImplementedException();
        }
    }
}