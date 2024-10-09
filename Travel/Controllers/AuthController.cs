using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Net;
using Travel.Models;
using Travel.Services;

namespace TravelBookingModels.Controllers
{
    public class AuthController : Controller
    {
        private readonly TravelDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AuthController(TravelDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }





        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new ApplicationUser object
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                // Create the user with the UserManager
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.Name, user.FullName),
                      new Claim(ClaimTypes.NameIdentifier, user.Id), // Use user.Id for the identifier
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in with cookies
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    // Redirect to the login 
                    return RedirectToAction("Login", "Auth");
                }

                // Add errors to ModelState if the user creation fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Return the view with the model if we reach here
            return View(model);
        }




        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        // Create claims for the user
                        var claims = new List<Claim>
                        {
                           new Claim(ClaimTypes.Name, user.FullName), // Adjust to your property
                           new Claim(ClaimTypes.NameIdentifier, user.Id), // Assuming 'Id' is the primary key
                     
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        // Sign in the user with cookies
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : (DateTimeOffset?)null // Set expiration based on RememberMe
                        });




                        // Store user info in session
                        HttpContext.Session.SetString("UserId", user.Id);
                        return RedirectToAction("Index", "Home"); // Redirect to a secure area


                    }
                }
            }
            return View(model);
        }



        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> PasswordManagement(PasswordManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsReset)
                {
                    // Process password reset
                    var user = await _userManager.FindByIdAsync(model.UserId);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                        return View(model);
                    }

                    var decodedToken = WebUtility.UrlDecode(model.Token);
                    var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

                    if (result.Succeeded)
                    {
                        ViewBag.Message = "Your password has been reset successfully.";
                        return View(model); // Display success message
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    // Process forgot password
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                        return View(model);
                    }

                    // Generate the reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetLink = Url.Action("PasswordManagement", "Auth", new { userId = user.Id, token = WebUtility.UrlEncode(token), isReset = true }, Request.Scheme);

                    // After generating the reset link

                    await _emailService.SendEmailAsync(model.Email, "Reset Your Password", $"Please reset your password by clicking here: <a href='{resetLink}'>link</a>");

                    // Display the reset link on the page or send via email
                    ViewBag.ResetLink = resetLink;
                    ViewBag.Message = "A password reset link has been sent to your email.";
                }
            }

            return View(model);
        }



    }
        
}

