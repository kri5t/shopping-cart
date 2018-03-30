using System;
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
    public class GetShoppingCartQuery : IRequest<GetShoppingCartResponse>
    {
        public Guid ShoppingCartUid { get; }

        public GetShoppingCartQuery(Guid shoppingCartUid)
        {
            ShoppingCartUid = shoppingCartUid;
        }
    }
    
    [UsedImplicitly]
    public class GetShoppingCartQueryHandler : BaseHandler<GetShoppingCartQuery, GetShoppingCartResponse>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public GetShoppingCartQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }
        
        public override async Task<GetShoppingCartResponse> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
        {
            if (request.ShoppingCartUid == Guid.Empty)
                return Error(ErrorCode.NotValid, $"{request.ShoppingCartUid} is not a valid uid");

            var shoppingCart = 
                await _databaseContext.ShoppingCarts
                                      .Include(sc => sc.Items)
                                      .SingleOrDefaultAsync(x => x.Uid == request.ShoppingCartUid, cancellationToken);
            
            if (shoppingCart == null)
                return Error(ErrorCode.NotFound, "No shopping cart with the uid was found");

            return new GetShoppingCartResponse
            {
                ShoppingCartResponse = _mapper.Map<Database.Models.ShoppingCart, ShoppingCartResponse>(shoppingCart)
            };
        }
    }

    public class GetShoppingCartResponse : BaseResponse
    {
        public ShoppingCartResponse ShoppingCartResponse { get; set; }
    }
}