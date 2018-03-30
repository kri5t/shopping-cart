using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Core.Queries.Item;
using Shopping.Database;
using Shopping.UnitTest.Helpers;
using Shopping.UnitTest.Infrastructure;
using Xunit;

namespace Shopping.UnitTest.Queries.Item
{
    public class GetItemQueryTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly GetItemQueryHandler _sut;
        private readonly Guid _uid = Guid.NewGuid();
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;
        private const string Description = "Something";
        private const int Quantity = 2;
        private readonly IMapper _mapper = AutoMapperFactory.Get();

        public GetItemQueryTest()
        {
            var context = Context();
            context.AddShoppingCartToContext(_createdDate, _uid, true, Description, Quantity);
            _sut = new GetItemQueryHandler(context, _mapper);
        }

        [Fact]
        public async Task Get_items_list_correct_item()
        {
            var result = await _sut.Handle(new GetItemQuery(_uid), CancellationToken.None);
            Assert.False(result.HasError);
            result.ItemResponse.VerifyItem(_uid, _createdDate, Description, Quantity);
        } 
        
        [Fact]
        public async Task Get_items_returns_error_on_wrong_cart_uid()
        {
            var result = await _sut.Handle(new GetItemQuery(Guid.NewGuid()), CancellationToken.None);
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotFound, result.ErrorCode);
        } 
    }
}