namespace Travel.ViewModel
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // e.g., Completed, Upcoming
    }
}
