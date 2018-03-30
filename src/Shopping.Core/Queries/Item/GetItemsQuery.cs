using System;
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
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }
        
        public override async Task<GetItemsResponse> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var shoppingCart = 
                await _databaseContext.ShoppingCarts.Include(sc => sc.Items)
                    .SingleOrDefaultAsync(sc => sc.Uid == request.ShoppingCartUid, cancellationToken);
            
            if (shoppingCart == null)
                return Error(ErrorCode.NotFound, $"No shopping cart was found with {request.ShoppingCartUid}");

            return new GetItemsResponse
            {
                Items = shoppingCart.Items.Select(i => _mapper.Map<Database.Models.Item, ItemResponse>(i)).ToList()
            };
        }
    }
    
    public class GetItemsResponse : BaseResponse
    {
        public List<ItemResponse> Items { get; set; }
    }
}