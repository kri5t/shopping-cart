using System;
using System.Collections.Generic;
using System.Linq;
using Shopping.Core.Responses;
using Xunit;

namespace Shopping.UnitTest.Helpers
{
    public static class ShoppingCartResponseVerifier
    {
        public static void VerifyShoppingCart(
            this ShoppingCartResponse shoppingCart,
            Guid uid,
            DateTimeOffset createdDate,
            bool empty = true
            )
        {
            Assert.Equal(uid, shoppingCart.Uid);
            Assert.Equal(createdDate, shoppingCart.CreatedDate);
            Assert.Equal(createdDate, shoppingCart.UpdatedDate);
            if(empty)
                Assert.Empty(shoppingCart.ItemList);
        }
        
        public static void VerifyItemList(
            this List<ItemResponse> items, Guid uid, DateTimeOffset createdDate, string description, int quantity)
        {
            var item = items.Single();
            Assert.Equal(uid, item.Uid);
            Assert.Equal(createdDate, item.CreatedDate);
            Assert.Equal(createdDate, item.UpdatedDate);
            Assert.Equal(description, item.Description);
            Assert.Equal(quantity, item.Quantity);
        }
    }
}