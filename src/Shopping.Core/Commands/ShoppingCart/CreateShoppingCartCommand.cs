using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;
using Shopping.Models.Responses;

namespace Shopping.Core.Commands.ShoppingCart
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
            var shoppingCart = new Database.Models.ShoppingCart
            {
                CreatedDate = request.CreatedDate,
                UpdatedDate = request.CreatedDate,
                Uid = Guid.NewGuid()
            };
            await _database.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            await _database.SaveChangesAsync(cancellationToken);
            return new CreateShoppingCartResponse
            {
                Response = new UidResponse { Uid = shoppingCart.Uid }
            };
        }
    }

    public class CreateShoppingCartResponse : BaseResponse
    {
        public UidResponse Response { get; set; }
    }
}