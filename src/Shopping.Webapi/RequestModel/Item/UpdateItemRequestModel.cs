namespace Shopping.Webapi.RequestModel.Item
{
    public class UpdateItemRequestModel
    {
        /// <summary>
        /// Item description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Item quantity
        /// </summary>
        public int Quantity { get; set; }
    }
}