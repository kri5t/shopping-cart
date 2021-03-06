﻿namespace Shopping.Webapi.RequestModel.Item
{
    public class CreateItemRequestModel
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