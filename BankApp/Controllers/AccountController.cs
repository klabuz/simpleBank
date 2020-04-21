﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace SimpleBank.Controllers
{
    public class AccountController : Controller
    {
        private SimpleBankContext _context;

        public AccountController(SimpleBankContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("createaccount")]
        public IActionResult CreateAccount(AccountViewModel account)
        {
            Create CRE = account.Cre;
            //Reads current user id from session
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            if (ModelState.IsValid)
            {
                string cardNumber = "1992";
                //Generate random Card Number
                for (var i = 0; i < 12; i++)
                {
                    Random r = new Random();
                    string n = r.Next(10).ToString();
                    cardNumber += n;
                }

                string pinNumber = "";
                //Generate random Card Number
                for (var i = 0; i < 4; i++)
                {
                    Random r = new Random();
                    string n = r.Next(10).ToString();
                    pinNumber += n;
                }

                Account newAccount = new Account()
                {
                    Type = CRE.Type,
                    Name = CRE.Name,
                    Balance = CRE.Balance,
                    CardNumber = cardNumber,
                    PIN = pinNumber,
                    isMain = false,
                    UserId = (int)currentUserId,          
                };

                _context.Add(newAccount);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Dashboard", new { userId = currentUserId });
            }
            return RedirectToAction("CreateAccount", "Dashboard", new { userId = currentUserId });
        }

        [HttpPost]
        [Route("transfer")]
        public IActionResult TransferMoney(SelfTransferViewModel transfer)
        {
            SelfTransfer TRA = transfer.Tra;
            //Reads current user id from session
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var amount = TRA.Amount;
            var fromAccount = _context.Accounts.Where(u => u.AccountId == TRA.FromAccount).SingleOrDefault();
            var toAccount = _context.Accounts.Where(u => u.AccountId == TRA.ToAccount).SingleOrDefault();

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            if (fromAccount.Balance >= 0)
            {
                _context.SaveChanges();

                Transaction newTransaction = new Transaction()
                {
                    SenderId = (int)currentUserId,
                    ReceiverId = (int)toAccount.UserId,
                    Amount = TRA.Amount,
                    AccountId = fromAccount.AccountId,
                    ToAccountId = toAccount.AccountId,
                    isSelfTransfer = true
                };

                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", "Account", new { accountId = fromAccount.AccountId, userId = currentUserId });
        }

        [HttpGet]
        [Route("details")]
        public IActionResult Details(int accountId, int userId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var currentUser = _context.Users.Where(u => u.UserId == userId).SingleOrDefault();
            var accountInfo = _context.Accounts.Where(i => i.AccountId == accountId).SingleOrDefault();
            var accounts = _context.Accounts.Where(u => u.UserId == userId).ToList();
            var transactions = _context.Transactions.Where(i => i.AccountId == accountId || i.ToAccountId == accountId).ToList();
            var users = _context.Users.ToList();

            var transactionsHistory = (from t in transactions
                                       join a in accounts on t.ToAccountId equals a.AccountId
                                       join b in accounts on t.AccountId equals b.AccountId
                                       join u in users on a.UserId equals u.UserId
                                       join y in users on b.UserId equals y.UserId

                                       select new TransactionsHistory
                                       {
                                           ToAccountId = a.AccountId,
                                           ToAccount = a.Name,
                                           FromAccountId = b.AccountId,
                                           FromAccount = b.Name,
                                           Amount = t.Amount,
                                           isSelfTransfer = t.isSelfTransfer,
                                           SenderId = t.SenderId,
                                           Sender = u.UserName,
                                           ReceiverId = t.ReceiverId,
                                           Receiver = y.UserName,
                                           CreatedDate = t.CreatedDate
                                       }).ToList().OrderByDescending(d => d.CreatedDate);

            ViewBag.transactions = transactionsHistory;
            ViewBag.account = accountInfo;

            return View(currentUser);
        }

        [HttpGet]
        [Route("mainaccount")]
        public IActionResult MainAccount(int accountId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var account = _context.Accounts.Where(u => u.UserId == currentUserId).ToList();

            foreach(var a in account)
            {
                if (a.isMain == true)
                {
                    a.isMain = false;
                }
            }

            var mainAccount = _context.Accounts.Where(u => u.AccountId == accountId).SingleOrDefault();
            mainAccount.isMain = true;
            _context.SaveChanges();


            return RedirectToAction("Details", new { accountId, userId = currentUserId });
        }


        [HttpPost]
        [Route("editaccount")]
        public IActionResult EditAccount(AccountViewModel account, int accountId)
        {
            Edit EDI = account.Edi;
            
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = currentUserId;

            if (ModelState.IsValid)
            {
                var accountEdit = _context.Accounts.Where(i => i.AccountId == accountId).SingleOrDefault();

                if (!String.IsNullOrEmpty(EDI.Name))
                {
                    accountEdit.Name = EDI.Name;
                }
                if (EDI.PIN != null)
                {
                    accountEdit.PIN = EDI.PIN;
                }

                _context.SaveChanges();

                return RedirectToAction("Details", new { accountId, userId = currentUserId });
            }
            return RedirectToAction("EditAccount", "Dashboard", new { accountId });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}