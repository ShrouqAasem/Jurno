using System;
using System.ComponentModel.DataAnnotations;

namespace Travel.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsConfirmed { get; set; }

        // Foreign keys
        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public string UserId { get; set; } = string.Empty; // The customer who made the booking
        public ApplicationUser User { get; set; }

        // Optional: Add payment-related properties
        public string PaymentId { get; set; } = string.Empty; // To store the PayPal payment ID
        public string PaymentStatus { get; set; } = string.Empty; // To track the payment status
    }
}
