using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;

namespace Shopping.Core.Commands
{
    public class DeleteShoppingCartCommand : IRequest<VoidResponse>
    {
        public Guid Uid { get; }

        public DeleteShoppingCartCommand(Guid uid)
        {
            Uid = uid;
        }
    }

    [UsedImplicitly]
    public class DeleteShoppingCartCommandHandler : BaseHandler<DeleteShoppingCartCommand, VoidResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public DeleteShoppingCartCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public override async Task<VoidResponse> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _databaseContext.ShoppingCarts.SingleOrDefaultAsync(sc => sc.Uid == request.Uid, cancellationToken);
            if (shoppingCart == null)
                return Error(ErrorCode.NotFound, "No shopping cart found to delete");
            _databaseContext.ShoppingCarts.Remove(shoppingCart);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new VoidResponse();
        }
    }
}