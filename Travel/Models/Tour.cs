namespace Travel.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int DurationDays { get; set; }
        public string ImageUrl { get; set; }
        public decimal Discount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> CarouselImages { get; set; } = new List<string>();
        public ICollection<Booking> Bookings { get; set; }
    }
}
