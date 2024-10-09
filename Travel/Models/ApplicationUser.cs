using Microsoft.AspNetCore.Identity;

namespace Travel.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; }


        // Navigation property
        public ICollection<Booking> Bookings { get; set; }
    }
}
