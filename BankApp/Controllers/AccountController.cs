using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
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
        [Route("transfer/{accountId}")]
        public IActionResult TransferMoney(TransferViewModel transfer)
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

            return RedirectToAction("Details", "Account", new { accountId = fromAccount.AccountId });
        }

        [HttpGet]
        [Route("details/{accountId}")]
        public IActionResult Details(int accountId)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = currentUserId;
            var currentUser = _context.Users.Where(u => u.UserId == currentUserId).SingleOrDefault();
            var accountInfo = _context.Accounts.Where(i => i.AccountId == accountId).SingleOrDefault();
            var accounts = _context.Accounts.ToList();
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
                                           Sender = y.UserName,
                                           ReceiverId = t.ReceiverId,
                                           Receiver = u.UserName,
                                           CreatedDate = t.CreatedDate
                                       }).ToList().OrderByDescending(d => d.CreatedDate);

            ViewBag.transactions = transactionsHistory;
            ViewBag.account = accountInfo;

            return View();
        }

        [HttpGet]
        [Route("mainaccount/{accountId}")]
        public IActionResult MainAccount(int accountId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var account = _context.Accounts.Where(u => u.UserId == currentUserId).ToList();
            var checkZelleAccount = _context.Accounts.Where(u => u.UserId == currentUserId)
                                                .Where(m => m.isMain == true)
                                                .SingleOrDefault();
            var mainAccount = _context.Accounts.Where(u => u.AccountId == accountId).SingleOrDefault();

            if (checkZelleAccount != null)
            {
                checkZelleAccount.isMain = false;
            }
         
            mainAccount.isMain = true;
            _context.SaveChanges();

            return RedirectToAction("Details", new { accountId, userId = currentUserId });
        }


        [HttpPost]
        [Route("editaccount/{accountId}")]
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

        [HttpPost]
        [Route("pay/{accountId}")]
        public IActionResult Pay(TransferViewModel transfer, int accountId)
        {
            PayTransfer PAY = transfer.Pay;
            //Reads current user id from session
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                bool hasZelle = false; 
                var allUserAccounts = _context.Accounts.Where(u => u.UserId == currentUserId).ToList();
                foreach(var z in allUserAccounts)
                {
                    if(z.isMain == true)
                    {
                        hasZelle = true;  
                    }
                }

                var fromAccount = _context.Accounts.Where(u => u.AccountId == accountId).SingleOrDefault();
                if(!hasZelle){
                    ModelState.AddModelError("PAY.ReceiverUsername", "You need to activate Zelle account.");

                    return RedirectToAction("Pay", "Dashboard", new { accountId });
                }

                var amount = PAY.Amount;
             
                var receiver = _context.Users.Where(u => u.UserName == PAY.ReceiverUsername).SingleOrDefault();
                if (receiver == null)
                {
                    ModelState.AddModelError("PAY.ReceiverUsername", "This username doesn't exist.");

                    return RedirectToAction("Pay", "Dashboard", new { accountId });
                }

                var receiverZelleAccount = _context.Accounts.Where(m => m.isMain == true)
                                                            .Where(i => i.UserId == receiver.UserId)
                                                            .SingleOrDefault();
                if (receiverZelleAccount == null)
                {
                    ModelState.AddModelError("PAY.ReceiverUsername", "This username exist but doesn't have activated Zelle account.");

                    return RedirectToAction("Pay", "Dashboard", new { accountId });
                }

                fromAccount.Balance -= amount;
                receiverZelleAccount.Balance += amount;

                if (fromAccount.Balance >= 0)
                {
                    _context.SaveChanges();

                    Transaction newTransaction = new Transaction()
                    {
                        SenderId = (int)currentUserId,
                        ReceiverId = (int)receiver.UserId,
                        Amount = amount,
                        AccountId = fromAccount.AccountId,
                        ToAccountId = receiverZelleAccount.AccountId,
                    };

                    _context.Transactions.Add(newTransaction);
                    _context.SaveChanges();
                }

                return RedirectToAction("Details", "Account", new { accountId = fromAccount.AccountId, userId = currentUserId});
            }
            return RedirectToAction("Pay", "Dashboard", new { accountId });
        }

        [HttpPost]
        [Route("details/{accountId}")]
        public IActionResult StatementPDF(AccountViewModel statement)
        {
            Statement STAT = statement.Stat;

            DateTime startDate = STAT.startDate;
            DateTime endDate = STAT.endDate;

            var accountId = STAT.AccountId;
            var accounts = _context.Accounts.ToList();
            var users = _context.Users.ToList();

            var accountInfo = _context.Accounts.Where(i => i.AccountId == accountId).SingleOrDefault();

            var statementTransactions = _context.Transactions.Where(u => u.AccountId == accountId || u.ToAccountId == accountId)
                                                 .Where(d => d.CreatedDate >= startDate)
                                                 .Where(de => de.CreatedDate <= endDate)
                                                 .ToList();

            var transactionsHistory = (from t in statementTransactions
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
                                           Sender = y.UserName,
                                           ReceiverId = t.ReceiverId,
                                           Receiver = u.UserName,
                                           CreatedDate = t.CreatedDate
                                       }).ToList().OrderByDescending(d => d.CreatedDate);

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Statement generated from simpleBank.";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font14 = new XFont("Verdana", 14, XFontStyle.BoldItalic);
            XFont font10 = new XFont("Verdana", 10, XFontStyle.BoldItalic);

            // Draw the text
            gfx.DrawString($"This is a {accountInfo.Name} account statement " +
                $"from {startDate.ToShortDateString()} " +
                $"to {endDate.ToShortDateString()}"
            ,font14, XBrushes.Black,
            new XRect(10, 10, page.Width, page.Height),
            XStringFormats.TopLeft);
            
            // Save the document...
            string filename = accountInfo.Name.Trim() + "_Statement_" + startDate.ToShortDateString().Trim().Replace('/','_') + ".pdf";
            
            document.Save(filename);

            return RedirectToAction("Details", "Account", new { accountId });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}