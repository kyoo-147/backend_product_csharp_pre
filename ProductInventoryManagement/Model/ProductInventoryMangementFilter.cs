namespace ProductInventoryManagement.Model
{
    public class ProductInventoryMangementFilter
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
