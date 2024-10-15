using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Travel.Models;
using Travel.Services;
using Travel.ViewModel;

namespace Travel.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBookingService _bookingService; // Service to handle booking logic
        private readonly TravelDbContext _context;

        public UserController(TravelDbContext context, UserManager<ApplicationUser> userManager, IBookingService bookingService)
        {
            _context = context;
            _userManager = userManager;
            _bookingService = bookingService;
        }

        // GET: /user/account
       
        [HttpGet]
        public async Task<IActionResult> Account()
        {
            ViewBag.PageTitle = "My Profile";
            var user = await _userManager.GetUserAsync(User);

            // Ensure user is not null
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var pastBookings = await _bookingService.GetPastBookings(user.Id) ?? new List<BookingViewModel>();
            var upcomingBookings = await _bookingService.GetUpcomingBookings(user.Id)  ?? new List<BookingViewModel>();

            var model = new UserAccountViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PastBookings = pastBookings.ToList(),
                UpcomingBookings = upcomingBookings.ToList()
            };
           

            return View(model);
        }



        // GET: /user/bookings
        [HttpGet]
        public async Task<IActionResult> Bookings()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = (await _bookingService.GetAllBookings(user.Id))?.ToList()?? new List<BookingViewModel>();
            return View(bookings);
        }




        // POST: /user/update
        [HttpPost]
        public async Task<IActionResult> Update(UserAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Account", model); // Show the account view with errors
            }

            var user = await _userManager.GetUserAsync(User);
            user.FullName = model.FullName;


            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Optionally, redirect or show a success message
                return RedirectToAction("Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Account", model);
        }


        // POST: /user/password-change
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user1 = await _userManager.GetUserAsync(User);
                var accountModel = new UserAccountViewModel
                {
                    FullName = user1.FullName,
                    Email = user1.Email,
                    PastBookings = (await _bookingService.GetPastBookings(user1.Id))?.ToList()?? new List<BookingViewModel>(),
                    UpcomingBookings = (await _bookingService.GetUpcomingBookings(user1.Id))?.ToList() ?? new List<BookingViewModel>()
                };
                return View("Account", accountModel); // Show the account view with errors and loaded profile info
            }

            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {

                // Re-fetch the user and their bookings after the password change
                var user1 = await _userManager.GetUserAsync(User);
                var accountModel = new UserAccountViewModel
                {
                    FullName = user.FullName,
                    Email = user1.Email,
                    PastBookings = (await _bookingService.GetPastBookings(user1.Id))?.ToList() ?? new List<BookingViewModel>(),
                    UpcomingBookings = (await _bookingService.GetUpcomingBookings(user1.Id))?.ToList() ?? new List<BookingViewModel>()
                };

                return RedirectToAction("Account"); // Redirect to the Account action after successful password change
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            var userWithError = await _userManager.GetUserAsync(User);
            var modelWithError = new UserAccountViewModel
            {
                FullName = userWithError.FullName,
                Email = userWithError.Email,
                PastBookings = (await _bookingService.GetPastBookings(userWithError.Id))?.ToList() ?? new List<BookingViewModel>(),
                UpcomingBookings =( await _bookingService.GetUpcomingBookings(userWithError.Id))?.ToList() ?? new List<BookingViewModel>()
            };

            return View("Account", modelWithError); // Show the account view with errors
        }




    }
}
