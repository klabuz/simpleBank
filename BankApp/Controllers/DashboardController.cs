using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SimpleBank.Models;
using System;
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

        public DashboardController()
        {
        }

        [HttpGet]
        [Route("dashboard/{userId}")]
        public IActionResult Dashboard(int userId)
        {
            //Reads current user id from session
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUserId != null)
            {
                ViewBag.UserId = currentUserId;
                ViewBag.UserName = _context.Users.Where(u => u.UserId == currentUserId).SingleOrDefault().UserName;
            }

            var accounts = _context.Accounts.Where(i => i.UserId == currentUserId).ToList();                                      

            ViewBag.accounts = accounts;

            return View("Index");
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
        public IActionResult StockDashboard(int accountId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            var fromAccount = _context.Accounts.Where(u => u.AccountId == accountId).SingleOrDefault();
            var allStocks = _context.Stocks.ToList();

            ViewBag.From = fromAccount;
            ViewBag.allStocks = allStocks;

            return View("StockDashboard", accountId);
        }

        [HttpPost]
        [Route("stockdashboard/{accountId}")]
        public IActionResult StockDetails(StockViewModel stock, int accountId)
        {
            StockSymbol SS = stock.SS;

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            string stockSymbol = SS.Symbol;

            var client = new RestClient("https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/get-detail?region=US&lang=en&symbol=" + stockSymbol);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "e5ea64e17emshfcf36b3cfdb9bfdp1113e0jsnb6675049f129");
            IRestResponse searchedStock = client.Execute(request);

            var deserializedStock = JsonConvert.DeserializeObject<stockInfo>(searchedStock.Content);

            if (deserializedStock != null && deserializedStock.price != null)
            {
                var searchingStock = _context.Stocks.Where(s => s.Symbol == deserializedStock.symbol).SingleOrDefault();
                int stockId = 0;

                if (searchingStock == null)
                {
                    Stock newStock = new Stock()
                    {
                        Name = deserializedStock.price.shortName,
                        Symbol = deserializedStock.symbol.ToUpper(),
                        Price = deserializedStock.price.regularMarketPrice.fmt,
                        PreMarketPrice = deserializedStock.price.preMarketPrice.fmt
                    };
                    _context.Stocks.Add(newStock);
                }
                else
                {
                    searchingStock.Price = deserializedStock.price.regularMarketPrice.fmt;
                    searchingStock.PreMarketPrice = deserializedStock.price.preMarketPrice.fmt;
                    stockId = searchingStock.StockId;
                }

                _context.SaveChanges();

                if(searchingStock == null && stockId == 0)
                {
                    stockId = _context.Stocks.Where(s => s.Symbol == deserializedStock.symbol).SingleOrDefault().StockId;
                }

                return RedirectToAction("StockDetails", new { stockId, accountId });
            }

            return RedirectToAction("StockDashboard", new { accountId });
        }

        [HttpGet]
        [Route("stockdetails/{stockId}/{accountId}")]
        public IActionResult StockDetails(int stockId, int accountId)
        {
            var fromAccount = _context.Accounts.Where(u => u.AccountId == accountId).SingleOrDefault();
            var stockInfo = _context.Stocks.Where(s => s.StockId == stockId).SingleOrDefault();

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.stock = stockInfo;
            ViewBag.From = fromAccount;

            return View();
        }

        //Simple calculator method for unit testing
        public int Add(int x, int y)
        {
            return x + y;
        }
            
            public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}