using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FlowerStore.Models
{
    // A confirmed order (stored in the Orders table)
    public class Order
    {
        public int Id { get; set; }

        // Email of the logged-in user who placed the order.
        // Filled by the server from the Session (not by the form).
        [ValidateNever]
        [StringLength(150)]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the receiver's name.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [Display(Name = "Receiver Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a phone number.")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Phone must be 10 digits and start with 05 (e.g. 0512345678).")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the delivery address.")]
        [StringLength(300, ErrorMessage = "Address cannot be longer than 300 characters.")]
        [Display(Name = "Delivery Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choose a delivery date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        [StringLength(200, ErrorMessage = "Gift message cannot be longer than 200 characters.")]
        [Display(Name = "Gift Card Message (optional)")]
        public string? Notes { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        // The flowers inside this order (stored in the OrderItems table)
        [ValidateNever]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
