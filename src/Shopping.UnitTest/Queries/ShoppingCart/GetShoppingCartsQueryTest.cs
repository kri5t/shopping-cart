using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Core.Queries;
using Shopping.Database;
using Shopping.UnitTest.Helpers;
using Shopping.UnitTest.Infrastructure;
using Xunit;

namespace Shopping.UnitTest.Queries.ShoppingCart
{
    public class GetShoppingCartsQueryTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _context;
        private readonly GetShoppingCartsQueryHandler _sut;
        private readonly Guid _uid = Guid.NewGuid();
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public GetShoppingCartsQueryTest()
        {
            _context = Context();
            _sut = new GetShoppingCartsQueryHandler(_context);
        }

        [Fact]
        public async Task Get_shopping_carts_returns_correct_list_of_carts()
        {
            _context.AddShoppingCartToContext(_createdDate, _uid);
            var result = await _sut.Handle(new GetShoppingCartsQuery(), CancellationToken.None);
            Assert.False(result.HasError);
            var carts = result.ShoppingCarts;
            Assert.NotEmpty(carts);
            carts.Single().VerifyShoppingCart(_uid, _createdDate, true);
        }
        
        [Fact]
        public async Task Get_shopping_cart_returns_correct_item_model()
        {
            var description = "description";
            var quantity = 2;
            _context.AddShoppingCartToContext(_createdDate, _uid, true, description, quantity);
            var result = await _sut.Handle(new GetShoppingCartsQuery(), CancellationToken.None);
            Assert.False(result.HasError);
            var carts = result.ShoppingCarts;
            Assert.NotEmpty(carts);
            carts.Single().ItemList.VerifyItemList(_uid, _createdDate, description, quantity);
        }

        [Fact]
        public async Task Get_shopping_cart_with_no_shopping_carts_returns_empty_list()
        {
            var result = await _sut.Handle(new GetShoppingCartsQuery(), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.Empty(result.ShoppingCarts);
        }
    }
}