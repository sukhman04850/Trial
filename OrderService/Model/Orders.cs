using System.ComponentModel.DataAnnotations;

namespace OrderService.Model
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        public int ProductId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

    }
}
