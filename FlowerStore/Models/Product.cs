using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerStore.Models
{
    // A flower product sold in the store (stored in the Products table)
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the product name.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Description cannot be longer than 300 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10,000 SAR.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Price (SAR)")]
        public decimal Price { get; set; }

        [StringLength(200)]
        [Display(Name = "Image Path")]
        public string? ImageUrl { get; set; }
    }
}
