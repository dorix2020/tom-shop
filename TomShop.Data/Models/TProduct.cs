using System;
using System.Collections.Generic;

#nullable disable

namespace TomShop.Data.Models
{
    public partial class TProduct
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string NameFull { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public long PriceBuy { get; set; }
        public long PriceSell { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
        public string VendorProductCode { get; set; }
        public string VendorProductName { get; set; }
        public int? VendorOrderId { get; set; }
        public int? VendorId { get; set; }
        public string ExtraDetails { get; set; }
        public string Location { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
