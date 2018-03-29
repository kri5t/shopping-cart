using System;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Core.Queries;
using Shopping.Database;
using Shopping.Database.Models;
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
            AddShoppingCartToContext(createdDate, uid);
            var result = await _sut.Handle(new GetShoppingCartQuery(uid), CancellationToken.None);
            Assert.False(result.HasError);
            var response = result.ShoppingCartResponse;
            Assert.Equal(uid, response.Uid);
            Assert.Equal(createdDate, response.CreatedDate);
            Assert.Equal(createdDate, response.UpdatedDate);
            Assert.Empty(response.ItemList);
        }

        private void AddShoppingCartToContext(DateTimeOffset createdDate, Guid uid)
        {
            _context.ShoppingCarts.Add(new ShoppingCart
            {
                CreatedDate = createdDate,
                Uid = uid,
                UpdatedDate = createdDate
            });
            _context.SaveChanges();
        }
    }
}