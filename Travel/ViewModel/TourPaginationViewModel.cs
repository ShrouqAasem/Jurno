using System.Collections.Generic;

namespace Travel.Models
{
    public class TourPaginationViewModel
    {
        public IEnumerable<Tour> Tours { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
