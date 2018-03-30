using System;
using System.Collections.Generic;

namespace Shopping.Models.Responses
{
    public class ShoppingCartResponse
    {
        public List<ItemResponse> ItemList { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid Uid { get; set; }
    }
}