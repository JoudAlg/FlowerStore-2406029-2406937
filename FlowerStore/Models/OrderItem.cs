using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerStore.Models
{
    // One product inside an order (stored in the OrderItems table)
    public class OrderItem
    {
        public int Id { get; set; }

        // Link to the order this line belongs to
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // We save a copy of the name and price, so old orders stay correct
        // even if the product is edited or deleted later
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        public decimal LineTotal => Price * Quantity;
    }
}
