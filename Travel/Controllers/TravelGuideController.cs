using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Travel.Models;

namespace Travel.Controllers
{
    public class TravelGuideController : Controller
    {
        // Sample data
        private static List<TravelGuide> TravelGuides = new List<TravelGuide>
        {
            new TravelGuide
            {
                DestinationId = 1,
                Name = "Thailand",
                Description = "Tropical beaches, opulent royal palaces, ancient ruins, and ornate temples.",
                ImageUrl = "/img/Thailand_Featured.webp",
                Activities = new List<string> { "Island hopping", "Scuba diving", "Visiting temples" },
                Hotels = new List<string> { "Siam Kempinski Hotel", "Rayavadee Beach Resort" },
                Restaurants = new List<string> { "Gaggan", "Nahm" }
            },
            new TravelGuide
            {
                DestinationId = 2,
                Name = "Australia",
                Description = "Iconic landmarks and natural wonders, such as the Great Barrier Reef and Sydney Opera House.",
                ImageUrl = "/img/Australia_Featured.webp",
                Activities = new List<string> { "Surfing", "Exploring the Outback", "Snorkeling at the Great Barrier Reef" },
                Hotels = new List<string> { "Park Hyatt Sydney", "Qualia Resort" },
                Restaurants = new List<string> { "Quay", "Attica" }
            },
            // Add more destinations as needed...
        };

        // Action to list destinations
        public IActionResult Index()
        {
            // Pass the list of travel guides to the view
            return View(TravelGuides);
        }

        // Action to display details of a specific destination
        public IActionResult Details(int id)
        {
            var destination = TravelGuides.Find(d => d.DestinationId == id);
            if (destination == null)
            {
                return NotFound();
            }
            return View(destination);
        }
    }
}
