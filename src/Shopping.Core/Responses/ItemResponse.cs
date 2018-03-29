using System;
using Shopping.Database.Models;

namespace Shopping.Core.Responses
{
    public class ItemResponse
    {
        public ItemResponse() {}

        public ItemResponse(Item item)
        {
            Description = item.Description;
            CreatedDate = item.CreatedDate;
            UpdatedDate = item.UpdatedDate;
            Quantity = item.Quantity;
            Uid = item.Uid;
        }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid Uid { get; set; }
    }
}