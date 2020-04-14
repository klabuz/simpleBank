using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;
using System.Diagnostics;

namespace SimpleBank.Controllers
{
    public class DashboardController : Controller
    {
        private SimpleBankContext _context;

        public DashboardController(SimpleBankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}