using System.ComponentModel.DataAnnotations;

namespace Travel.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string? Role { get; set; } // "Customer" or "Admin"

        // Navigation property for bookings
        public ICollection<Booking> Bookings { get; set; }
    }
}
