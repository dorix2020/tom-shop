using System;
using System.Collections.Generic;

#nullable disable

namespace TomShop.Data.Models
{
    public partial class TCart
    {
        public int Id { get; set; }
        public long Amount1 { get; set; }
        public long Amount2 { get; set; }
        public long Amount3 { get; set; }
        public int Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
