using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;

namespace Shopping.Core.Commands.Item
{
    public class DeleteItemCommand : IRequest<VoidResponse>
    {
        public Guid Uid { get; }

        public DeleteItemCommand(Guid uid)
        {
            Uid = uid;
        }
    }
    
    [UsedImplicitly]
    public class DeleteItemCommandHandler : BaseHandler<DeleteItemCommand, VoidResponse>
    {
        private readonly DatabaseContext _databaseContext;
        
        public DeleteItemCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public override async Task<VoidResponse> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item =  await _databaseContext.Items.SingleOrDefaultAsync(i => i.Uid == request.Uid, cancellationToken);
            if(item == null)
                return Error(ErrorCode.NotFound, $"Not able to find item with {request.Uid}");
            _databaseContext.Remove(item);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new VoidResponse();
        }
    }
}