using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Core.Responses;
using Shopping.Database;

namespace Shopping.Core.Queries.Item
{
    public class GetItemQuery : IRequest<GetItemResponse>
    {
        public Guid Uid { get; }

        public GetItemQuery(Guid uid)
        {
            Uid = uid;
        }
    }
    
    [UsedImplicitly]
    public class GetItemQueryHandler : BaseHandler<GetItemQuery, GetItemResponse>
    {
        private readonly DatabaseContext _databaseContext;
        
        public GetItemQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public override async Task<GetItemResponse> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var item = await _databaseContext.Items.SingleOrDefaultAsync(i => i.Uid == request.Uid, cancellationToken);
            if(item == null)
                return Error(ErrorCode.NotFound, $"Not able to find item with {request.Uid}");

            return new GetItemResponse
            {
                ItemResponse = new ItemResponse(item)
            };
        }
    }
    
    public class GetItemResponse : BaseResponse
    {
        public ItemResponse ItemResponse { get; set; }
    }
}