using Travel.Models;

namespace Travel.ViewModel
{
    public class TourFilterViewModel
    {
        
            public string Location { get; set; }
            public decimal? MinPrice { get; set; }
            public decimal? MaxPrice { get; set; }
            public int? MinDuration { get; set; } // In days or hours
            public int? MaxDuration { get; set; }
            

            public List<Tour> Tours { get; set; } // Filtered list of tours
        

    }
}
