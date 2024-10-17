using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using System;
using Travel.Models;

namespace Travel.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PayPalPayment _payPalPayment;
        private readonly TravelDbContext _context;

        public PaymentController(TravelDbContext context, IConfiguration configuration)
        {
            _context = context;
            _payPalPayment = new PayPalPayment(configuration);
        }

        // Initiate the payment process
        public IActionResult Pay(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId); // Fetch the booking from the database
            if (booking == null)
            {
                return NotFound(); // Handle the case where the booking doesn't exist
            }

            var amount = booking.Tour.Price; // Assume the Tour model has a Price property

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Url.Content("~/")}";
            var returnUrl = $"{baseUrl}Payment/Success";
            var cancelUrl = $"{baseUrl}Payment/Cancel";

            var payment = _payPalPayment.CreatePayment(amount, returnUrl, cancelUrl, booking); // Pass the booking

            var approvalUrl = payment.GetApprovalUrl();
            return Redirect(approvalUrl);
        }

        // Success callback
        public IActionResult Success()
        {
            // Handle successful payment here
            return View();
        }

        // Cancel callback
        public IActionResult Cancel()
        {
            // Handle canceled payment here
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
