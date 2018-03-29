using System;
using System.Linq;
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
        
        [Fact]
        public async Task Get_shopping_cart_returns_correct_item_model()
        {
            var createdDate = DateTimeOffset.UtcNow;
            var uid = Guid.NewGuid();
            var description = "description";
            var quantity = 2;
            AddShoppingCartToContext(createdDate, uid, true, description, quantity);
            var result = await _sut.Handle(new GetShoppingCartQuery(uid), CancellationToken.None);
            Assert.False(result.HasError);
            var item = result.ShoppingCartResponse.ItemList.Single();
            Assert.Equal(uid, item.Uid);
            Assert.Equal(createdDate, item.CreatedDate);
            Assert.Equal(createdDate, item.UpdatedDate);
            Assert.Equal(description, item.Description);
            Assert.Equal(quantity, item.Quantity);
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
            AddShoppingCartToContext(DateTimeOffset.UtcNow, Guid.NewGuid());
            var result = await _sut.Handle(new GetShoppingCartQuery(Guid.NewGuid()), CancellationToken.None);
            Assert.True(result.HasError);
        }

        private void AddShoppingCartToContext(
            DateTimeOffset createdDate, 
            Guid uid, 
            bool addItem = false, 
            string description = "", 
            int quantity = 0
            )
        {
            var shoppingCart = new ShoppingCart
            {
                CreatedDate = createdDate,
                Uid = uid,
                UpdatedDate = createdDate,
            };
            if(addItem){
                shoppingCart.Items.Add(new Item
                {
                    CreatedDate = createdDate,
                    UpdatedDate = createdDate,
                    Description = description,
                    Quantity = quantity,
                    Uid = uid
                });
            }
            
            _context.ShoppingCarts.Add(shoppingCart);
            _context.SaveChanges();
        }
    }
}