using System;
using System.Collections.Generic;

namespace shopping.database.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public List<Item> Items { get; set; }
    }
}