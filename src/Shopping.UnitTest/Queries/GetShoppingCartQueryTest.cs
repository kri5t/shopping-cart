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
    public class GetShoppingCartQueryTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _context;
        private readonly GetShoppingCartQueryHandler _sut;

        public GetShoppingCartQueryTest()
        {
            _context = Context();
            _sut = new GetShoppingCartQueryHandler(_context);
        }

        [Fact]
        public async Task Get_shopping_cart_returns_correct_model()
        {
            var createdDate = DateTimeOffset.UtcNow;
            var uid = Guid.NewGuid();
            _context.AddShoppingCartToContext(createdDate, uid);
            var result = await _sut.Handle(new GetShoppingCartQuery(uid), CancellationToken.None);
            Assert.False(result.HasError);
            result.ShoppingCartResponse.VerifyShoppingCart(uid, createdDate, true);
        }
        
        [Fact]
        public async Task Get_shopping_cart_returns_correct_item_model()
        {
            var createdDate = DateTimeOffset.UtcNow;
            var uid = Guid.NewGuid();
            var description = "description";
            var quantity = 2;
            _context.AddShoppingCartToContext(createdDate, uid, true, description, quantity);
            var result = await _sut.Handle(new GetShoppingCartQuery(uid), CancellationToken.None);
            Assert.False(result.HasError);
            result.ShoppingCartResponse.ItemList.VerifyItemList(uid, createdDate, description, quantity);
        }

        [Fact]
        public async Task Get_shopping_cart_with_empty_uid_returns_error()
        {
            var uid = Guid.Empty;
            var result = await _sut.Handle(new GetShoppingCartQuery(uid), CancellationToken.None);
            Assert.True(result.HasError);
        }

        [Fact]
        public async Task Get_shopping_cart_with_nonexistant_uid_returns_error()
        {
            _context.AddShoppingCartToContext(DateTimeOffset.UtcNow, Guid.NewGuid());
            var result = await _sut.Handle(new GetShoppingCartQuery(Guid.NewGuid()), CancellationToken.None);
            Assert.True(result.HasError);
        }
    }
}