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
    public class UpdateItemCommandTest : EfContextTest<DatabaseContextFactory, DatabaseContext>
    {
        private readonly DatabaseContext _dbContext;
        private readonly UpdateItemCommandHandler _sut;
        private readonly Guid _uid = Guid.NewGuid();
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;
        private const string Description = "Something";
        private const int Quantity = 2;

        public UpdateItemCommandTest()
        {
            _dbContext = Context();
            _dbContext.AddShoppingCartToContext(_createdDate, _uid, true, Description, Quantity);
            _dbContext.SaveChanges();
            _sut = new UpdateItemCommandHandler(_dbContext);
        }

        [Fact]
        public async Task Update_item_command_updates_description_quantity_and_updatedate()
        {
            var newDescription = "New description";
            var newQuantity = 22;
            var updatedDate = DateTimeOffset.UtcNow;
            var result = await _sut.Handle(new UpdateItemCommand(updatedDate, _uid, newDescription, newQuantity), CancellationToken.None);
            Assert.False(result.HasError);
            Assert.NotEmpty(_dbContext.Items);
            var item = _dbContext.Items.Single(x => x.Uid == _uid);
            Assert.Equal(newDescription, item.Description);
            Assert.Equal(newQuantity, item.Quantity);
            Assert.Equal(_createdDate, item.CreatedDate);
            Assert.Equal(updatedDate, item.UpdatedDate);
        } 
        
        [Fact]
        public async Task Update_item_command_with_non_existant_item_returns_error()
        {
            var result = await _sut.Handle(new UpdateItemCommand(_createdDate, Guid.NewGuid(), Description, Quantity), CancellationToken.None);
            
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotFound, result.ErrorCode);
        }
        
        [Fact]
        public async Task Update_item_command_with_negative_quantity_fails()
        {
            var result = await _sut.Handle(new UpdateItemCommand(_createdDate, _uid, Description, -10), CancellationToken.None);
            
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotValid, result.ErrorCode);
        }
        
        [Fact]
        public async Task Update_item_command_with_empty_description_fails()
        {
            var result = await _sut.Handle(new UpdateItemCommand(_createdDate, _uid, "", 10), CancellationToken.None);
            
            Assert.True(result.HasError);
            Assert.Equal(ErrorCode.NotValid, result.ErrorCode);
        }
    }
}