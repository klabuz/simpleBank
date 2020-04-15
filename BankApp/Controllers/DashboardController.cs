using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;
using System.Diagnostics;
using System.Linq;

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
        public IActionResult Dashboard(int userId)
        {
            var currentUser = _context.Users.Where(u => u.UserId == userId);

            ViewBag.UserId = userId;

            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}