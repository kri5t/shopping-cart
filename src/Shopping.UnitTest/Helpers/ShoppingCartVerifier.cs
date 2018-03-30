using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Shopping.Models.Responses;
using Xunit;

namespace Shopping.UnitTest.Helpers
{
    public static class ShoppingCartResponseVerifier
    {
        [AssertionMethod]
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
        
        [AssertionMethod]
        public static void VerifyItemList(
            this List<ItemResponse> items, Guid uid, DateTimeOffset createdDate, string description, int quantity)
        {
            items.Single().VerifyItem(uid, createdDate, description, quantity);
        }
        
        [AssertionMethod]
        public static void VerifyItem(
            this ItemResponse item, Guid uid, DateTimeOffset createdDate, string description, int quantity)
        {
            Assert.NotEqual(Guid.Empty, item.Uid);
            Assert.Equal(uid, item.Uid);
            Assert.Equal(createdDate, item.CreatedDate);
            Assert.Equal(createdDate, item.UpdatedDate);
            Assert.Equal(description, item.Description);
            Assert.Equal(quantity, item.Quantity);
        }
    }
}