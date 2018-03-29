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

namespace Shopping.Core.Queries
{
    public class GetShoppingCartsQuery : IRequest<GetShoppingCartsResponse>
    {
        public bool IncludeItems { get; }

        public GetShoppingCartsQuery(bool includeItems = false)
        {
            IncludeItems = includeItems;
        }
    }

    [UsedImplicitly]
    public class GetShoppingCartsQueryHandler : BaseHandler<GetShoppingCartsQuery, GetShoppingCartsResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public GetShoppingCartsQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public override Task<GetShoppingCartsResponse> Handle(GetShoppingCartsQuery request, CancellationToken cancellationToken)
        {
            List<ShoppingCartResponse> shoppingCarts;
            if (request.IncludeItems)
            {
                shoppingCarts = _databaseContext.ShoppingCarts
                    .Include(sc => sc.Items)
                    .Select(sc => new ShoppingCartResponse(sc))
                    .ToList();
            }
            else
            {
                shoppingCarts = _databaseContext.ShoppingCarts
                    .Select(sc => new ShoppingCartResponse(sc))
                    .ToList();
            }
            return Task.FromResult(new GetShoppingCartsResponse
            {
                ShoppingCarts = shoppingCarts
            });
        }
    }

    public class GetShoppingCartsResponse : BaseResponse
    {
        public List<ShoppingCartResponse> ShoppingCarts { get; set; }
    }
}