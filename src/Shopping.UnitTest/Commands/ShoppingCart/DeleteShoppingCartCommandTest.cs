using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Core.Commands;
using Shopping.Core.Commands.ShoppingCart;
using Shopping.Database;
using Shopping.UnitTest.Helpers;
using Shopping.UnitTest.Infrastructure;
using Xunit;

namespace Shopping.UnitTest.Commands
{
    public class DeleteShoppingCartCommandTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _dbContext;
        private readonly DeleteShoppingCartCommandHandler _sut;
        private readonly Guid _uid = Guid.NewGuid(); 

        public DeleteShoppingCartCommandTest()
        {
            _dbContext = Context();            
            _sut = new DeleteShoppingCartCommandHandler(_dbContext);
        }
        
        [Fact]
        public async Task Delete_shopping_cart_without_items_removes_shopping_cart_from_db()
        {
            _dbContext.AddShoppingCartToContext(DateTimeOffset.UtcNow, _uid);
            var result = await _sut.Handle(new DeleteShoppingCartCommand(_uid), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.DoesNotContain(_dbContext.ShoppingCarts, x => x.Uid != _uid);
        }
        
        [Fact]
        public async Task Delete_shopping_cart_with_items_removes_shopping_cart_and_items_from_db()
        {
            _dbContext.AddShoppingCartToContext(DateTimeOffset.UtcNow, _uid, true, "something", 10);
            Assert.True(_dbContext.Items.Any());
            var result = await _sut.Handle(new DeleteShoppingCartCommand(_uid), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.DoesNotContain(_dbContext.ShoppingCarts, x => x.Uid != _uid);
            Assert.Empty(_dbContext.ShoppingCarts);
            Assert.DoesNotContain(_dbContext.Items, x => x.Uid != _uid);
            Assert.Empty(_dbContext.Items);
        }
        [Fact]
        public async Task Delete_shopping_cart_with_wrong_uid_returns_error()
        {
            _dbContext.AddShoppingCartToContext(DateTimeOffset.UtcNow, _uid);
            var result = await _sut.Handle(new DeleteShoppingCartCommand(Guid.NewGuid()), CancellationToken.None);
            Assert.True(result.HasError);
        }
    }
}