using System;

namespace shopping.database.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        
        public ShoppingCart ShoppingCart { get; set; }
        public int ShoppingCartId { get; set; }
    }
}