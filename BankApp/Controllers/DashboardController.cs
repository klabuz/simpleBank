﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SimpleBank.Models;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
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
        [Route("dashboard")]
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

            var transactions = _context.Transactions.Where(u => u.SenderId == currentUser.UserId).ToList();


            var transactionsHistory = (from t in transactions
                        join a in accounts on t.ToAccountId equals a.AccountId
                        join b in accounts on t.AccountId equals b.AccountId
                        select new SelfTransaction
                        {
                            ToAccount = a.Name,
                            FromAccount = b.Name,
                            Amount = t.Amount,
                            isSelfTransfer = t.isSelfTransfer
                        }).ToList();


            ViewBag.accounts = accounts;
            ViewBag.transactions = transactionsHistory;

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
        [Route("transfer")]
        public IActionResult Transfer(int accountId, int userId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            var currentUser = _context.Users.Where(u => u.UserId == userId).SingleOrDefault();

            var accountsAvailable = _context.Accounts.Where(i => i.AccountId != accountId)
                                                     .Where(u => u.UserId == userId)
                                                     .ToList();

            var transferFrom = _context.Accounts.Where(i => i.AccountId == accountId)
                                                .SingleOrDefault();

            ViewBag.From = transferFrom;
            ViewBag.Available = accountsAvailable;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}