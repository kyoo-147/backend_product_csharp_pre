using System.ComponentModel.DataAnnotations;

namespace ProductInventoryManagement.Entities
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
