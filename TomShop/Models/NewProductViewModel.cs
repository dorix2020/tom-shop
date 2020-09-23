namespace TomShop.Models
{
    public class NewProductViewModel
    {
        public int CategoryId { get; set; }
        public string NameFull { get; set; }
        public string Description { get; set; }
        public long PriceBuy { get; set; }
        public long PriceSell { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }

        public string ImageContent { get; set; }
    }
}
