using Microsoft.AspNetCore.Http;
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

            return RedirectToAction("Dashboard", "Dashboard", new { userId = currentUserId });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}