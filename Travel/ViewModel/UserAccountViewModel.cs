using Travel.Models;

namespace Travel.ViewModel
{
    public class UserAccountViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<BookingViewModel> PastBookings { get; set; } = new List<BookingViewModel>();
        public List<BookingViewModel> UpcomingBookings { get; set; } = new List<BookingViewModel>();


    }
}
