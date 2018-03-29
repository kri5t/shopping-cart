using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;

namespace Shopping.Core.Commands.Item
{
    public class UpdateItemCommand : IRequest<VoidResponse>
    {
        public DateTimeOffset UpdatedDate { get; }
        public Guid Uid { get; }
        public string Description { get; }
        public int Quantity { get; }

        public UpdateItemCommand(DateTimeOffset updatedDate, Guid uid, string description, int quantity)
        {
            UpdatedDate = updatedDate;
            Uid = uid;
            Description = description;
            Quantity = quantity;
        }
    }
    
    [UsedImplicitly]
    public class UpdateItemCommandHandler : BaseHandler<UpdateItemCommand, VoidResponse>
    {
        private readonly DatabaseContext _databaseContext;
        
        public UpdateItemCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public override async Task<VoidResponse> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _databaseContext.Items.SingleOrDefaultAsync(i => i.Uid == request.Uid, cancellationToken);
            if(item == null)
                return Error(ErrorCode.NotFound, $"Not able to find item with {request.Uid}");
            
            if(request.Quantity <= 0)
                return Error(ErrorCode.NotValid, "Quantity cannot be less than 0");
            
            if(string.IsNullOrWhiteSpace(request.Description))
                return Error(ErrorCode.NotValid, "Description cannot be empty");
            
            item.UpdatedDate = request.UpdatedDate;
            item.Quantity = request.Quantity;
            item.Description = request.Description;
            await _databaseContext.SaveChangesAsync(cancellationToken);
            
            return new VoidResponse();
        }
    }
}