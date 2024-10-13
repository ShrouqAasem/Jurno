using Travel.ViewModel;

namespace Travel.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingViewModel>> GetPastBookings(string userId);
        Task<IEnumerable<BookingViewModel>> GetUpcomingBookings(string userId);
        Task<IEnumerable<BookingViewModel>> GetAllBookings(string userId);
    }
}
