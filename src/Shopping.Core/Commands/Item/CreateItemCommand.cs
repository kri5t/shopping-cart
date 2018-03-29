﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;

namespace Shopping.Core.Commands.Item
{
    public class CreateItemCommand : IRequest<CreateItemResponse>
    {
        public Guid ShoppingCartUid { get; }
        public DateTimeOffset CreatedDate { get; }
        public string Description { get; }
        public int Quantity { get; }

        public CreateItemCommand(Guid shoppingCartUid, DateTimeOffset createdDate, string description, int quantity)
        {
            ShoppingCartUid = shoppingCartUid;
            CreatedDate = createdDate;
            Description = description;
            Quantity = quantity;
        }
    }
    
    [UsedImplicitly]
    public class CreateItemCommandHandler : BaseHandler<CreateItemCommand, CreateItemResponse>
    {
        private readonly DatabaseContext _databaseContext;
        
        public CreateItemCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public override async Task<CreateItemResponse> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = 
                await _databaseContext.ShoppingCarts.SingleOrDefaultAsync(sc => sc.Uid == request.ShoppingCartUid, cancellationToken);
            
            if (shoppingCart == null)
                return Error(ErrorCode.NotFound, $"No shopping cart was found with {request.ShoppingCartUid}");

            shoppingCart.UpdatedDate = request.CreatedDate;
            
            var item = new Database.Models.Item
            {
                CreatedDate = request.CreatedDate,
                Description = request.Description,
                Quantity = request.Quantity,
                UpdatedDate = request.CreatedDate,
                ShoppingCartId = shoppingCart.Id
            };
            _databaseContext.Items.Add(item);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            
            return new CreateItemResponse
            {
                ItemUid = item.Uid
            };
        }
    }
    
    public class CreateItemResponse : BaseResponse
    {
        public Guid ItemUid { get; set; }
    }
}