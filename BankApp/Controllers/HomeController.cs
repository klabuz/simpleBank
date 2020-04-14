using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;
using System.Linq;

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
        public ActionResult Index()
        {
            return View();
        }
    }
}