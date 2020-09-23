using System;
using System.Collections.Generic;

#nullable disable

namespace TomShop.Data.Models
{
    public partial class TCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool HasChildren { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string ExtraDetails { get; set; }
    }
}
