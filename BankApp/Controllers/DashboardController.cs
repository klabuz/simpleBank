using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;

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
        public ActionResult Index()
        {
            return View();
        }
    }
}