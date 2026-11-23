
using System.ComponentModel.DataAnnotations;

namespace FlowerStore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // User role: Customer or Admin
        [Required]
        public string Role { get; set; } = "Customer";
    }
}
