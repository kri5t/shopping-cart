using System;
using System.Collections.Generic;
using System.Linq;
using Shopping.Database.Models;

namespace Shopping.Core.Responses
{
    public class ShoppingCartResponse
    {
        public ShoppingCartResponse(ShoppingCart shoppingCart)
        {
            Uid = shoppingCart.Uid;
            CreatedDate = shoppingCart.CreatedDate;
            UpdatedDate = shoppingCart.UpdatedDate;
            ItemList = shoppingCart.Items.Select(i => new ItemResponse
            {
                CreatedDate = i.CreatedDate,
                Description = i.Description,
                Quantity = i.Quantity,
                UpdatedDate = i.UpdatedDate,
                Uid = i.Uid
            }).ToList();
        }

        public List<ItemResponse> ItemList { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid Uid { get; set; }
    }
}