using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;

namespace Shopping.Core.Commands.ShoppingCart
{
    public class EmptyShoppingCartCommand : IRequest<VoidResponse>
    {
        public Guid Uid { get; }
        public DateTimeOffset UpdatedDate { get; }

        public EmptyShoppingCartCommand(Guid uid, DateTimeOffset updatedDate)
        {
            Uid = uid;
            UpdatedDate = updatedDate;
        }
    }
    
    [UsedImplicitly]
    public class EmptyShoppingCartCommandHandler : BaseHandler<EmptyShoppingCartCommand, VoidResponse>
    {
        private readonly DatabaseContext _databaseContext;
        
        public EmptyShoppingCartCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public override async Task<VoidResponse> Handle(EmptyShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _databaseContext.ShoppingCarts
                .Include(sc => sc.Items)
                .SingleOrDefaultAsync(x => x.Uid == request.Uid, cancellationToken);
            
            if (shoppingCart == null)
                return Error(ErrorCode.NotFound, "No shopping cart found to delete");

            shoppingCart.UpdatedDate = request.UpdatedDate;
            _databaseContext.Items.RemoveRange(shoppingCart.Items);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new VoidResponse();
        }
    }
}