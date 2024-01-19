using System.ComponentModel.DataAnnotations;

namespace ProductService.Model
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        
        public float Price { get; set; }
        public int Quantity { get; set; }

    }
}
