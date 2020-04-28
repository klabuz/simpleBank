using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RestSharp;
using SimpleBank.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Transactions;

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
        [Route("dashboard/{userId}")]
        public IActionResult Dashboard(int userId)
        {
            var currentUser = _context.Users.Where(u => u.UserId == userId)
                                            .SingleOrDefault();

            //Reads current user id from session
            ViewBag.CurrentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUser != null)
            {
                ViewBag.UserId = currentUser.UserId;
                ViewBag.UserName = currentUser.UserName;
            }

            var accounts = _context.Accounts.Where(i => i.UserId == currentUser.UserId)
                                            .ToList();

            ViewBag.accounts = accounts;

            return View("Index", currentUser);
        }

        [HttpGet]
        [Route("createaccount")]
        public IActionResult CreateAccount()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }

        [HttpGet]
        [Route("editaccount/{accountId}")]
        public IActionResult EditAccount(int accountId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var accountEdit = _context.Accounts.Where(i => i.AccountId == accountId).SingleOrDefault();
            ViewBag.account = accountEdit;
            return View();
        }

        [HttpGet]
        [Route("transfer/{accountId}")]
        public IActionResult Transfer(int accountId)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = currentUserId;

            var currentUser = _context.Users.Where(u => u.UserId == currentUserId).SingleOrDefault();

            var accountsAvailable = _context.Accounts.Where(i => i.AccountId != accountId)
                                                     .Where(u => u.UserId == currentUserId)
                                                     .ToList();

            var transferFrom = _context.Accounts.Where(i => i.AccountId == accountId)
                                                .SingleOrDefault();

            ViewBag.From = transferFrom;
            ViewBag.Available = accountsAvailable;

            return View(currentUser);
        }

        [HttpGet]
        [Route("pay/{accountId}")]
        public IActionResult Pay(int accountId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var fromAccount = _context.Accounts.Where(i => i.AccountId == accountId).SingleOrDefault();
            ViewBag.account = fromAccount;
            return View();
        }

        [HttpGet]
        [Route("stockdashboard/{accountId}")]
        public IActionResult StockDashboard(Stock searchedStock, int accountId)
        {
            var fromAccount = _context.Accounts.Where(u => u.AccountId == accountId).SingleOrDefault();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.From = fromAccount;

            var stock = searchedStock;
            


            return View();
        }


        [HttpPost]
        [Route("stockdashboard/{accountId}")]
        public IActionResult StockDetails(StockViewModel stock)
        {
            StockSymbol SS = stock.SS;

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            string stockSymbol = SS.Symbol;

            var client = new RestClient("https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/get-detail?region=US&lang=en&symbol=" + stockSymbol);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "e5ea64e17emshfcf36b3cfdb9bfdp1113e0jsnb6675049f129");
            IRestResponse searchedStock = client.Execute(request);

            var deserializedStock = JsonConvert.DeserializeObject(searchedStock.Content);

            ViewBag.stock = stock;

            return RedirectToAction("StockDashboard", searchedStock);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}