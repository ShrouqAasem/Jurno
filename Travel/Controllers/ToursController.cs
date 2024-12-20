﻿using Microsoft.AspNetCore.Mvc;
using Travel.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Travel.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace Travel.Controllers
{
    public class ToursController : Controller
    {
        private readonly int PageSize = 9;
        private readonly IConfiguration _configuration;
        private readonly TravelDbContext _context;

        public ToursController(TravelDbContext context,IConfiguration configuration)
        {

            _context = context;
            _configuration = configuration;
        }

        //private List<Tour> GetTours()
        //{
        //    return new List<Tour>
        //    {
        //        new Tour { 
        //            Id = 1, 
        //            Name = "Explore Thailand", 
        //            Location = "Thailand", 
        //            Price = 499.00M, 
        //            Description = "Enjoy a guided tour to the heart of Thailand.", 
        //            DurationDays = 5, 
        //            ImageUrl = "https://encrypted-tbn3.gstatic.com/licensed-image?q=tbn:ANd9GcQSs8fyFH2UV4qe7qUZD1anU16aJdSRgA-aDLX_P9Dk-og62T-gpiHsRd-GxoRIB-tEqKVIcrRRDj1DkQanczBZrvAyQzhEnEm_VTYLYbk", 
        //            Discount = 25,
        //            Latitude = 15.8700, 
        //            Longitude = 100.9925,
        //            CarouselImages = new List<string>
        //            {
        //                "https://s3.amazonaws.com/iexplore_web/images/assets/000/005/856/original/thailand.jpg?1442939757",
        //                "https://cdn5.tropicalsky.co.uk/images/800x600/wooden-longtail-boat-maya-bay.jpg",
        //                "https://cdn1.tropicalsky.co.uk/images/800x600/busy-bangkok-street.jpg"
        //            }

        //        },
        //        new Tour { 
        //            Id = 2, 
        //            Name = "Adventure in Malaysia", 
        //            Location = "Malaysia", 
        //            Price = 649.00M, 
        //            Description = "Exciting adventure in Malaysia.", 
        //            DurationDays = 7, 
        //            ImageUrl = "https://www.eyeonasia.gov.sg/images/asean-countries/Malaysia%20snapshot%20cover%20iso.jpg", 
        //            Discount = 30, 
        //            Latitude = 4.2105, 
        //            Longitude = 101.9758,
        //            CarouselImages = new List<string>
        //            {
        //                "https://lp-cms-production.imgix.net/2022-02/shutterstockRF_583559440.jpg?fit=crop&w=1280&auto=format&q=75",
        //                "https://lp-cms-production.imgix.net/2024-08/GettyRF458965367.jpg?fit=crop&w=1280&auto=format&q=75",
        //                "https://lp-cms-production.imgix.net/2024-08/Malaysia-Melaka--cktravels.com-shutterstock1857813349.jpg?fit=crop&w=1280&auto=format&q=75"
        //            }
        //        },
        //        new Tour { 
        //            Id = 3,
        //            Name = "Discover Australia", 
        //            Location = "Australia", 
        //            Price = 799.00M, 
        //            Description = "Unique experience through Australia.", 
        //            DurationDays = 10, 
        //            ImageUrl = "https://www.storylines.com/hs-fs/hubfs/Blogs/Australia/shutterstock_590390942.jpg?width=1200&name=shutterstock_590390942.jpg", 
        //            Discount = 20, 
        //            Latitude = -25.2744, 
        //            Longitude = 133.7751,
        //            CarouselImages = new List<string>
        //            {
        //                "https://www.kkday.com/en/blog/wp-content/uploads/Aussie-VTL-1170x680.jpg",
        //                "https://www.kkday.com/en/blog/wp-content/uploads/au_moreton_island_queensland_eco_tours_kkday.jpg",
        //                "https://www.kkday.com/en/blog/wp-content/uploads/au_glowworm.jpg"
        //            }
        //        },
           
        //    };
        //}


        //public IActionResult Index(int page = 1)
        //{
        //    var tours = GetTours();


        //    var paginatedTours = tours
        //        .Skip((page - 1) * PageSize)
        //        .Take(PageSize)
        //        .ToList();


        //    var viewModel = new TourPaginationViewModel
        //    {
        //        Tours = paginatedTours,
        //        CurrentPage = page,
        //        TotalPages = (int)System.Math.Ceiling(tours.Count() / (double)PageSize)
        //    };

        //    return View(viewModel);
        //}

        //public IActionResult Index(Tour model,
        //    int page = 1,
        //    decimal? minPrice = null,
        //    decimal? maxPrice = null,
        //    string location = null,
        //    int? minDuration = null,
        //    int? maxDuration = null
        //)
        //{
        //    ViewBag.PageTitle = "Tours";
        //    var tours = GetTours();

        //    // Apply filters
        //    if (minPrice.HasValue)
        //    {
        //        tours = tours.Where(t => t.Price >= minPrice.Value).ToList();
        //    }
        //    if (maxPrice.HasValue)
        //    {
        //        tours = tours.Where(t => t.Price <= maxPrice.Value).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(location))
        //    {
        //        tours = tours.Where(t => t.Location.Equals(location, StringComparison.OrdinalIgnoreCase)).ToList();
        //    }
        //    if (minDuration.HasValue)
        //    {
        //        tours = tours.Where(t => t.DurationDays >= minDuration.Value).ToList();
        //    }
        //    if (maxDuration.HasValue)
        //    {
        //        tours = tours.Where(t => t.DurationDays <= maxDuration.Value).ToList();
        //    }

        //    // Pagination
        //    var paginatedTours = tours
        //        .Skip((page - 1) * PageSize)
        //        .Take(PageSize)
        //        .ToList();

        //    var viewModel = new TourPaginationViewModel
        //    {
        //        Tours = paginatedTours,
        //        CurrentPage = page,
        //        TotalPages = (int)System.Math.Ceiling(tours.Count() / (double)PageSize)
        //    };

        //    return View(viewModel);
        //}

        public IActionResult Index(
         int page = 1,
         decimal? minPrice = null,
         decimal? maxPrice = null,
           string location = null,
          int? minDuration = null,
          int? maxDuration = null
         )
        {
            ViewBag.PageTitle = "Tours";

            var toursQuery = _context.Tours.AsQueryable();

            // Apply filters
            if (minPrice.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.Price <= maxPrice.Value);
            }
            if (!string.IsNullOrEmpty(location))
            {
                toursQuery = toursQuery.Where(t => t.Location.Equals(location, StringComparison.OrdinalIgnoreCase));
            }
            if (minDuration.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.DurationDays >= minDuration.Value);
            }
            if (maxDuration.HasValue)
            {
                toursQuery = toursQuery.Where(t => t.DurationDays <= maxDuration.Value);
            }

            // Pagination
            var totalTours = toursQuery.Count();
            var paginatedTours = toursQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new TourPaginationViewModel
            {
                Tours = paginatedTours,
                CurrentPage = page,
                TotalPages = (int)System.Math.Ceiling(totalTours / (double)PageSize)
            };

            return View(viewModel);
        }


        public IActionResult Details(int id)
        {
           
            var tour = _context.Tours.FirstOrDefault(t => t.Id == id);

            if (tour == null)
            {
                return NotFound();
            }

            ViewBag.MapboxToken = _configuration["Mapbox:AccessToken"];

            return View(tour);
        }



       

    }
}
