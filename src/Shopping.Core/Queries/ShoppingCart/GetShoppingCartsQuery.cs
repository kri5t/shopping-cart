using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;
using Shopping.Models.Responses;

namespace Shopping.Core.Queries.ShoppingCart
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
        private readonly IMapper _mapper;

        public GetShoppingCartsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }
        public override Task<GetShoppingCartsResponse> Handle(GetShoppingCartsQuery request, CancellationToken cancellationToken)
        {
            List<ShoppingCartResponse> shoppingCarts;
            if (request.IncludeItems)
            {
                shoppingCarts = _databaseContext.ShoppingCarts
                    .Include(sc => sc.Items)
                    .Select(sc => _mapper.Map<Database.Models.ShoppingCart, ShoppingCartResponse>(sc))
                    .ToList();
            }
            else
            {
                shoppingCarts = _databaseContext.ShoppingCarts
                    .Select(sc => _mapper.Map<Database.Models.ShoppingCart, ShoppingCartResponse>(sc))
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