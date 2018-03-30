using System;

namespace Shopping.Models.Responses
{
    public class ItemResponse
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid Uid { get; set; }
    }
}