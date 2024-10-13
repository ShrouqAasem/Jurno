using Microsoft.EntityFrameworkCore;
using Travel.Models;
using Travel.ViewModel;

namespace Travel.Services
{
    public class BookingService : IBookingService
    {
        private readonly TravelDbContext _context;

        public BookingService(TravelDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookingViewModel>> GetPastBookings(string userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId && b.BookingDate < DateTime.Now) // Assuming 'Date' is the booking date
                .ToListAsync();

            return bookings.Select(b => new BookingViewModel
            {
                // Map properties from Booking to BookingViewModel
                Id = b.BookingId,
                Date = b.BookingDate
               
            });
        }


        public async Task<IEnumerable<BookingViewModel>> GetUpcomingBookings(string userId)
        {
            // Fetch upcoming bookings from the database
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId && b.BookingDate >= DateTime.Now) // Assuming 'Date' is the booking date
                .ToListAsync();

            return bookings.Select(b => new BookingViewModel
            {
                // Map properties from Booking to BookingViewModel
                Id = b.BookingId,
                Date = b.BookingDate
            });
        }


        public async Task<IEnumerable<BookingViewModel>> GetAllBookings(string userId)
        {
            var pastBookings = await GetPastBookings(userId);
            var upcomingBookings = await GetUpcomingBookings(userId);

            // Combine both lists and return
            return pastBookings.Concat(upcomingBookings);
        }
    }
}
