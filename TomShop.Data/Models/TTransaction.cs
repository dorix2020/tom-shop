using System;
using System.Collections.Generic;

#nullable disable

namespace TomShop.Data.Models
{
    public partial class TTransaction
    {
        public int Id { get; set; }
        public int IdCart { get; set; }
        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
        public int Amount3 { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
