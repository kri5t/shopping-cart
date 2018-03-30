using System;
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
        private readonly IMapper _mapper;

        public GetItemQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }
        
        public override async Task<GetItemResponse> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var item = await _databaseContext.Items.SingleOrDefaultAsync(i => i.Uid == request.Uid, cancellationToken);
            if(item == null)
                return Error(ErrorCode.NotFound, $"Not able to find item with {request.Uid}");

            return new GetItemResponse
            {
                ItemResponse = _mapper.Map<Database.Models.Item, ItemResponse>(item)
            };
        }
    }
    
    public class GetItemResponse : BaseResponse
    {
        public ItemResponse ItemResponse { get; set; }
    }
}