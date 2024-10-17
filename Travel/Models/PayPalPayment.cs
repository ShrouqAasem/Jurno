using System;
using System.Collections.Generic;
using PayPal.Api;

namespace Travel.Models
{
    public class PayPalPayment
    {
        private readonly APIContext apiContext;
        private readonly TravelDbContext _context;

        public PayPalPayment(IConfiguration configuration)
        {
            var clientId = configuration["PayPal:ClientId"];
            var clientSecret = configuration["PayPal:ClientSecret"];
            apiContext = new APIContext(new OAuthTokenCredential(clientId, clientSecret).GetAccessToken());
        } 

        public Payment CreatePayment(decimal amount, string returnUrl, string cancelUrl, Booking booking)
        {
            // Validate the amount
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
            }

            // Create the payment object
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = "Booking payment for Tour: " + booking.Tour.Name, // Customize based on Tour
                        invoice_number = Guid.NewGuid().ToString(),
                        amount = new Amount
                        {
                            currency = "USD",
                            total = amount.ToString("0.00") // Ensure two decimal places
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    cancel_url = cancelUrl,
                    return_url = returnUrl
                }
            };

            // Create the payment
            var createdPayment = payment.Create(apiContext);

            // Update the booking with payment details
            booking.IsConfirmed = true; // Set booking as confirmed
            // Here, you can add any additional fields related to payment if needed, like PaymentStatus.
            _context.Bookings.Add(booking); // Add the booking to the DbContext
            _context.SaveChanges(); // Save changes to the database

            return createdPayment;
        }
    }
}
