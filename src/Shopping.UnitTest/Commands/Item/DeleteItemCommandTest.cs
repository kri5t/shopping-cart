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
    public class DeleteItemCommandTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _dbContext;
        private readonly DeleteItemCommandHandler _sut;
        private readonly Guid _uid = Guid.NewGuid();
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;
        private const string Description = "Something";
        private const int Quantity = 2;

        public DeleteItemCommandTest()
        {
            _dbContext = Context();
            _dbContext.AddShoppingCartToContext(_createdDate, _uid, true, Description, Quantity);
            _dbContext.SaveChanges();
            _sut = new DeleteItemCommandHandler(_dbContext);
        }

        [Fact]
        public async Task Delete_item_command_removes_item_from_db()
        {
            var result = await _sut.Handle(new DeleteItemCommand(_uid), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.Empty(_dbContext.Items);
        } 
        
        [Fact]
        public async Task Delete_item_command_with_non_existant_item_returns_error()
        {
            var result = await _sut.Handle(new DeleteItemCommand(Guid.NewGuid()), CancellationToken.None);
            
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotFound, result.ErrorCode);
        }
    }
}