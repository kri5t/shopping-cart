using System;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Core.Commands.ShoppingCart;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;
using Shopping.UnitTest.Helpers;
using Shopping.UnitTest.Infrastructure;
using Xunit;

namespace Shopping.UnitTest.Commands.ShoppingCart
{
    public class EmptyShoppingCartCommandTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _dbContext;
        private readonly EmptyShoppingCartCommandHandler _sut;
        private readonly Guid _uid = Guid.NewGuid();
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public EmptyShoppingCartCommandTest()
        {
            _dbContext = Context();            
            _sut = new EmptyShoppingCartCommandHandler(_dbContext);
        }
        
        [Fact]
        public async Task Delete_shopping_cart_without_items_removes_shopping_cart_from_db()
        {
            _dbContext.AddShoppingCartToContext(DateTimeOffset.UtcNow, _uid, true, "OK", 12);
            var result = await _sut.Handle(new EmptyShoppingCartCommand(_uid, _createdDate), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.Empty(_dbContext.Items);
        }
        
        [Fact]
        public async Task Delete_shopping_cart_with_wrong_uid_returns_error()
        {
            _dbContext.AddShoppingCartToContext(DateTimeOffset.UtcNow, _uid);
            var result = await _sut.Handle(new EmptyShoppingCartCommand(Guid.NewGuid(), _createdDate), CancellationToken.None);
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotFound, result.ErrorCode);
        }
    }
}