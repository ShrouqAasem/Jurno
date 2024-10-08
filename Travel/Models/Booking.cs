namespace Travel.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsConfirmed { get; set; }

        // Foreign keys
        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public string UserId { get; set; }  // The customer who made the booking
        public ApplicationUser User { get; set; }
    }
}
