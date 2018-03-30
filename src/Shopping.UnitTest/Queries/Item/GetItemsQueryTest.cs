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
    public class GetItemsQueryTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly GetItemsQueryHandler _sut;
        private readonly Guid _uid = Guid.NewGuid();
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;
        private const string Description = "Something";
        private const int Quantity = 2;
        private readonly IMapper _mapper = AutoMapperFactory.Get();

        public GetItemsQueryTest()
        {
            var context = Context();
            context.AddShoppingCartToContext(_createdDate, _uid, true, Description, Quantity);
            _sut = new GetItemsQueryHandler(context, _mapper);
        }

        [Fact]
        public async Task Get_items_list_all_items_in_shopping_cart()
        {
            var result = await _sut.Handle(new GetItemsQuery(_uid), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.NotEmpty(result.Items);
        } 
        
        [Fact]
        public async Task Get_items_list_correct_item()
        {
            var result = await _sut.Handle(new GetItemsQuery(_uid), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.NotEmpty(result.Items);
            result.Items.VerifyItemList(_uid, _createdDate, Description, Quantity);
        } 
        
        [Fact]
        public async Task Get_items_returns_error_on_wrong_cart_uid()
        {
            var result = await _sut.Handle(new GetItemsQuery(Guid.NewGuid()), CancellationToken.None);
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotFound, result.ErrorCode);
        } 
    }
}