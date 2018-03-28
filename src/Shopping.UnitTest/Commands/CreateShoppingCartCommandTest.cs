using System;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Core.Commands;
using Shopping.Database;
using Shopping.UnitTest.Infrastructure;
using Xunit;

namespace Shopping.UnitTest.Commands
{
    public class CreateShoppingCartCommandTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _dbContext;
        private readonly CreateShoppingCartCommandHandler _sut;

        public CreateShoppingCartCommandTest()
        {
            _dbContext = Context();            
            _sut = new CreateShoppingCartCommandHandler(_dbContext);
        }
        
        [Fact]
        public async Task Creating_a_shopping_cart_adds_one_to_the_db()
        {
            var result = await _sut.Handle(new CreateShoppingCartCommand(DateTimeOffset.UtcNow), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.NotEqual(Guid.Empty, result.ShoppingCartUid);
            Assert.Single(_dbContext.ShoppingCarts, x => x.Uid == result.ShoppingCartUid);
        }
    }
}