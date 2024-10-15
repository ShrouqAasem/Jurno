using Microsoft.AspNetCore.Mvc;

namespace Travel.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.PageTitle = "About";
            return View();
        }
    }
}
