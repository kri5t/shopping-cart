using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Core.Queries;
using Shopping.Database;
using Shopping.Database.Models;
using Shopping.UnitTest.Helpers;
using Shopping.UnitTest.Infrastructure;
using Xunit;

namespace Shopping.UnitTest.Queries
{
    public class GetShoppingCartsQueryTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _context;
        private readonly GetShoppingCartsQueryHandler _sut;

        public GetShoppingCartsQueryTest()
        {
            _context = Context();
            _sut = new GetShoppingCartsQueryHandler(_context);
        }

        [Fact]
        public async Task Get_shopping_carts_returns_correct_list_of_carts()
        {
            var createdDate = DateTimeOffset.UtcNow;
            var uid = Guid.NewGuid();
            _context.AddShoppingCartToContext(createdDate, uid);
            var result = await _sut.Handle(new GetShoppingCartsQuery(), CancellationToken.None);
            Assert.False(result.HasError);
            var carts = result.ShoppingCarts;
            Assert.NotEmpty(carts);
            carts.Single().VerifyShoppingCart(uid, createdDate, true);
        }
        
        [Fact]
        public async Task Get_shopping_cart_returns_correct_item_model()
        {
            var createdDate = DateTimeOffset.UtcNow;
            var uid = Guid.NewGuid();
            var description = "description";
            var quantity = 2;
            _context.AddShoppingCartToContext(createdDate, uid, true, description, quantity);
            var result = await _sut.Handle(new GetShoppingCartsQuery(), CancellationToken.None);
            Assert.False(result.HasError);
            var carts = result.ShoppingCarts;
            Assert.NotEmpty(carts);
            carts.Single().ItemList.VerifyItemList(uid, createdDate, description, quantity);
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