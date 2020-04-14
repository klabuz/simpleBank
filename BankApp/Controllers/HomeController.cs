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
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}