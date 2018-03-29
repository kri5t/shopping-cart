using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Core.Commands.Item;
using Shopping.Core.Infrastructure.Mediation;
using Shopping.Database;
using Shopping.UnitTest.Helpers;
using Shopping.UnitTest.Infrastructure;
using Xunit;

namespace Shopping.UnitTest.Commands.Item
{
    public class CreateItemCommandTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _dbContext;
        private readonly CreateItemCommandHandler _sut;
        private readonly Guid _uid = Guid.NewGuid();
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;
        private const string Description = "Something";
        private const int Quantity = 2;

        public CreateItemCommandTest()
        {
            _dbContext = Context();
            _dbContext.AddShoppingCartToContext(_createdDate, _uid);
            _dbContext.SaveChanges();
            _sut = new CreateItemCommandHandler(_dbContext);
        }

        [Fact]
        public async Task Create_item_command_adds_item_to_shopping_cart()
        {
            var result = await _sut.Handle(new CreateItemCommand(_uid, _createdDate, Description, Quantity), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.NotEmpty(_dbContext.Items);
            Assert.NotEmpty(_dbContext.ShoppingCarts.Single().Items);
        } 
        
        [Fact]
        public async Task Create_item_command_adds_correct_properties_to_item()
        {
            var result = await _sut.Handle(new CreateItemCommand(_uid, _createdDate, Description, Quantity), CancellationToken.None);
            
            Assert.False(result.HasError);
            Assert.Contains(_dbContext.Items, x => x.Uid == result.ItemUid);
            var item = _dbContext.Items.Single(x => x.Uid == result.ItemUid);
            Assert.Equal(Description, item.Description);
            Assert.Equal(Quantity, item.Quantity);
            Assert.Equal(_createdDate, item.CreatedDate);
            Assert.Equal(_createdDate, item.UpdatedDate);
        }
        
        [Fact]
        public async Task Create_item_command_with_non_existant_shopping_cart_returns_error()
        {
            var result = await _sut.Handle(new CreateItemCommand(new Guid(), _createdDate, Description, Quantity), CancellationToken.None);
            
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotFound, result.ErrorCode);
        }
    }
}