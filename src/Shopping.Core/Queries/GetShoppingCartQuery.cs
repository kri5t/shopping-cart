﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Core.Responses;
using Shopping.Database;
using Shopping.Database.Models;

namespace Shopping.Core.Queries
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

        public GetShoppingCartQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
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

            return new GetShoppingCartResponse(shoppingCart);
        }
    }

    public class GetShoppingCartResponse : BaseResponse
    {
        public GetShoppingCartResponse(ShoppingCart shoppingCart)
        {
            ShoppingCartResponse = new ShoppingCartResponse(shoppingCart);
        }
        public ShoppingCartResponse ShoppingCartResponse { get; set; }
    }
}