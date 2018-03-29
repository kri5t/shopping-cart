using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Core.Responses;
using Shopping.Database;

namespace Shopping.Core.Queries.Item
{
    public class GetItemsQuery : IRequest<GetItemsResponse>
    {
        public Guid ShoppingCartUid { get; }

        public GetItemsQuery(Guid shoppingCartUid)
        {
            ShoppingCartUid = shoppingCartUid;
        }
    }
    
    [UsedImplicitly]
    public class GetItemsQueryHandler : BaseHandler<GetItemsQuery, GetItemsResponse>
    {
        private readonly DatabaseContext _databaseContext;
        
        public GetItemsQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public override async Task<GetItemsResponse> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var shoppingCart = 
                await _databaseContext.ShoppingCarts.SingleOrDefaultAsync(sc => sc.Uid == request.ShoppingCartUid, cancellationToken);
            
            if (shoppingCart == null)
                return Error(ErrorCode.NotFound, $"No shopping cart was found with {request.ShoppingCartUid}");

            return new GetItemsResponse
            {
                Items = shoppingCart.Items.Select(i => new ItemResponse(i)).ToList()
            };
        }
    }
    
    public class GetItemsResponse : BaseResponse
    {
        public List<ItemResponse> Items { get; set; }
    }
}