using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;
using System.Diagnostics;

namespace SimpleBank.Controllers
{
    public class HomeController : Controller
    {
        private SimpleBankContext _context;

        public HomeController(SimpleBankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }

        [HttpGet]
        [Route("signup")]
        public IActionResult SignUp()
        {
            return View("SignUp");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            if (TempData["ModelState"] != null)
            {
                var errorMessage = TempData["ModelState"];
            }

            return View("Login");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}