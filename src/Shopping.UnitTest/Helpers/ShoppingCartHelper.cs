using System;
using Shopping.Database;
using Shopping.Database.Models;

namespace Shopping.UnitTest.Helpers
{
    public static class ShoppingCartHelper
    {
        public static void AddShoppingCartToContext(
            this DatabaseContext context,
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
            
            context.ShoppingCarts.Add(shoppingCart);
            context.SaveChanges();
        }
    }
}