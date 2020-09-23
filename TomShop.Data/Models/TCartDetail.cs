using System;
using System.Collections.Generic;

#nullable disable

namespace TomShop.Data.Models
{
    public partial class TCartDetail
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public DateTimeOffset CartCreatedTime { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public long ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
