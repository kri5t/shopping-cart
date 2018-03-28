using System;
using System.Collections.Generic;

namespace Shopping.Database.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Items = new List<Item>();
        }
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public List<Item> Items { get; set; }
    }
}