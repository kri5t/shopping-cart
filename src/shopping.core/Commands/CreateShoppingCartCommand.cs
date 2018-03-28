using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;
using Shopping.Database.Models;

namespace Shopping.Core.Commands
{
    public class CreateShoppingCartCommand : IRequest<CreateShoppingCartResponse>
    {
        public DateTimeOffset CreatedDate { get; }

        public CreateShoppingCartCommand(DateTimeOffset createdDate)
        {
            CreatedDate = createdDate;
        }
    }
    
    [UsedImplicitly]
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, CreateShoppingCartResponse> {
        private readonly DatabaseContext _database;

        public CreateShoppingCartCommandHandler(DatabaseContext database)
        {
            _database = database;
        }
        
        public async Task<CreateShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = new ShoppingCart
            {
                CreatedDate = request.CreatedDate,
                UpdatedDate = request.CreatedDate,
                Uid = Guid.NewGuid()
            };
            await _database.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            return new CreateShoppingCartResponse(shoppingCart.Uid);
        }
    }

    public class CreateShoppingCartResponse : BaseResponse
    {
        public Guid ShoppingCartUid { get; }

        public CreateShoppingCartResponse(Guid shoppingCartUid)
        {
            ShoppingCartUid = shoppingCartUid;
        }
    }
}